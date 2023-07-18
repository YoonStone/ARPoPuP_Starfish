using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yo_TrashMission : MonoBehaviour
{
    [Header("챕터4")]
    public GameObject chapter4;

    [Header("날아오는 쓰레기 개수")]
    public int trashCount;
    [Header("넣어야하는 쓰레기 최소개수")]
    public int trash_Min;
    [Header("쓰레기 간의 간격 (시간)")]
    public float nextTime;

    [Header("스코어 팻말")]
    public GameObject score;
    [Header("팻말 밑 빨강,초록불")]
    public GameObject lights;
    [Header("팻말 점수 숫자")]
    public GameObject[] scoreNumber;

    [HideInInspector]
    public int goalCount;

    int preTrashNumber, rand;
    public bool isLightOpen;

    Yo_Chapter_4 CH4;
    Animator panel_anim, light_animR, light_animG;

    void Start()
    {
        CH4 = chapter4.GetComponent<Yo_Chapter_4>();
        panel_anim = score.GetComponent<Animator>();
        light_animR = lights.transform.GetChild(0).GetComponent<Animator>();
        light_animG = lights.transform.GetChild(1).GetComponent<Animator>();
    }

    public void RealStart()
    {
        trashCount = 15;
        nextTime = 2;
        goalCount = 0;
        // 팻말 초기화
        NumberSetting(goalCount);

        // 스코어 팻말 나오기
        score.GetComponent<Animator>().SetTrigger("IsOpen");

        // 불 나오기
        lights.SetActive(true);
        isLightOpen = true;

        // 카운트 후 시작
        Invoke("Spawn", 2);
    }

    public void Spawn()
    {
        if (!CH4.popup.isReset)
        {
            switch (preTrashNumber)
            {
                // 처음이라면 완전 랜덤
                case 0:
                    rand = Random.Range(1, 4);
                    Load(rand); break;

                // 이전 쓰레기가 1번이었다면 2,3
                case 1:
                    rand = Random.Range(0, 2);
                    if (rand == 0) Load(2);
                    else Load(3); break;

                // 이전 쓰레기가 2번이었다면 1,3
                case 2:
                    rand = Random.Range(0, 2);
                    if (rand == 0) Load(1);
                    else Load(3); break;

                // 이전 쓰레기가 3번이었다면 1,2
                case 3:
                    rand = Random.Range(0, 2);
                    if (rand == 0) Load(1);
                    else Load(2); break;
            }
        }
    }

    public void Load(int trashNumber)
    {
        // 랜덤한 쓰레기 고르기
        int rand2 = Random.Range(1, 4);
        GameObject trash = Resources.Load((trashNumber + "_" + rand2).ToString()) as GameObject;
        Instantiate(trash, transform);

        // 랜덤한 각도
        trash.transform.localRotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));

        // 쓰레기 던지는 효과음
        CH4.audioCtrl.TrashThrow();

        // 이전 쓰레기통 번호에 지금 쓰레기통 번호 넣기
        preTrashNumber = trashNumber;

        // 남은 쓰레기 수 -1
        trashCount--;

        // 던지는 간격 줄이기
        nextTime = nextTime - 0.05f;

        // 다 던진 게 아니라면 다시 스폰
        if (trashCount > 0) Invoke("Spawn", nextTime);

        // 다 던졌다면 5초 후에 결과 확인
        else Invoke("Result", 5f);
    }

    // 5초 후 결과 확인
    public void Result()
    {
        if (!CH4.popup.isReset)
        {
            // 만약 넣은 개수가 OO개 이상이라면 성공
            if (goalCount >= trash_Min) StartCoroutine("Success");

            // 아니라면 실패
            else StartCoroutine("Fail");
        }
    }

    // 성공하면
    public IEnumerator Success()
    {
        // 팻말 커졌다가 작아지기 + 불 점프 흔들흔들
        yield return new WaitForSeconds(0f);
        panel_anim.SetTrigger("IsBig");
        light_animG.SetTrigger("IsJump");

        // 스코어 팻말 들어가기 + 불 들어가기
        yield return new WaitForSeconds(3f);
        panel_anim.SetTrigger("IsClose");
        isLightOpen = false;

        // 팝업종이 들어가기
        CH4.paperNumber = 1;
        CH4.isPaperOpen = false;

        // 스코어 팻말 없어지기 + 바다 깨끗하게 하기
        yield return new WaitForSeconds(2);
        CH4.StartCoroutine("CleanSea");
    }

    // 실패하면
    public IEnumerator Fail()
    {
        // 팻말 커졌다가 작아지기 + 불 점프 흔들흔들
        yield return new WaitForSeconds(0f);
        panel_anim.SetTrigger("IsBig");
        light_animR.SetTrigger("IsJump");

        // 점수 초기화
        yield return new WaitForSeconds(3f);
        trashCount = 15;
        nextTime = 2;
        goalCount = 0;

        // 팻말 초기화
        NumberSetting(goalCount);

        // 카운트 후 재시작
        Invoke("Spawn", 2);
    }

    // 팻말 점수 설정
    public void NumberSetting(int score)
    {
        for (int i = 0; i < scoreNumber.Length; i++)
        {
            scoreNumber[i].SetActive(false);
        }

        scoreNumber[score].SetActive(true);
    }

    private void Update()
    {
        // 불 올라오기
        if (isLightOpen)
        {
            lights.transform.localScale = Vector3.Lerp(lights.transform.localScale, new Vector3(1, 1, 1), Time.deltaTime * 0.5f);
        }

        // 불 내려가기
        else
        {
            lights.transform.localScale = Vector3.Lerp(lights.transform.localScale, new Vector3(1, 0, 1), Time.deltaTime * 0.5f);
        }
    }
}
