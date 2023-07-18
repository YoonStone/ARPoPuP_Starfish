using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 별 미션
// 1. 랜덤으로 순서 정하기
// 2. 순서대로 소리내기 (도,레,미,파,솔)
// 2. 순서대로 머테리얼 바꾸기 (빨,주+노,초,파,보)
// 3. 머테리얼 바뀌면 불빛 반짝

public class Yo_StarMissionCtrl : MonoBehaviour
{
    [Header("별 하나하나")]
    public GameObject[] stars;
    [Header("피아노 종이")]
    public GameObject[] piano;

    [Header("불가사리")]
    public GameObject starfish;
    [Header("불가사리 실")]
    public GameObject line;
    [Header("불가사리 옆 별")]
    public GameObject star_Left;


    [Header("챕터2 스크립트")]
    public GameObject chapter2;

    [Header("POPUP BGM")]
    public GameObject bgm;

    [Header("별 머테리얼")]
    public Material[] materials;

    Animator left_anim;
    Yo_Chapter_2 CH2;
    Yo_AudioCtrl audioCtrl;
    Yo_PopupCtrl popup;

    [HideInInspector]
    public bool isMission, isEnd;
    private bool isRight, isLineUp;
    private readonly int[] rand = new int[5];
    int[] answer = new int[5];

    [HideInInspector]
    public int sequence;

    private void Start()
    {
        CH2 = chapter2.GetComponent<Yo_Chapter_2>();
        audioCtrl = bgm.GetComponent<Yo_AudioCtrl>();
        popup = bgm.GetComponent<Yo_PopupCtrl>();
    }

    // 별 색상 초기화
    public void StarReset()
    {
        isMission = false;
        isLineUp = false;

        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].GetComponent<Renderer>().material = materials[0];
        }
    }

    public void RealStart()
    {
        left_anim = star_Left.GetComponent<Animator>();
        left_anim.enabled = false;
        line.transform.localPosition = new Vector3(0.34f, 9, -1.33f);
    }

    // 도레미파솔 + 빛 미션 시작
    public void StarMission()
    {
        // 초기화
        sequence = 0;
        isEnd = false;
        isRight = true;

        // 시작하면 배경음 줄이기
        isMission = true;

        MakeRandom();

        // 개발자용) 바로 넘어가기
        //StartCoroutine("MissionEnding");
    }

    // 랜덤한 순서 정하기
    public void MakeRandom()
    {
        for (int i = 0; i < 5; i++)
        {
            // 0~4 중 랜덤으로 저장
            rand[i] = Random.Range(0, 5);

            // 중복제거
            for (int j = 0; j < i; j++)
            {
                // 이전이랑 똑같다면 i를 낮춰서 다시 반복하도록
                if (rand[i] == rand[j]) i--;
            }
        }

        StartCoroutine("MissionStart");
    }

    // 도레미파솔라시도
    public IEnumerator MissionStart()
    {
        // 3초 후에, 첫번째 별 반짝 + 사운드 
        yield return new WaitForSeconds(2);
        stars[rand[0]].GetComponent<Renderer>().material = materials[rand[0] + 1];
        audioCtrl.PianoSound(rand[0]);

        // 이전 사운드가 끝나면, 다음 별 반짝 + 사운드 (그 전 별은 다시 노란색으로) 반
        for (int i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(1);
            stars[rand[i]].GetComponent<Renderer>().material = materials[0];
            stars[rand[i+1]].GetComponent<Renderer>().material = materials[rand[i + 1] + 1];
            audioCtrl.PianoSound(rand[i + 1]);
        }

        // 솔 사운드가 끝나면, 그 전 별 다시 노란색으로
        yield return new WaitForSeconds(1);
        stars[rand[4]].GetComponent<Renderer>().material = materials[0];

        // 별 스스로 다 보여줬다면 플레이어가 누르길 기다림
        sequence = 1;
    }

    public void PlayerMission()
    {
        if (!popup.isReset)
        {
            // 순서 진행 중이라면
            if (sequence != 0)
            {
                // 건반종이 보여주기
                CH2.paperNumber = 1;
                CH2.isPaperOpen = true;
            }

            // 진행 중이지 않고 건반종이가 나와있다면 건반종이 들어가기
            else if (CH2.paperNumber == 1) CH2.isPaperOpen = false;
        }
    }

    //int shineMyself = 0; // 초기화

    // 플레이어가 별 누르면 반짝
    public void Shining(int starNumber)
    {
        answer[sequence - 1] = starNumber;

        // 별 색깔 반짝하고
        stars[starNumber].GetComponent<Renderer>().material = materials[starNumber + 1];
        // 소리 나오기
        audioCtrl.PianoSound(starNumber);
        // 효과음 후, 그 전 별 다시 노란색으로
        StartCoroutine("BackToYellow", starNumber);

        sequence++;

        // 마지막 5번째를 쳤다면
        if(sequence == 6)
        {
            // 맞았는지 틀렸는지 판정
            for (int i = 0; i < 5; i++)
            {
                if (rand[i] != answer[i])
                {
                    isRight = false;
                    StartCoroutine("Fail"); // 실패
                    break;
                }
            }

            if (isRight) StartCoroutine("MissionEnding"); // 성공
        }
    }

    // 효과음 후, 그 전 별 다시 노란색으로
    public IEnumerator BackToYellow(int starNumber)
    {  
        yield return new WaitForSeconds(1);
        // 틀렸거나 맞추면 노란색으로 돌아가지 않도록
        if(!isEnd)
            stars[starNumber].GetComponent<Renderer>().material = materials[0];
    }

    // 틀렸을 경우
    public IEnumerator Fail()
    {
        // 다같이 빨간색으로 변하기
        yield return new WaitForSeconds(1);
        sequence = 0;
        isEnd = true;

        for (int i = 0; i < 5; i++)
        {
            stars[rand[i]].GetComponent<Renderer>().material = materials[1];
        }

        // 땡 음 내기
        audioCtrl.PianoSound(5);

        // 진동이 켜져있다면
        if (GameObject.FindWithTag("EditorOnly").GetComponent<Yo_SettingCtrl>().isViabrate)
        {
            // 진동오기
#if UNITY_ANDROID
            Handheld.Vibrate();
#endif
        }

        // 효과음 후, 그 전 별 다시 노란색으로 + 재시작
        yield return new WaitForSeconds(audioCtrl.audio_PianoResult[0].length);
        for (int i = 0; i < 5; i++)
        {
            stars[rand[i]].GetComponent<Renderer>().material = materials[0];
        }

        StarMission();
    }

    // 미션 엔딩
    public IEnumerator MissionEnding()
    {
        // 딩동댕동 음 내기
        yield return new WaitForSeconds(1);
        sequence = 0;
        audioCtrl.PianoSound(6);

        // 건반 들어가기
        CH2.paperNumber = 1;
        CH2.isPaperOpen = false;

        // 딩동댕동 후에 배경음 키우기
        yield return new WaitForSeconds(2);
        isMission = false;
        
        // 불가사리 연결된 실 올라가기
        isLineUp = true;

        // 실 다 올라가면 옆에 별 흔들리기
        yield return new WaitForSeconds(1);
        left_anim.enabled = true;

        // 옆에 별이 불가사리를 치면 불가사리 날아가기
        yield return new WaitForSeconds(2.5f);
        starfish.GetComponent<Rigidbody>().isKinematic = false;
        starfish.GetComponent<Rigidbody>().AddForce(Vector3.right * 400 + Vector3.up * 200);
        // 뽁 효과음
        audioCtrl.StarEffect();
        CH2.ShowDialog(17);
    }

    private void Update()
    {
        // 미션 중이라면
        if (isMission)
        {
            // 배경음 0.1로 감소하기
            audioCtrl.bgmAuto = Mathf.Lerp(audioCtrl.bgmAuto, 0.1f, Time.deltaTime * 1);
            PlayerMission();
        }
        // 아니라면
        else
        {
            // 배경음 0.6으로 증가하기
            audioCtrl.bgmAuto = Mathf.Lerp(audioCtrl.bgmAuto, 0.6f, Time.deltaTime * 1);
        }

        // 불가사리 줄 올라가기
        if (isLineUp)
        {
            line.transform.localPosition = Vector3.Lerp(line.transform.localPosition,
                new Vector3(line.transform.localPosition.x, 10, line.transform.localPosition.z), Time.deltaTime * 1);

            starfish.transform.localPosition = Vector3.Lerp(starfish.transform.localPosition,
                new Vector3(starfish.transform.localPosition.x, 3.5f, starfish.transform.localPosition.z), Time.deltaTime * 1);
        }
    }
}
