using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Yo_Chapter_3 : MonoBehaviour
{
    [Header("팝업매니저")]
    public GameObject popupMng;
    [Header("자막")]
    public GameObject textUI;
    [Header("팝업종이")]
    public GameObject paper;

    [Header("불가사리")]
    public GameObject starfish;
    public Transform starfishPos;
    
    [Header("난로 파티클")]
    public GameObject fireplace;
    
    [Header("리스폰 유아이")]
    public GameObject respawnUI;

    [Header("벽들")]
    public GameObject[] walls;
    [Header("창문들")]
    public GameObject[] windows;
    [Header("전구줄")]
    public GameObject lightLine;
    [Header("전구들")]
    public GameObject[] lights;
    [Header("전구불빛들")]
    public GameObject[] light_Images;
    [Header("레드카펫")]
    public GameObject carpet;
    [Header("선물들")]
    public GameObject[] gifts;
    [Header("트리 위 전구")]
    public GameObject starLight;

    [Header("오른쪽 맵")]
    public Transform map_R;

    [Header("현재 팝업종이")]
    public int paperNumber = 0;
    public bool isPaperOpen;

    bool isStarfishWalk, isVolumeUp;

    Yo_TextCtrl dialog;
    public Yo_PopupCtrl popup;
    Yo_SettingCtrl setting;
    Yo_AudioCtrl audioCtrl;

    private void Start()
    {
        dialog = textUI.GetComponent<Yo_TextCtrl>();
        popup = popupMng.GetComponent<Yo_PopupCtrl>();
        setting = popupMng.GetComponent<Yo_SettingCtrl>();
        audioCtrl = popupMng.GetComponent<Yo_AudioCtrl>();

        //// 텍스트 머테리얼 바꾸기
        //dialog.MaterialChange();
    }

    public void Start1()
    {
        isStarfishWalk = false;
        // 불가사리 초기화 + 안 보이게
        starfish.GetComponent<Yo_Starfish_3>().RealStart();
        starfish.GetComponent<Rigidbody>().isKinematic = true;
        starfish.transform.GetChild(0).gameObject.SetActive(false);
        starfish.transform.localPosition = starfishPos.localPosition;
        starfish.transform.localRotation = starfishPos.localRotation;

        // 전구 불 한꺼번에 탁 켜지기
        for (int i = 0; i < light_Images.Length; i++)
        {
            light_Images[i].GetComponent<Image>().color = new Vector4(1, 1, 1, 0);
        }
        // 선물 미션
        respawnUI.SetActive(false);
        // 파티클
        fireplace.SetActive(false);
        // 팝업종이 및 상호작용 오브젝트 상태 초기화
        paperNumber = 0;
        isPaperOpen = false;
        paper.transform.GetChild(0).localPosition = new Vector3(0, 0, 0);
        paper.transform.GetChild(1).localPosition = new Vector3(5, 0, 0);

        StartCoroutine("RealStart");
    }

    // 시작
    public IEnumerator RealStart()
    {
        // 벽 열리기
        yield return new WaitForSeconds(2f);
        walls[0].GetComponent<Animator>().enabled = true;
        walls[1].GetComponent<Animator>().enabled = true;

        // 카펫 펼쳐지기
        yield return new WaitForSeconds(1f);
        carpet.GetComponent<Animator>().enabled = true;

        // 선물 나오기
        for (int i = 0; i < gifts.Length; i++)
        {
            gifts[i].GetComponent<Animator>().enabled = true;
            yield return new WaitForSeconds(0.5f);
            // 전구 줄 보이게
            lightLine.SetActive(true);
        }

        // 창문 열리기
        windows[0].GetComponent<Animator>().enabled = true;
        windows[1].GetComponent<Animator>().enabled = true;

        // 불가사리 떨어지고
        starfish.transform.GetChild(0).gameObject.SetActive(true);
        starfish.GetComponent<Rigidbody>().isKinematic = false;

        // 전구 하나씩 생기기
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }

        // 전구 불 한꺼번에 탁 켜지기
        for (int i = 0; i < light_Images.Length; i++)
        {
            light_Images[i].GetComponent<Image>().color = new Vector4(1, 1, 1, 0.35f);
        }

        // 자막 나오기
        yield return new WaitForSeconds(1);
        print("자막 나오기 20");
        ShowDialog(20);
        //StartCoroutine("ArriveTree");
    }

    // 자막 보이게
    public void ShowDialog(int text)
    {
        if (!popup.isReset)
        {
            dialog.textNumber = text;
            dialog.TextOpen(gameObject);
        }
    }

    // 자막 닫은 후 할 일
    public void NextToDo(int textNumber)
    {
        switch (textNumber)
        {
            // 불가사리 이동
            case 22: StartCoroutine("StarfishStart"); break;

            // 전구 종이 보이게
            case 25:
                StartCoroutine("ArriveTree"); break;

            // 엔딩
            case 28:
                StartCoroutine("Ending"); break;
        }
    }

    // 불가사리 이동
    public IEnumerator StarfishStart()
    {
        // 불가사리 선물 앞으로 움직이게
        yield return new WaitForSeconds(0);
        starfish.GetComponent<Yo_Starfish_3>().GoPresent();
        starfish.GetComponent<Animator>().SetBool("IsWalk", true);

        // 움직임 끄기
        yield return new WaitForSeconds(4f);
        starfish.GetComponent<Animator>().SetBool("IsWalk", false);
    }

    // 난로 켜지기
    public void FireDelay()
    {
        // 난로 파티클 켜고
        fireplace.SetActive(true);

        // 장작소리 서서히 들리게
        isVolumeUp = true;

        // 회전, 점프종이 나오게
        paperNumber = 0;
        isPaperOpen = true;

        // 리스폰 유아이 보이게
        respawnUI.SetActive(true);
    }

    // 트리에 도착 후 전구종이 보이게
    public IEnumerator ArriveTree()
    {
        // 전구 종이 보이게
        yield return new WaitForSeconds(2f);
        paperNumber = 1;
        isPaperOpen = true;

        yield return new WaitForSeconds(0.5f);
        starLight.GetComponent<Animator>().enabled = false;

        // 불가사리 물리작용 활성화
        starfish.GetComponent<Rigidbody>().isKinematic = false;
    }

    // 전구는 불가사리를 떨어뜨렸어요
    public IEnumerator JumpEnd()
    {
        // 전구 종이 들어가기
        yield return new WaitForSeconds(0);
        paperNumber = 1;
        isPaperOpen = false;
        starLight.GetComponent<Animator>().enabled = false;

        // 글자 보이게
        yield return new WaitForSeconds(2f);
        ShowDialog(26);
    }

    public IEnumerator Ending()
    {
        if (!popup.isReset)
        {
            yield return new WaitForSeconds(0);
            popup.isBookmark = false;
            
            // 불가사리 오른쪽으로 이동
            isStarfishWalk = true;
            starfish.GetComponent<Animator>().SetBool("IsWalk", true);

            // 없어지는 애니메이션
            yield return new WaitForSeconds(1);
            StartCoroutine("Down");
        }
    }

    // 초기화
    public void ResetChapter()
    {
        popup.isReset = true;

        StartCoroutine("Down");
    }

    public IEnumerator Down()
    {
        // 불가사리 초기화
        yield return new WaitForSeconds(0f);
        starfish.transform.GetChild(0).gameObject.SetActive(false);

        // 팝업종이
        isPaperOpen = false;

        // 리스폰 유아이
        respawnUI.SetActive(false);

        // 창문
        windows[0].GetComponent<Animator>().SetTrigger("IsClose");
        windows[1].GetComponent<Animator>().SetTrigger("IsClose");

        // 벽
        yield return new WaitForSeconds(0.5f);
        walls[0].GetComponent<Animator>().SetTrigger("IsClose");
        walls[1].GetComponent<Animator>().SetTrigger("IsClose");

        // 선물 들어가고 전구 들어가기
        yield return new WaitForSeconds(0.5f);
        for (int i = gifts.Length - 1; i >= 0; i--)
        {
            gifts[i].GetComponent<Animator>().SetTrigger("IsClose");

            if (i <= 5)
            {
                lights[i].SetActive(false);
                if (i == 0)
                    lightLine.SetActive(false);

            }
            yield return new WaitForSeconds(0.2f);
        }
        carpet.GetComponent<Animator>().SetTrigger("IsClose");

        // 파티클
        fireplace.SetActive(false);

        // 장작소리 서서히 끄기
        isVolumeUp = false;

        // 마무리
        yield return new WaitForSeconds(1f);

        // 애니메이션 초기화
        starLight.GetComponent<Animator>().enabled = false;
        windows[0].GetComponent<Animator>().enabled = false;
        windows[1].GetComponent<Animator>().enabled = false;
        windows[0].transform.localRotation = Quaternion.Euler(0, 0, 0);
        windows[1].transform.localRotation = Quaternion.Euler(0, 0, 0);
        walls[0].GetComponent<Animator>().enabled = false;
        walls[1].GetComponent<Animator>().enabled = false;
        walls[0].transform.localScale = new Vector3(0.2f, 0, 1);
        walls[1].transform.localScale = new Vector3(0.2f, 0, 1); 
        for (int i = 0; i < gifts.Length; i++)
        {
            gifts[i].GetComponent<Animator>().enabled = false;
            gifts[i].transform.localScale = new Vector3(0, 0, 0);
        }
        carpet.GetComponent<Animator>().enabled = false;
        carpet.transform.localScale = new Vector3(0, 1, 1);
       
        // 자막 들어가기
        dialog.isTextClose = true;
        dialog.isTextOpen = false;

        // 챕터 배경음 0으로 감소하기
        audioCtrl.fireAuto = 0;
        audioCtrl.bgmAuto = 0.5f;

        // 넘어가기
        if (popup.isReset)
        {
            NextContent();
        }
        else
        {
            NextChapter();
        }
    }


    public void NextChapter()
    {
        if (!popup.isReset)
        {
            popup.isBookmark = false;
            popup.isRe2345 = false;

            // 4번 챕터 잠금 해제
            setting.ChaperUnlock(4);
            popup.GoNext(3);
            gameObject.SetActive(false);
        }
    }

    public void NextContent()
    {
        popup.GoContent(popup.beforeState);
        gameObject.SetActive(false);
    }


    private void Update()
    {
        // 마지막에 불가사리 일어나서 걷기
        if (isStarfishWalk)
        {
            starfish.transform.localRotation = Quaternion.Slerp(starfish.transform.localRotation, Quaternion.Euler(0, -270, 0), Time.deltaTime * 5);
            starfish.transform.localPosition = Vector3.Lerp(starfish.transform.localPosition,
                new Vector3(7, starfish.transform.localPosition.y, starfish.transform.localPosition.z), Time.deltaTime * 0.5f);

            if (starfish.transform.localPosition.x >= 6)
            {
                starfish.GetComponent<Rigidbody>().isKinematic = true;
                starfish.GetComponent<Animator>().SetBool("IsWalk", false);
            }
        }

        // 효과음 서서히 커지기
        if (isVolumeUp)
        {
            // 비지엠 0.3으로 감소하기
            audioCtrl.bgmAuto = Mathf.Lerp(audioCtrl.bgmAuto, 0.3f, Time.deltaTime * 0.5f);
            // 장작소리 0.5로 증가하기
            audioCtrl.fireAuto = Mathf.Lerp(audioCtrl.fireAuto, 0.4f, Time.deltaTime * 0.5f);
        }
        // 효과음 서서히 줄어들기
        else
        {
            // 비지엠 0.5f으로 증가하기
            audioCtrl.bgmAuto = Mathf.Lerp(audioCtrl.bgmAuto, 0.5f, Time.deltaTime * 0.5f);
            // 장작소리 0으로 감소하기
            audioCtrl.fireAuto = Mathf.Lerp(audioCtrl.fireAuto, 0, Time.deltaTime * 0.5f);
        }

        // 팝업종이 열리기
        if (isPaperOpen)
        {
            paper.transform.GetChild(paperNumber).gameObject.SetActive(true);
            paper.transform.GetChild(paperNumber).transform.localScale = Vector3.Lerp(paper.transform.GetChild(paperNumber).transform.localScale,
                new Vector3(1, 1, 1f), Time.deltaTime * 1.5f);
        }
        else
        {
            paper.transform.GetChild(paperNumber).transform.localScale = Vector3.Lerp(paper.transform.GetChild(paperNumber).transform.localScale,
                new Vector3(1, 1, 0), Time.deltaTime * 1.5f);
            if (paper.transform.GetChild(paperNumber).transform.localScale.z <= 0.1f)
            {
                paper.transform.GetChild(paperNumber).gameObject.SetActive(false);
            }
        }
    }
}
