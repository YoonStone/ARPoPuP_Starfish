using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Yo_Chapter_2 : MonoBehaviour
{
    [Header("팝업매니저")]
    public GameObject popupMng;
    [Header("자막")]
    public GameObject textUI;
    [Header("팝업종이")]
    public GameObject paper;
    [Header("왼쪽 면")]
    public Transform map_L;

    [Header("불가사리")]
    public GameObject starfish;
    public Transform starfishPos;

    [Header("거북이")]
    public GameObject turtle;

    [Header("애니메이션 오브젝트 (바다)")]
    public GameObject sea;
    [Header("애니메이션 오브젝트 (구름)")]
    public GameObject cloud;
    [Header("애니메이션 오브젝트 (등대)")]
    public GameObject lightHouse;
    [Header("애니메이션 오브젝트 (조개들)")]
    public GameObject[] joges;
    [Header("애니메이션 오브젝트 (모래성)")]
    public GameObject sand;
    [Header("애니메이션 오브젝트 (사다리)")]
    public GameObject ladder;
    [Header("애니메이션 오브젝트 (파라솔)")]
    public GameObject umb;
    [Header("애니메이션 오브젝트 (돗자리)")]
    public GameObject blan;
    [Header("애니메이션 오브젝트 (게)")]
    public GameObject[] crab;

    [Header("주변환경 (실 달린 별)")]
    public GameObject star;

    [Header("주변환경 (밧줄)")]
    private GameObject rope;
    
    [Header("바다 기울기 (개발자용)")]
    public bool isSeaMove;

    [Header("현재 팝업종이")]
    public int paperNumber = 0;
    public bool isPaperOpen;
    public bool isLadderPaper;

    private bool isSeaOpen, isTurtleUp, isLightHouse, isLadderDown, isVolumeUp;

    public Animator ladderAnim2;

    Yo_TextCtrl dialog;
    Yo_SettingCtrl setting;
    Yo_AudioCtrl audioCtrl;
    Yo_PopupCtrl popup;
    Animator anim_L, anim_B, anim_U, anim_S;

    private void Start()
    {
        dialog = textUI.GetComponent<Yo_TextCtrl>();
        popup = popupMng.GetComponent<Yo_PopupCtrl>();
        setting = popupMng.GetComponent<Yo_SettingCtrl>();
        audioCtrl = popupMng.GetComponent<Yo_AudioCtrl>();

        anim_L = ladder.GetComponent<Animator>();
        anim_B = blan.GetComponent<Animator>();
        anim_U = umb.GetComponent<Animator>();
        anim_S = sand.GetComponent<Animator>();

        //// 텍스트 머테리얼 바꾸기
        //dialog.MaterialChange();
    }

    public void Start1()
    {
        isLadderPaper = false;
        isLadderDown = false;
        
        rope = GameObject.FindWithTag("Rope");

        // 불가사리 초기화 + 안 보이게
        starfish.GetComponent<Yo_Starfish_2>().RealStart();
        starfish.GetComponent<Yo_Starfish_2>().isStarfishTurn = false;
        starfish.GetComponent<Rigidbody>().isKinematic = true;
        starfish.transform.GetChild(0).gameObject.SetActive(false);
        isTurtleUp = false;
        starfish.transform.localPosition = starfishPos.localPosition;
        starfish.transform.localRotation = starfishPos.localRotation;

        // 별 초기화
        star.GetComponent<Animator>().enabled = false;
        star.transform.GetChild(0).localPosition = new Vector3(0, 5, 0);
        star.transform.GetChild(0).gameObject.SetActive(false);
        star.SetActive(true);
        star.GetComponent<Yo_StarMissionCtrl>().StarReset();

        // 밧줄 초기화
        rope.transform.localPosition = new Vector3(0.08f, 0.15f, 0.16f);
        rope.GetComponent<Yo_RopeCtrl>().isUp = false;
        rope.GetComponent<Yo_RopeCtrl>().isCanUp = false;

        // 팝업종이 및 상호작용 오브젝트 상태 초기화
        paperNumber = 0;
        isPaperOpen = false;
        paper.transform.GetChild(0).localPosition = new Vector3(0.65f, 0, 1);
        paper.transform.GetChild(0).GetComponent<Yo_LadderPaper>().isLadderEnd = false;
        paper.transform.GetChild(1).localPosition = new Vector3(0, -0.1f, 0.1f);

        StartCoroutine("RealStart");
    }

    public IEnumerator RealStart()
    {
        // 바닷물 들어오기
        yield return new WaitForSeconds(0);
        sea.SetActive(true);
        isSeaOpen = true;

        // 파도소리 서서히 들리게
        isVolumeUp = true;

        // 사다리 펼쳐지기
        yield return new WaitForSeconds(1);
        anim_L.enabled = true;

        // -- 개발자 전용--
        //// 별 내려오고
        //GameObject.FindWithTag("Star").GetComponent<Animator>().enabled = true;

        //// 불가사리 움직임 시작
        //StartCoroutine("StarfishStart");

        // 구름 내려오기
        cloud.SetActive(true);

        // 모래성 올라오기
        anim_S.enabled = true;

        // 돗자리, 우산 펼쳐지기
        yield return new WaitForSeconds(0.8f);
        anim_U.enabled = true;
        anim_B.enabled = true;

        // 게 올라오기
        yield return new WaitForSeconds(0.8f);
        crab[0].GetComponent<Animator>().enabled = true;
        crab[1].GetComponent<Animator>().enabled = true;

        // 등대 불 켜지기
        lightHouse.SetActive(true);
        isLightHouse = true;

        // 거북이, 불가사리 올라오기
        starfish.transform.GetChild(0).gameObject.SetActive(true);
        turtle.transform.GetChild(0).gameObject.SetActive(true);
        isTurtleUp = true;

        // 조개 애니메이션
        for (int i = 0; i < joges.Length; i++)
        {
            joges[i].GetComponent<Animator>().enabled = true;
            yield return new WaitForSeconds(0.3f);
        }

        // 바다가 움직일 수 있도록
        isSeaMove = true;
        //StartCoroutine("Ending");

        // 동앗줄 내려오기
        rope.GetComponent<Animator>().enabled = true;
        rope.GetComponent<Animator>().SetTrigger("IsDown");

        // 동앗줄 내려오는 애니메이션 그만
        yield return new WaitForSeconds(2f);
        rope.GetComponent<Animator>().enabled = false;

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
            // 사다리 종이 나오기
            case 13:
                anim_L.enabled = false;
                Invoke("PaperOpen", 1);
                break;

            // 피아노 종이 나오기
            case 16:
                // 별미션 시작
                star.GetComponent<Yo_StarMissionCtrl>().StarMission(); break;

            // 진짜 엔딩
            case 19: StartCoroutine("Ending");
                break;
        }
    }

    // 사다리 종이 나오기
    public void PaperOpen()
    {
        paperNumber = 0;
        isPaperOpen = true;
    }

    // 불가사리 이동 + 자막
    public IEnumerator StarfishStart()
    {
        // 효과음
        yield return new WaitForSeconds(0.7f);
        audioCtrl.StarEffect();
        yield return new WaitForSeconds(0.9f);
        audioCtrl.StarEffect();

        star.GetComponent<Yo_StarMissionCtrl>().RealStart();

        // 불가사리가 사다리 앞까지 이동
        starfish.GetComponent<Yo_Starfish_2>().Move();

        // 글자 보이게
        yield return new WaitForSeconds(4.5f);
        ShowDialog(12);
    }

    // 엔딩
    public IEnumerator Ending()
    {
        if (!popup.isReset)
        {
            yield return new WaitForSeconds(0);
            popup.isBookmark = false;
            
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
        //  불가사리 안 보이게
        yield return new WaitForSeconds(0f);
        starfish.transform.GetChild(0).gameObject.SetActive(false);

        // 바다 들어가기
        isSeaMove = false;
        isSeaOpen = false;

        // 팝업종이
        isPaperOpen = false;

        // 배경 사운드
        isVolumeUp = false;

        // 사다리 닫히기
        if (isLadderPaper)
        {
            // 사다리가 이미 다 펴졌다면
            isLadderDown = true;
        }
        else
        {
            // 사다리가 안 펴졌다면
            anim_L.SetTrigger("IsDown");
        }

        // 별,구름 올라가기
        star.GetComponent<Animator>().SetTrigger("IsEnding");
        cloud.GetComponent<Animator>().SetTrigger("IsEnding");

        // 모래성 내려가기
        yield return new WaitForSeconds(0.5f);
        anim_S.SetTrigger("IsDown");

        // 거북이 초기화
        isTurtleUp = false;
        turtle.transform.localPosition = new Vector3(-4, -1.5f, -3.72f);
        turtle.transform.GetChild(0).gameObject.SetActive(false);

        // 돗자리, 우산 없어지기
        yield return new WaitForSeconds(0.5f);
        anim_B.SetTrigger("IsDown");
        anim_U.SetTrigger("IsDown");

        // 게 내려가기
        yield return new WaitForSeconds(0.5f);
        crab[0].GetComponent<Animator>().SetTrigger("IsDown");
        crab[1].GetComponent<Animator>().SetTrigger("IsDown");

        // 등대 불 꺼지기
        isLightHouse = false;
        lightHouse.SetActive(false);

        // 조개 애니메이션
        for (int i = 0; i < 4; i++)
        {
            joges[i].GetComponent<Animator>().SetTrigger("IsDown");
            yield return new WaitForSeconds(0.3f);
        }

        // 마무리
        yield return new WaitForSeconds(1f);
        star.SetActive(false);

        // 애니메이션 초기화
        anim_L.enabled = false;
        ladder.transform.localPosition = new Vector3(0.989f, -0.3f, -2.162f);
        ladder.transform.localRotation = Quaternion.Euler(0, 50, 63);
        ladder.transform.GetChild(0).localRotation = Quaternion.Euler(0, 0, 207);
        anim_B.enabled = false;
        blan.transform.localScale = new Vector3(0, 1, 1);
        blan.transform.GetChild(0).gameObject.SetActive(false);
        anim_U.enabled = false;
        umb.transform.localScale = new Vector3(0, 0, 1);
        anim_S.enabled = false;
        sand.transform.localScale = new Vector3(1, 0, 1);
        sand.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        sand.transform.GetChild(0).gameObject.SetActive(false);
        for (int i = 0; i < 4; i++)
        {
            joges[i].GetComponent<Animator>().enabled = false;
            joges[i].GetComponent<MeshRenderer>().enabled = false;
            joges[i].transform.localScale = new Vector3(0, 0, 0);
        }
        for (int i = 0; i < 2; i++)
        {
            crab[i].GetComponent<Animator>().enabled = false;
            crab[i].GetComponent<MeshRenderer>().enabled = false;
            crab[i].transform.localRotation = Quaternion.Euler(-9, 0, -360);
        }

        // 챕터 배경음 0으로 감소하기
        audioCtrl.waveAuto = 0;
        audioCtrl.bgmAuto = 0.5f;

        // 자막 들어가기
        dialog.isTextClose = true;
        dialog.isTextOpen = false;

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

            // 3번 챕터 잠금 해제
            setting.ChaperUnlock(3);
            popup.GoNext(2);
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
        // 처음에 바다 늘어나기
        if (isSeaOpen)
        {
            if (!popup.isReset)
            {
                sea.transform.localScale = Vector3.Lerp(sea.transform.localScale, new Vector3(1, 1, 1), Time.deltaTime * 0.4f);
            }
        }
        else
        {
            sea.transform.localScale = Vector3.Lerp(sea.transform.localScale, new Vector3(0, 1, 1), Time.deltaTime * 1.2f);
        }

        // 거북이, 불가사리 올라오기
        if (isTurtleUp)
        {
            turtle.transform.localPosition = Vector3.Lerp(turtle.transform.localPosition, new Vector3(-4, 0.3f, -3.72f), Time.deltaTime * 1.2f);
            starfish.transform.localPosition = Vector3.Lerp(starfish.transform.localPosition, new Vector3(-4, 0.6f, -3.78f), Time.deltaTime * 1.2f);

            if(turtle.transform.localPosition.y >= 0.29f)
            {
                isTurtleUp = false;
            }
        }

        // 핸드폰 기울기에 따라 바닷물 움직이기
        if (isSeaMove)
        {
            if (setting.isAcceleration)
            {
                // 핸드폰을 오른쪽으로 기울이면
                if (Input.acceleration.x > 0.2f)
                {
                    // 바다 늘어나기
                    sea.transform.localScale += new Vector3(Time.deltaTime * 0.3f, 0, 0);

                    // 거북이 오른쪽으로
                    turtle.transform.localPosition += new Vector3(Time.deltaTime * 1.5f, 0, 0);
                }
                // 핸드폰을 왼쪽으로 기울이면
                else if (Input.acceleration.x < -0.2f)
                {
                    // 바다 줄어들기
                    sea.transform.localScale -= new Vector3(Time.deltaTime * 0.3f, 0, 0);

                    // 거북이 왼쪽으로
                    turtle.transform.localPosition -= new Vector3(Time.deltaTime * 1.5f, 0, 0);
                }

                // 바다의 x스케일 한계두기 (0.4 ~ 1)
                sea.transform.localScale
                    = new Vector3(Mathf.Clamp(sea.transform.localScale.x, 0.4f, 1), 1, 1);

                // 거북이 x포지션 한계두기 (-7 ~ -3)
                turtle.transform.localPosition
                    = new Vector3(Mathf.Clamp(turtle.transform.localPosition.x, -6.7f, -4f),
                    turtle.transform.localPosition.y, turtle.transform.localPosition.z);
            }
        }

        // 등대 돌아가게 하기
        if (isLightHouse)
        {
            lightHouse.transform.Rotate(0, -Time.deltaTime * 6, 0);
        }

        // 효과음 서서히 커지기
        if (isVolumeUp)
        {
            // 비지엠 0.3으로 감소하기
            audioCtrl.bgmAuto = Mathf.Lerp(audioCtrl.bgmAuto, 0.3f, Time.deltaTime * 0.5f);
            // 파도소리 0.5로 증가하기
            audioCtrl.waveAuto = Mathf.Lerp(audioCtrl.waveAuto, 0.5f, Time.deltaTime * 0.5f);
        }
        // 효과음 서서히 줄어들기
        else
        {
            // 비지엠 0.5f으로 증가하기
            audioCtrl.bgmAuto = Mathf.Lerp(audioCtrl.bgmAuto, 0.5f, Time.deltaTime * 0.5f);
            // 파도소리 0으로 감소하기
            audioCtrl.waveAuto = Mathf.Lerp(audioCtrl.waveAuto, 0, Time.deltaTime * 0.5f);
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

        // 사다리 내려가기
        if (isLadderDown)
        {
            ladder.transform.localRotation = Quaternion.Slerp(ladder.transform.localRotation, Quaternion.Euler(0, 50, 90), Time.deltaTime * 1f);
        }

        if (popup.isReset)
        {
            // 밧줄 올라가기
            rope.transform.localPosition = Vector3.Lerp(rope.transform.localPosition, new Vector3(0.08f, 0.15f, 0.16f), Time.deltaTime * 1.2f);
        }
    }
}
