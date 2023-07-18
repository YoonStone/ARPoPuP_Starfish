using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Yo_Chapter_1 : MonoBehaviour
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

    [Header("해초 애니메이션")]
    public GameObject[] seaweeds1, seaweeds2, seaweeds3;

    [Header("주변환경 (포그)")]
    public GameObject fog;
    [Header("주변환경 (물빛)")]
    public GameObject waterLight;
    [Header("주변환경 (바위 스팟라이트)")]
    public GameObject rockLight;
    [Header("주변환경 (실 달린 물고기)")]
    public GameObject fish;

    [Header("절벽")]
    public GameObject cliff;
    [Header("가자미셋")]
    public GameObject[] flatfish;
    [Header("가마지 미션")]
    public GameObject flatfishMission;

    [Header("현재 팝업종이")]
    public int paperNumber = 0;
    public bool isPaperOpen;

    [HideInInspector]
    public bool isTextOpen, isTextClose, isStarfishEnd;

    private bool isFogOn, isVolumeUp, isChEnd;

    Yo_TextCtrl dialog;
    Yo_AudioCtrl audioCtrl;
    Yo_PopupCtrl popup;
    Yo_SettingCtrl setting;
    Yo_Starfish_1 starfish_script;

    private void Start()
    {
        dialog = textUI.GetComponent<Yo_TextCtrl>();
        audioCtrl = popupMng.GetComponent<Yo_AudioCtrl>();
        popup = popupMng.GetComponent<Yo_PopupCtrl>();
        setting = popupMng.GetComponent<Yo_SettingCtrl>();
        starfish_script = starfish.GetComponent<Yo_Starfish_1>();
    }

    public void Start1()
    {
        isStarfishEnd = false;
        isChEnd = false;
        // 불가사리 스크립트 초기화 + 안 보이게
        starfish.GetComponent<Yo_Starfish_1>().RealStart();
        starfish.transform.GetChild(0).gameObject.SetActive(false);
        // 불가사리 위치 초기화
        starfish.transform.localPosition = starfishPos.localPosition;
        starfish.transform.localRotation = starfishPos.localRotation;
        starfish.GetComponent<Yo_Starfish_1>().turtle.GetComponent<Animator>().enabled = false;
        flatfishMission.GetComponent<Yo_FlatfishMission>().count = 3;

        // 팝업종이 및 상호작용 오브젝트 상태 초기화
        paperNumber = 0;
        paper.transform.GetChild(0).transform.localPosition = new Vector3(-6, 0, 0);
        paper.transform.GetChild(0).GetComponent<Yo_FishesPaper>().inter_Obj.transform.localPosition = new Vector3(-10, 4.8f, -3);
        paper.transform.GetChild(1).transform.localPosition = new Vector3(-2, 0, 1);
        paper.transform.GetChild(1).GetComponent<Yo_ClamPaper>().inter_Obj.transform.localRotation = Quaternion.Euler(10, 0, 0);
        paper.transform.GetChild(2).GetChild(1).transform.localPosition = new Vector3(-5, 0, -0.9f);
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

        // 물속소리 서서히 들리게
        isVolumeUp = true;

        // 두번째 해초
        yield return new WaitForSeconds(0.6f);
        for (int i = 0; i < 2; i++)
        {
            seaweeds2[i].GetComponent<Animator>().SetTrigger("IsUp");
        }

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

        // 불가사리 일어나기
        yield return new WaitForSeconds(2f);
        starfish.transform.GetChild(0).gameObject.SetActive(true);
        starfish_script.isWakeUp = true;

        // 불가사리 일어나기 끝
        yield return new WaitForSeconds(2f);
        starfish_script.isWakeUp = false;

        // 자막 보이게
        ShowDialog(0);
        //StartCoroutine("Ending");
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
            // 물고기들 팝업종이 열리기
            case 1: 
                Invoke("FishesPaperOn", 2f);
                //StartCoroutine("Ending");
                break;

            // 엔딩
            case 11: isChEnd = true;  break;
        }
    }

    // 물고기들 팝업종이 열리기
    public void FishesPaperOn()
    {
        isPaperOpen = true;
    }

    // 엔딩
    public IEnumerator Ending()
    {
        if (!popup.isReset)
        {
            // 물고기 올라가기
            yield return new WaitForSeconds(2);
            popup.isBookmark = false;
            
            // 해초 내려가기
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
        yield return new WaitForSeconds(0f);
        starfish.transform.GetChild(0).gameObject.SetActive(false);

        // 팝업종이
        isPaperOpen = false;

        // 내려오는 물고기 + 지나가는 물고기
        fish.GetComponent<Animator>().SetTrigger("IsRealEnd");
        paper.transform.GetChild(0).GetComponent<Yo_FishesPaper>().inter_Obj.SetActive(false);


        // 가자미 베지어 커브
        Destroy(starfish.GetComponent<Yo_Starfish_1>().tempBG_F);

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
        yield return new WaitForSeconds(0.5f);

        // 가자미 미션 없애기
        flatfishMission.SetActive(false);
        for (int i = 0; i < flatfish.Length; i++)
        {
            flatfish[i].GetComponent<Renderer>().material.SetFloat("_HueShift", 0.08f);
            flatfish[i].GetComponent<Renderer>().material.SetFloat("_SaturationShift", 0.8f);
        }
        flatfish[0].transform.parent.localPosition = new Vector3(1.5f, 0.7f, -3.6f);
        flatfish[0].transform.parent.localRotation = Quaternion.Euler(-80, 148, -155);

        // 불가사리 주변 초기화 (애니메이션 초기화)
        starfish_script.mark_Image.GetComponent<Animator>().enabled = false;
        starfish_script.mark_Image.transform.GetChild(0).gameObject.SetActive(false);
        starfish_script.mark_Image.transform.GetChild(1).gameObject.SetActive(false);
        starfish_script.jealous_Image.GetComponent<Animator>().enabled = false;
        starfish_script.jealous_Image.GetComponent<Image>().color = new Vector4(1, 1, 1, 0);
        starfish_script.oh_Image.GetComponent<Animator>().enabled = false;
        starfish_script.oh_Image.GetComponent<Image>().color = new Vector4(1, 1, 1, 0);
        starfish_script.ouch_Image.GetComponent<Animator>().enabled = false;
        starfish_script.ouch_Image.GetComponent<Image>().color = new Vector4(1, 1, 1, 0);

        // 거북이 초기화
        starfish_script.isUp = false;
        starfish_script.turtle.GetComponent<Animator>().enabled = false;
        starfish_script.turtle.transform.localPosition = new Vector3(6.4f, 0.218f, -3.82f);
        starfish_script.turtle.transform.localRotation = Quaternion.Euler(-4, -15, 0);
        starfish_script.turtle.SetActive(true);

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

            // 2번 챕터 잠금 해제
            setting.ChaperUnlock(2);
            popup.GoNext(1);
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
        // 거북이도 다 올라가고 자막도 다 눌렀다면
        if (isChEnd && isStarfishEnd)
        {
            StartCoroutine("Ending");
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
            if(paper.transform.GetChild(paperNumber).transform.localScale.z <= 0.1f)
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
                if(i <= 8)
                {
                    // fillamount 사용
                    waterLight.transform.GetChild(i).GetComponent<Image>().fillAmount = Mathf.Lerp(waterLight.transform.GetChild(i).GetComponent<Image>().fillAmount, 1, Time.deltaTime * 1);
                    waterLight.transform.GetChild(i).GetComponent<Image>().color = Color.Lerp(waterLight.transform.GetChild(i).GetComponent<Image>().color, new Vector4(1, 1, 1, 1), Time.deltaTime * 1);
                }
                else if(i < 12)
                {
                    waterLight.transform.GetChild(i).GetComponent<Image>().color = Color.Lerp(waterLight.transform.GetChild(i).GetComponent<Image>().color, new Vector4(1, 1, 1, 1), Time.deltaTime * 1);
                }
                else
                {
                    waterLight.transform.GetChild(i).GetComponent<Image>().color = Color.Lerp(waterLight.transform.GetChild(i).GetComponent<Image>().color, new Vector4(1, 1, 1, 0.6f), Time.deltaTime * 1);
                }
            }
        }
        else
        {
            fog.GetComponent<Image>().color = Color.Lerp(fog.GetComponent<Image>().color, new Vector4(0.35f, 0.53f, 0.6f, 0), Time.deltaTime * 0.8f);

            for (int i = 0; i < waterLight.transform.childCount; i++)
            {
                if (i <= 8)
                {
                    // fillamount 사용
                    waterLight.transform.GetChild(i).GetComponent<Image>().fillAmount = Mathf.Lerp(waterLight.transform.GetChild(i).GetComponent<Image>().fillAmount, 0, Time.deltaTime * 0.8f);
                    waterLight.transform.GetChild(i).GetComponent<Image>().color = Color.Lerp(waterLight.transform.GetChild(i).GetComponent<Image>().color, new Vector4(1, 1, 1, 0), Time.deltaTime * 0.8f);
                }
                else if (i < 12)
                {
                    waterLight.transform.GetChild(i).GetComponent<Image>().color = Color.Lerp(waterLight.transform.GetChild(i).GetComponent<Image>().color, new Vector4(1, 1, 1, 0), Time.deltaTime * 0.8f);
                }
                else
                {
                    waterLight.transform.GetChild(i).GetComponent<Image>().color = Color.Lerp(waterLight.transform.GetChild(i).GetComponent<Image>().color, new Vector4(1, 1, 1, 0), Time.deltaTime * 0.8f);
                }
            }

            // 바위라이트 서서히 안보이게
            rockLight.GetComponent<Light>().intensity = Mathf.Lerp(rockLight.GetComponent<Light>().intensity, 0, Time.deltaTime * 0.8f);
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
            // 절벽 초기화
            cliff.transform.localScale = Vector3.Lerp(cliff.transform.localScale, new Vector3(1, 0, 1), Time.deltaTime * 0.8f);
        }
    }
}
