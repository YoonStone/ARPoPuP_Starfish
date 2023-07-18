using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Yo_Chapter_5 : MonoBehaviour
{
    [Header("팝업매니저")]
    public GameObject popupMng;
    [Header("자막")]
    public GameObject textUI;
    [Header("팝업종이")]
    public GameObject paper;
    [Header("종이들")]
    public GameObject[] papers;

    [Header("불가사리")]
    public GameObject starfish;
    public Transform starfishPos;
    
    [Header("거북이")]
    public GameObject turtle;

    [Header("해초 애니메이션")]
    public GameObject[] seaweeds1, seaweeds2, seaweeds3;
    [Header("큰 바위")]
    public GameObject cliff;
    [Header("숨어있는 물고기")]
    public GameObject fish_hide;
    [Header("조개")]
    public GameObject clam;
    [Header("성게")]
    public GameObject urchin;
    [Header("가자미")]
    public GameObject flatfish;

    [Header("별")]
    public GameObject star;
    [Header("별전구")]
    public GameObject starLight;

    [Header("주변환경 (포그)")]
    public GameObject fog;
    [Header("주변환경 (물빛)")]
    public GameObject waterLight;
    [Header("주변환경 (바위 스팟라이트)")]
    public GameObject rockLight;
    [Header("주변환경 (실 달린 물고기)")]
    public GameObject fish;

    [Header("현재 팝업종이")]
    public int paperNumber = 0;
    public bool isPaperOpen;

    [HideInInspector]
    public bool isTextOpen, isTextClose, isStarfishEnd, isCliffOpen;
    public int paperCount = 4;

    private bool isFogOn, isVolumeUp, isTurtleDown, isStarfishDown;

    Yo_TextCtrl dialog;
    public Yo_PopupCtrl popup;
    Yo_AudioCtrl audioCtrl;
    Animator anim_S, anim_L, anim_Starfish;

    private void Start()
    {
        dialog = textUI.GetComponent<Yo_TextCtrl>();
        audioCtrl = popupMng.GetComponent<Yo_AudioCtrl>();
        popup = popupMng.GetComponent<Yo_PopupCtrl>();

        anim_S = star.GetComponent<Animator>();
        anim_L = starLight.GetComponent<Animator>();
        anim_Starfish = starfish.GetComponent<Animator>();
    }

    public void Start1()
    {
        isCliffOpen = false;
        isTurtleDown = false;
        isStarfishDown = false;
        paperCount = 4;

        // 불가사리 스크립트 초기화 + 안 보이게
        starfish.transform.GetChild(0).gameObject.SetActive(false);
        // 불가사리 위치 초기화
        starfish.GetComponent<Animator>().enabled = true;
        starfish.GetComponent<Rigidbody>().isKinematic = true;
        starfish.transform.localPosition = starfishPos.localPosition;
        starfish.transform.localRotation = starfishPos.localRotation;
        starfish.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        // 거북이 초기화
        turtle.transform.localPosition = new Vector3(-4.5f, 7, -3);
        turtle.SetActive(false);

        // 애니메이션 초기화
        star.transform.localPosition = new Vector3(-0.26f, 10, -3.88f);
        star.transform.GetChild(0).gameObject.SetActive(false);
        starLight.transform.localScale = new Vector3(0, 0, 0);
        starLight.transform.GetChild(0).gameObject.SetActive(false);

        // 팝업종이 및 상호작용 오브젝트 상태 초기화
        paperNumber = 0;
        isPaperOpen = false;
        paper.transform.GetChild(0).transform.localPosition = new Vector3(0.8f, -0.075f, 1);

        // 가자미+땅바닥
        papers[4].GetComponent<Yo_SandPaper>().RealStart();

        // 성게+돌
        papers[5].GetComponent<Yo_StonePaper>().RealStart();

        StartCoroutine("AlreadyStart");
    }

    public IEnumerator AlreadyStart()
    {
        // 첫번째 해초
        yield return new WaitForSeconds(0f);
        for (int i = 0; i < 2; i++)
        {
            seaweeds1[i].GetComponent<Animator>().SetTrigger("IsUp");
        }
        // 물고기+분홍해초
        papers[0].GetComponent<Yo_SeaweedPaper_Fish>().RealStart();
        papers[1].GetComponent<Yo_SeaweedPaper_Fish>().RealStart();

        // 물속소리 서서히 들리게
        isVolumeUp = true;

        // 두번째 해초
        yield return new WaitForSeconds(0.6f);
        for (int i = 0; i < 2; i++)
        {
            seaweeds2[i].GetComponent<Animator>().SetTrigger("IsUp");
        }
        // 조개+초록해초
        papers[2].GetComponent<Yo_SeaweedPaper_Clam>().RealStart();
        papers[3].GetComponent<Yo_SeaweedPaper_Clam>().RealStart();
        isCliffOpen = true;

        // 세번째 해초
        yield return new WaitForSeconds(0.6f);
        for (int i = 0; i < 2; i++)
        {
            seaweeds3[i].GetComponent<Animator>().SetTrigger("IsUp");
        }

        // 오프닝
        yield return new WaitForSeconds(0.6f);
        StartCoroutine("RealStart");
    }

    // 오프닝
    public IEnumerator RealStart()
    {
        // 물고기 내려오기
        yield return new WaitForSeconds(0f);
        fish.GetComponent<Animator>().enabled = true;

        // 포그 서서히 보이게
        isFogOn = true;

        // 거북이 내려가기
        yield return new WaitForSeconds(1f);
        starfish.transform.GetChild(0).gameObject.SetActive(true);
        turtle.SetActive(true);
        isTurtleDown = true;
        isStarfishDown = true;

        // 불가사리 점프
        yield return new WaitForSeconds(2f);
        isStarfishDown = false;
        starfish.GetComponent<Rigidbody>().isKinematic = false;
        starfish.GetComponent<Rigidbody>().AddForce(Vector3.right * 220 + Vector3.up * 300);

        // 글자 보이게
        yield return new WaitForSeconds(1f);
        //StartCoroutine("Ending");
        ShowDialog(38);
        // 부딪히는 효과음 (탁)
        audioCtrl.TouchHeavy();
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
            // 오브젝트들 드래그 가능하게
            case 40:
                PaperDrag(true);
                break;

            // 오브젝트들 드래그 가능하게
            case 42: 
            case 44:
            case 46: 
            case 48: 
                if(paperCount > 0)
                {
                    PaperDrag(true);
                }
                // 종이들이 다 활성화 되었다면 자막나오기
                else
                {
                    PaperDrag(true);
                    Invoke("LastDialog", 2);                    
                }
                break;

            // 전구처럼
            case 50: anim_L.enabled = true; break;

            // 팝업종이 나오기
            case 51: Invoke("PaperOpen", 1); break;
            // 엔딩
            case 53: StartCoroutine("Ending"); break;
        }
    }

    // 모든 종이들 드래그할 수 있는지 없는지 조절하기
    public void PaperDrag(bool isDrag)
    {
        for (int i = 0; i < papers.Length; i++)
        {
            papers[i].SendMessage("Drag", isDrag);
        }
    }

    // 불가사리 팝업종이 나오기
    public void PaperOpen()
    {
        anim_Starfish.enabled = false;
        isPaperOpen = true;
    }

    public void LastDialog()
    {
        // 자막 나오기
        ShowDialog(49);

        // 별처럼
        anim_S.enabled = true;
    }

    public IEnumerator StarfishJump()
    {
        // 팝업종이 들어가고
        yield return new WaitForSeconds(0);
        isPaperOpen = false;

        // 불가사리 폴짝폴짝 반복
        anim_Starfish.enabled = true;
        anim_Starfish.SetTrigger("IsJump");

        // 자막 나오기
        yield return new WaitForSeconds(1);
        ShowDialog(52);
    }

    // 엔딩
    public IEnumerator Ending()
    {
        if (!popup.isReset)
        {
            yield return new WaitForSeconds(0);
            popup.isBookmark = false;
           
            yield return new WaitForSeconds(1);
            StartCoroutine("SeaweedsDown");
        }
    }

    // 초기화
    public void ResetChapter()
    {
        popup.isReset = true;
        
        // 해초 내려가기
        StartCoroutine("SeaweedsDown");
    }

    public IEnumerator SeaweedsDown()
    {
        // 불가사리 안 보이게
        yield return new WaitForSeconds(1f);
        starfish.GetComponent<Animator>().enabled = true;
        starfish.transform.GetChild(0).gameObject.SetActive(false);
        anim_Starfish.enabled = false;

        // 팝업종이
        isPaperOpen = false;

        // 물고기,별 올라가기 + 트리 들어가기 + 불가사리 작아지기
        fish.GetComponent<Animator>().SetTrigger("IsRealEnd");
        anim_S.SetTrigger("IsEnd");
        anim_L.SetTrigger("IsEnd");
        anim_Starfish.SetTrigger("IsEnd");

        // 큰 바위 내려가기
        isCliffOpen = false;

        // 첫번째 해초
        for (int i = 0; i < 2; i++)
        {
            seaweeds1[i].GetComponent<Animator>().SetTrigger("IsEnd");
        }

        // 물빛
        isFogOn = false;
        // 배경 사운드
        isVolumeUp = false;

        // 두번째 해초
        yield return new WaitForSeconds(0.8f);
        for (int i = 0; i < 2; i++)
        {
            seaweeds2[i].GetComponent<Animator>().SetTrigger("IsEnd");
        }

        // 세번째 해초
        yield return new WaitForSeconds(0.8f);
        for (int i = 0; i < 2; i++)
        {
            seaweeds3[i].GetComponent<Animator>().SetTrigger("IsEnd");
        }

        // 마무리
        yield return new WaitForSeconds(1f);
        fish.GetComponent<Animator>().enabled = false;
        anim_S.enabled = false;
        anim_L.enabled = false;
        anim_Starfish.enabled = false;

        cliff.transform.localRotation = Quaternion.Euler(0, 0, 90);
        cliff.transform.localScale = new Vector3(0, 0, 1);

        // 물고기+분홍해초
        papers[0].transform.localRotation = Quaternion.Euler(0, 0, 0);
        papers[1].transform.localRotation = Quaternion.Euler(0, 0, 0);
        fish_hide.transform.localPosition = new Vector3(-2, 2.3f, 4);

        // 조개+초록해초
        papers[2].transform.localPosition = new Vector3(1.47f, 1.8f, -4.79f);
        papers[3].transform.localPosition = new Vector3(2.75f, 1.8f, -4.7f);
        clam.transform.localRotation = Quaternion.Euler(10, 0, 0);

        // 가자미+땅바닥
        papers[4].transform.localRotation = Quaternion.Euler(-15, 0, 0);
        flatfish.transform.localPosition = new Vector3(4.05f, -0.2f, -3.56f);

        // 성게+돌
        papers[5].transform.localRotation = Quaternion.Euler(-35, 0, 0);
        urchin.transform.localScale = new Vector3(0, 0, 0);

        // 거북이 초기화
        isTurtleDown = false;
        isStarfishDown = false;
        turtle.SetActive(false);
        turtle.transform.localPosition = new Vector3(-4.5f, 7, -3);

        // 자막 들어가기
        dialog.isTextClose = true;
        dialog.isTextOpen = false;

        // 챕터 배경음 0으로 감소하기
        audioCtrl.waterAuto = 0;
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

            popup.GoNext(5);
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
        // 큰 바위 펼쳐지기
        if (isCliffOpen)
        {
            cliff.transform.localRotation = Quaternion.Slerp(cliff.transform.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 1.5f);
            cliff.transform.localScale = Vector3.Lerp(cliff.transform.localScale, new Vector3(1, 1, 1), Time.deltaTime * 0.8f);
        }
        else
        {
            cliff.transform.localRotation = Quaternion.Slerp(cliff.transform.localRotation, Quaternion.Euler(0, 0, 90), Time.deltaTime * 1.5f);
            cliff.transform.localScale = Vector3.Lerp(cliff.transform.localScale, new Vector3(0, 0, 1), Time.deltaTime * 1.5f);
        }

        // 거북이 내려가기 (7 -> 0.3)
        if (isTurtleDown)
        {
            turtle.transform.localPosition = Vector3.Lerp(turtle.transform.localPosition, new Vector3(-4.5f, 0.3f, -3f), Time.deltaTime * 0.6f);
        }
        // 불가사리 내려가기 (7.3 -> 0.6)
        if (isStarfishDown)
        {
            starfish.transform.localPosition = Vector3.Lerp(starfish.transform.localPosition, new Vector3(-4.5f, 0.6f, -3f), Time.deltaTime * 0.6f);
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

        // 안개 보이기
        if (isFogOn)
        {
            fog.GetComponent<Image>().color = Color.Lerp(fog.GetComponent<Image>().color, new Vector4(0.35f, 0.53f, 0.6f, 0.65f), Time.deltaTime * 1);

            for (int i = 0; i < waterLight.transform.childCount; i++)
            {
                if (i <= 8)
                {
                    // fillamount 사용
                    waterLight.transform.GetChild(i).GetComponent<Image>().fillAmount = Mathf.Lerp(waterLight.transform.GetChild(i).GetComponent<Image>().fillAmount, 1, Time.deltaTime * 1);
                    waterLight.transform.GetChild(i).GetComponent<Image>().color = Color.Lerp(waterLight.transform.GetChild(i).GetComponent<Image>().color, new Vector4(1, 1, 1, 1), Time.deltaTime * 1);
                }
                else if (i < 12)
                {
                    waterLight.transform.GetChild(i).GetComponent<Image>().color = Color.Lerp(waterLight.transform.GetChild(i).GetComponent<Image>().color, new Vector4(1, 1, 1, 1), Time.deltaTime * 1);
                }
                else
                {
                    waterLight.transform.GetChild(i).GetComponent<Image>().color = Color.Lerp(waterLight.transform.GetChild(i).GetComponent<Image>().color, new Vector4(1, 1, 1, 0.35f), Time.deltaTime * 1);
                }
            }

            //// 바위라이트 서서히 보이게
            //rockLight.GetComponent<Light>().intensity = Mathf.Lerp(rockLight.GetComponent<Light>().intensity, 2, Time.deltaTime * 1);
        }
        else
        {
            fog.GetComponent<Image>().color = Color.Lerp(fog.GetComponent<Image>().color, new Vector4(0.35f, 0.53f, 0.6f, 0), Time.deltaTime * 1);

            for (int i = 0; i < waterLight.transform.childCount; i++)
            {
                if (i <= 8)
                {
                    // fillamount 사용
                    waterLight.transform.GetChild(i).GetComponent<Image>().fillAmount = Mathf.Lerp(waterLight.transform.GetChild(i).GetComponent<Image>().fillAmount, 0, Time.deltaTime * 1);
                    waterLight.transform.GetChild(i).GetComponent<Image>().color = Color.Lerp(waterLight.transform.GetChild(i).GetComponent<Image>().color, new Vector4(1, 1, 1, 0), Time.deltaTime * 1);
                }
                else if (i < 12)
                {
                    waterLight.transform.GetChild(i).GetComponent<Image>().color = Color.Lerp(waterLight.transform.GetChild(i).GetComponent<Image>().color, new Vector4(1, 1, 1, 0), Time.deltaTime * 1);
                }
                else
                {
                    waterLight.transform.GetChild(i).GetComponent<Image>().color = Color.Lerp(waterLight.transform.GetChild(i).GetComponent<Image>().color, new Vector4(1, 1, 1, 0), Time.deltaTime * 1);
                }
            }

            // 바위라이트 서서히 안보이게
            //rockLight.GetComponent<Light>().intensity = Mathf.Lerp(rockLight.GetComponent<Light>().intensity, 0, Time.deltaTime * 1);
        }

        // 효과음 서서히 커지기
        if (isVolumeUp)
        {
            // 비지엠 0.3으로 감소하기
            audioCtrl.bgmAuto = Mathf.Lerp(audioCtrl.bgmAuto, 0.3f, Time.deltaTime * 0.5f);
            // 물속소리 0.5로 증가하기
            audioCtrl.waterAuto = Mathf.Lerp(audioCtrl.waterAuto, 0.5f, Time.deltaTime * 0.5f);
        }
        // 효과음 서서히 줄어들기
        else
        {
            // 비지엠 0.5f으로 증가하기
            audioCtrl.bgmAuto = Mathf.Lerp(audioCtrl.bgmAuto, 0.5f, Time.deltaTime * 0.5f);
            // 물속소리 0으로 감소하기
            audioCtrl.waterAuto = Mathf.Lerp(audioCtrl.waterAuto, 0, Time.deltaTime * 0.5f);
        }

        if (popup.isReset)
        {
            // 물고기+분홍해초
            papers[0].transform.localRotation = Quaternion.Slerp(papers[0].transform.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 3f);
            papers[1].transform.localRotation = Quaternion.Slerp(papers[1].transform.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 3f);
            fish_hide.transform.localPosition = Vector3.Lerp(fish_hide.transform.localPosition, new Vector3(-2, 2.3f, 4), Time.deltaTime * 3f);

            // 조개+초록해초
            papers[2].transform.localPosition = Vector3.Lerp(papers[2].transform.localPosition, new Vector3(1.47f, 1.8f, -4.79f), Time.deltaTime * 3f);
            papers[3].transform.localPosition = Vector3.Lerp(papers[3].transform.localPosition, new Vector3(2.75f, 1.8f, -4.7f), Time.deltaTime * 3f);
            clam.transform.localRotation = Quaternion.Slerp(clam.transform.localRotation, Quaternion.Euler(10, 0, 0), Time.deltaTime * 3f);

            // 가자미+땅바닥
            papers[4].transform.localRotation = Quaternion.Slerp(papers[4].transform.localRotation, Quaternion.Euler(-15, 0, 0), Time.deltaTime * 3f);
            flatfish.transform.localPosition = Vector3.Lerp(flatfish.transform.localPosition, new Vector3(4.05f, -0.2f, -3.56f), Time.deltaTime * 3f);

            // 성게+돌
            papers[5].transform.localRotation = Quaternion.Slerp(papers[5].transform.localRotation, Quaternion.Euler(-35, 0, 0), Time.deltaTime * 3f);
            urchin.transform.localScale = Vector3.Lerp(urchin.transform.localScale, new Vector3(0, 0, 0), Time.deltaTime * 3f);
        }
    }
}
