using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Yo_Chapter_4 : MonoBehaviour
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
    
    [Header("거북이")]
    public GameObject turtle;

    [Header("애니메이션 오브젝트 (바다)")]
    public GameObject sea;
    [Header("애니메이션 오브젝트 (구름)")]
    public GameObject cloud;
    [Header("애니메이션 오브젝트 (등대)")]
    public GameObject lightHouse;

    [Header("애니메이션 오브젝트 (팻말)")]
    public GameObject panel;

    [Header("쓰레기 미션")]
    public GameObject trashMission;
    [Header("쓰레기통")]
    public GameObject[] trashCan;    
    [Header("쓰레기통 전 쓰레기")]
    public GameObject[] trashCantrash;
    [Header("쓰레기들")]
    public GameObject[] trashes;
    [Header("쓰레기 없어지는 파티클")]
    public ParticleSystem trash_particle;

    [Header("불가사리 베지어커브 (거북이한테 가는)")]
    public GameObject[] bgCurve_Starfish4;

    [Header("바다 기울기 (개발자용)")]
    public bool isSeaMove;

    [Header("현재 팝업종이")]
    public int paperNumber = 0;
    public bool isPaperOpen;

    // 불가사리 위치
    public int nowStarfishState = 0;
    public int trashChangeCount = 0;

    private bool isSeaOpen, isLightHouse, isTurtleUp, isTurtleDown, isSeaColor, isVolumeUp;

    Yo_TextCtrl dialog;
    public Yo_PopupCtrl popup;
    Yo_SettingCtrl setting;
    public Yo_AudioCtrl audioCtrl;
    Yo_TrashMission trashMissionS;

    MeshRenderer sea_material;

    private void Start()
    {
        dialog = textUI.GetComponent<Yo_TextCtrl>();
        popup = popupMng.GetComponent<Yo_PopupCtrl>();
        setting = popupMng.GetComponent<Yo_SettingCtrl>();
        audioCtrl = popupMng.GetComponent<Yo_AudioCtrl>();
        
        trashMissionS = trashMission.GetComponent<Yo_TrashMission>();
        sea_material = sea.transform.GetChild(0).GetComponent<MeshRenderer>();

        //// 텍스트 머테리얼 바꾸기
        //dialog.MaterialChange();
    }

    public void Start1()
    {
        // 불가사리 초기화 + 안 보이게
        //starfish.GetComponent<Yo_Starfish_2>().RealStart();
        //starfish.GetComponent<Yo_Starfish_2>().isStarfishTurn = false;
        starfish.GetComponent<Rigidbody>().isKinematic = true;
        starfish.transform.GetChild(0).gameObject.SetActive(false);
        isTurtleUp = false;
        isTurtleDown = false;
        starfish.transform.localPosition = starfishPos.localPosition;
        starfish.transform.localRotation = starfishPos.localRotation;
        turtle.transform.localPosition = new Vector3(-4.12f, -0.2f, -3.72f);
        turtle.SetActive(false);
        nowStarfishState = 0;
        trashChangeCount = 0;

        // 미션 중지
        trashMission.SetActive(false);

        // 배치되어있는 쓰레기 있기
        for (int i = 0; i < trashes.Length; i++)
        {
            trashes[i].SetActive(true);
        }

        // 팝업종이 및 상호작용 오브젝트 상태 초기화
        paperNumber = 0;
        isPaperOpen = false;
        paper.transform.GetChild(0).localPosition = new Vector3(-7.2f, 0, 0);
        paper.transform.GetChild(0).GetComponent<Yo_StarfishPaper_4>().isStarfishTurn = false;
        paper.transform.GetChild(0).GetComponent<Yo_StarfishPaper_4>().isEnd = false;

        for (int i = 0; i < trashCan.Length; i++)
        {
            trashCan[i].transform.GetChild(0).GetChild(0).gameObject.GetComponent<Animator>().enabled = false;
            trashCan[i].transform.GetChild(0).GetChild(0).localRotation = Quaternion.Euler(-90, 0, 0);
            trashCantrash[i].SetActive(true);
            trashCan[i].GetComponent<Animator>().enabled = false;
            trashCan[i].transform.localScale = new Vector3(0, 0, 0);
            trashCan[i].transform.GetChild(0).gameObject.SetActive(false);
        }
        // 바다 색 돌아오기
        isSeaColor = false;
        sea.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Vector4(0.27f, 0.525f, 0.423f, 1);

        StartCoroutine("RealStart");
    }

    public IEnumerator RealStart()
    {
        // 바닷물 들어오기
        yield return new WaitForSeconds(0);
        starfish.transform.GetChild(0).gameObject.SetActive(true);
        sea.SetActive(true);
        isSeaOpen = true;

        // 파도소리 서서히 들리게
        isVolumeUp = true;

        // 구름 내려오기
        yield return new WaitForSeconds(3);
        cloud.SetActive(true);

        // 등대 불 켜지기
        lightHouse.SetActive(true);
        isLightHouse = true;

        // 바다가 움직일 수 있도록
        isSeaMove = true;

        // 자막 나오기
        ShowDialog(29);
        print("자막 나오기 29");
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
        if (!popup.isReset)
        {
            switch (textNumber)
            {
                // 불가사리 팝업종이 나오기
                case 31: Invoke("PaperOpen", 1); break;

                // 쓰레기 버리기 미션 시작
                case 34:
                    trashMission.SetActive(true);
                    trashMission.GetComponent<Yo_TrashMission>().RealStart();
                    // 쓰레기통 종이 나오기
                    paperNumber = 1;
                    isPaperOpen = true;
                    for (int i = 0; i < trashCan.Length; i++)
                    {
                        trashCan[i].transform.GetChild(0).GetChild(0).gameObject.GetComponent<Animator>().enabled = true;
                    }
                    break;

                // 불가사리 거북이한테 가기 (엔딩)
                case 37:
                    StartCoroutine("Ending"); break;
            }
        }
    }

    // 불가사리 팝업종이 나오기
    public void PaperOpen()
    {
        if (!popup.isReset)
        {
            paperNumber = 0;
            isPaperOpen = true;
        }
    }

    // 불가사리 팝업종이 들어가고 자막 / 미션 끝
    public IEnumerator PaperToDialog(int paper)
    {
        // 팝업종이 들어가기
        yield return new WaitForSeconds(0);
        paperNumber = paper;
        isPaperOpen = false;

        // 자막 나오기
        yield return new WaitForSeconds(1);
        ShowDialog(32);
    }

    // 깨끗해지게
    public IEnumerator CleanSea()
    {
        // 깨끗해지는 파티클
        yield return new WaitForSeconds(1);
        trash_particle.Play();

        // 깨끗해지는 효과음
        audioCtrl.Chapter4Clean();

        // 배치되어있는 쓰레기 없어지기
        for (int i = 0; i < trashes.Length; i++)
        {
            trashes[i].SetActive(false);
        }

        // 던진 쓰레기 없어지기
        for (int i = 3; i < trashMission.transform.childCount; i++)
        {
            Destroy(trashMission.transform.GetChild(i).gameObject);
        }

        // 바다 서서히 색 바뀌기
        isSeaColor = true;

        yield return new WaitForSeconds(2);
        ShowDialog(35);
    }

    // 엔딩
    public IEnumerator Ending()
    {
        if (!popup.isReset)
        {
            // 거북이 올라오기
            yield return new WaitForSeconds(0);
            popup.isBookmark = false;
            turtle.SetActive(true);
            isTurtleUp = true;

            // 불가사리 이동
            yield return new WaitForSeconds(1);
            bgCurve_Starfish4[nowStarfishState - 1].SetActive(true);
            starfish.GetComponent<Animator>().SetBool("IsWalk", true);

            // 쓰레기통 닫기
            yield return new WaitForSeconds(0.1f);
            trashCan[nowStarfishState - 1].transform.GetChild(0).GetChild(0).gameObject.GetComponent<Animator>().SetBool("IsOpen", false);

            // 불가사리 이동 끝
            switch (nowStarfishState)
            {
                case 1:
                    yield return new WaitForSeconds(4.5f);
                    starfish.GetComponent<Animator>().SetBool("IsWalk", false);
                    break;
                case 2:
                    yield return new WaitForSeconds(3.8f);
                    starfish.GetComponent<Animator>().SetBool("IsWalk", false);
                    break;
                case 3:
                    yield return new WaitForSeconds(3.5f);
                    starfish.GetComponent<Animator>().SetBool("IsWalk", false);
                    break;
            }

            // 불가사리 이동 끝나면 거북이랑 같이 내려가기
            yield return new WaitForSeconds(1);
            isTurtleUp = false;
            isTurtleDown = true;

            // 다음 페이지
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

        // 바다 들어가기
        isSeaMove = false;
        isSeaOpen = false;

        // 팝업종이
        isPaperOpen = false;

        // 배경 사운드
        isVolumeUp = false;

        // 구름
        cloud.GetComponent<Animator>().SetTrigger("IsEnding");
        // 미션 중지
        trashMission.SetActive(false);
        // 팻말 들어가기
        panel.GetComponent<Animator>().SetTrigger("IsClose");

        // 불가사리 초기화
        starfish.transform.localPosition = starfishPos.localPosition;
        starfish.GetComponent<Rigidbody>().isKinematic = true;
        starfish.transform.localRotation = starfishPos.localRotation;

        // 쓰레기통 없어지기
        for (int i = 0; i < trashCan.Length; i++)
        {
            trashCan[i].GetComponent<Animator>().SetTrigger("IsClose");
            yield return new WaitForSeconds(0.5f);
            trashCantrash[i].SetActive(true);
        }

        // 거북이 초기화
        isTurtleUp = false;
        isTurtleDown = false;
        turtle.transform.localPosition = new Vector3(-4.12f, -0.2f, -3.72f);
        turtle.SetActive(false);

        // 등대 불 꺼지기
        yield return new WaitForSeconds(0.5f);
        isLightHouse = false;
        lightHouse.SetActive(false);

        // 마무리
        yield return new WaitForSeconds(0.5f);

        // 애니메이션 초기화
        panel.GetComponent<Animator>().SetTrigger("IsClose");
        panel.transform.localRotation = Quaternion.Euler(106, -25, 0);
        trashMissionS.lights.SetActive(false);
        for (int i = 0; i < trashCan.Length; i++)
        {
            trashCan[i].GetComponent<Animator>().enabled = false;
            trashCan[i].transform.localScale = new Vector3(0, 0, 0);
            trashCan[i].transform.GetChild(0).gameObject.SetActive(false);
            trashCan[i].transform.GetChild(0).GetChild(0).gameObject.GetComponent<Animator>().enabled = false;
            trashCan[i].transform.GetChild(0).GetChild(0).localRotation = Quaternion.Euler(-90, 0, 0);
        }
        // 던진 쓰레기 없기
        for (int i = 3; i < trashMission.transform.childCount; i++)
        {
            Destroy(trashMission.transform.GetChild(i).gameObject);
        }
        // 배치되어있는 쓰레기 있기
        for (int i = 0; i < trashes.Length; i++)
        {
            trashes[i].SetActive(true);
        }
        // 바다 색 돌아오기
        sea_material.material.color = new Vector4(0.27f, 0.525f, 0.423f, 1);

        // 자막 들어가기
        dialog.isTextClose = true;
        dialog.isTextOpen = false;

        // 챕터 배경음 0으로 감소하기
        audioCtrl.waveAuto = 0;
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

            // 5번 챕터 잠금 해제
            setting.ChaperUnlock(5);
            popup.GoNext(4);
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

        // 핸드폰 기울기에 따라 바닷물 움직이기
        if (isSeaMove)
        {
            // 핸드폰을 오른쪽으로 기울이면
            if (Input.acceleration.x > 0.2f)
            {
                // 바다 늘어나기
                sea.transform.localScale -= new Vector3(Time.deltaTime * 0.3f, 0, 0);
            }
            // 핸드폰을 왼쪽으로 기울이면
            else if (Input.acceleration.x < -0.2f)
            {
                // 바다 줄어들기
                sea.transform.localScale += new Vector3(Time.deltaTime * 0.3f, 0, 0);
            }

            // 바다의 x스케일 한계두기 (0.4 ~ 1)
            sea.transform.localScale
                = new Vector3(Mathf.Clamp(sea.transform.localScale.x, 0.4f, 1), 1, 1);
        }

        // 등대 돌아가게 하기
        if (isLightHouse)
        {
            lightHouse.transform.Rotate(0, - Time.deltaTime * 6, 0);
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

        // 거북이 올라오기 (-0.2 -> 0.28)
        if (isTurtleUp)
        {
            turtle.transform.localPosition = Vector3.Lerp(turtle.transform.localPosition, new Vector3(turtle.transform.localPosition.x, 0.28f, turtle.transform.localPosition.z), Time.deltaTime * 1);
        }
        // 거북이 내려가기 (0.28 -> -0.2)
        if (isTurtleDown)
        {
            turtle.transform.localPosition = Vector3.Lerp(turtle.transform.localPosition, new Vector3(turtle.transform.localPosition.x, -1.3f, turtle.transform.localPosition.z), Time.deltaTime * 1);
            starfish.transform.localPosition = Vector3.Lerp(starfish.transform.localPosition, new Vector3(starfish.transform.localPosition.x, -0.93f, starfish.transform.localPosition.z), Time.deltaTime * 1);
        }

        // 바다 색 서서히 변하게
        if (isSeaColor)
        {
            sea_material.material.color = Color.Lerp(sea_material.material.color, new Vector4(0.172f, 0.725f, 0.9f, 1), Time.deltaTime * 1);
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
