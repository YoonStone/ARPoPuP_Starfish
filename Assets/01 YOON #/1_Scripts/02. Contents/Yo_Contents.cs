using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 왼쪽 -30
public class Yo_Contents : MonoBehaviour
{
    public GameObject popupCtrl;
    public Transform myParent, chpter_3;
    public GameObject[] panels;
    string[] nameSplit;

    public bool isPause;

    [HideInInspector]
    public int chapter;

    public bool isDown;
    private bool isClear;

    Yo_PopupCtrl popup_script;
    Yo_SettingCtrl setting;


    private void Start()
    {
        popup_script = popupCtrl.GetComponent<Yo_PopupCtrl>();
        setting = popupCtrl.GetComponent<Yo_SettingCtrl>();

        // 챕터의 이름에서 챕터 번호만 따서 저장
        nameSplit = transform.gameObject.name.Split('_');
        chapter = int.Parse(nameSplit[0]);
    }

    public void Start1()
    {
        // 바로 터치 못하게
        GetComponent<BoxCollider>().enabled = false;
        transform.GetChild(0).GetChild(0).gameObject.SetActive(false);

        Invoke("RealStart", 4f);
    }

    // 시작한지 3초 후에 클릭할 수 있도록
    public void RealStart()
    {
        GetComponent<BoxCollider>().enabled = true;
        transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
    }

    // 해당 챕터를 터치하면
    void OnMouseDown()
    {
        IslandClick();
    }

    public void IslandClick()
    {
        // 이미 클리어한 챕터라면
        if (isClear)
        {
            // 진행하기
            DownAndNext();
        }
        // 잠겨있다면
        else
        {
            // 애니메이션
            GetComponentInChildren<Animator>().SetTrigger("Start");

            // 진동이 켜져있다면
            if (popupCtrl.GetComponent<Yo_SettingCtrl>().isViabrate)
            {
                // 진동오기
#if UNITY_ANDROID
                Handheld.Vibrate();
#endif
            }
        }
    }

    // 섬 들어가고 넘어가기
    public void DownAndNext()
    {
        // 해당챕터 가라앉기
        isDown = true;

        // 모든 판넬의 콜라이더 없애기 (다른 챕터 터치방지)
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].GetComponent<BoxCollider>().enabled = false;
            panels[i].transform.GetChild(0).gameObject.SetActive(true);
        }

        Invoke("GoChapter", 2.5f);
    }

    // 해당 챕터로 이동
    public void GoChapter()
    {
        popup_script.nowState = chapter;
    }

    public void ReStart()
    {
        isDown = false;
    }

    private void Update()
    {
        switch (chapter)
        {
            case 1: isClear = true; break;
            case 2: isClear = setting.isClear2; break;
            case 3: isClear = setting.isClear3; break;
            case 4: isClear = setting.isClear4; break;
            case 5: isClear = setting.isClear5; break;
        }

        // 이 팻말이 챕터1이 아니라면 잠금상태
        if (chapter != 1)
        {
            // 잠겨있지 않다면
            if (isClear)
            {
                // 텍스트 켜고 자물쇠 끄기
                transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
                transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
            }
            else
            {
                // 텍스트 끄고 자물쇠 켜기
                transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
            }
        }

        if (isDown)
        {
            myParent.localScale = Vector3.Lerp(myParent.localScale, new Vector3(1, 0, 1), Time.deltaTime * 1);

            // 섬 내려가기
            myParent.localPosition = Vector3.Lerp(myParent.localPosition, new Vector3(myParent.localPosition.x, -0.3f, myParent.localPosition.z), Time.deltaTime * 1);

            // 어느정도 가라앉으면
            if (myParent.localScale.y <= 0.8f)
            {
                // 3번 섬 오른쪽으로 이동
                chpter_3.transform.localPosition = Vector3.Lerp(chpter_3.transform.localPosition, new Vector3(5, 0.097f, 3.2f), Time.deltaTime * 0.8f);
            }

            if (myParent.localScale.y <= 0.1f)
            {
                myParent.gameObject.SetActive(false);
                gameObject.SetActive(false);
            }
        }
        else
        {
            myParent.localScale = Vector3.Lerp(myParent.localScale, new Vector3(1, 1, 1), Time.deltaTime * 1);

            // 섬 올라가기
            myParent.localPosition = Vector3.Lerp(myParent.localPosition, new Vector3(myParent.localPosition.x, 0.14f, myParent.localPosition.z), Time.deltaTime * 1);
        }
    }
}
