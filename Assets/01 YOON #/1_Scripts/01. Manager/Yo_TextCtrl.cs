using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Yo_TextCtrl : MonoBehaviour
{
    public GameObject popupMng;

    public GameObject enter, dialogPaper, foldPaper;
    public Transform textTrans, foldTrans;
    public Text sentence;

    Material _material;
    GameObject callFrom;
    Yo_PopupCtrl popup;

    List<Dictionary<string, object>> data_Dialog;

    public bool isTextOpen, isTextClose;
    public int textNumber, chapterNumber = 2;

    private void Start()
    {
        data_Dialog = CSVReader.Read();
        popup = popupMng.GetComponent<Yo_PopupCtrl>();
    }

    // 자막 출력
    public void TextOpen(GameObject fromScript)
    {
        switch (textNumber)
        {
            // 챕터 1 시작
            case 0:
                // 지금 문장 번호(챕터)에 따라 자막 종이 달라지게
                for (int i = 0; i < dialogPaper.transform.childCount; i++)
                {
                    dialogPaper.transform.GetChild(i).gameObject.SetActive(false);
                    foldPaper.transform.GetChild(i).gameObject.SetActive(false);
                }
                dialogPaper.transform.GetChild(0).gameObject.SetActive(true);
                foldPaper.transform.GetChild(0).gameObject.SetActive(true);
                break;
            // 챕터 2 시작
            case 12:
                // 지금 문장 번호(챕터)에 따라 자막 종이 달라지게
                for (int i = 0; i < dialogPaper.transform.childCount; i++)
                {
                    dialogPaper.transform.GetChild(i).gameObject.SetActive(false);
                    foldPaper.transform.GetChild(i).gameObject.SetActive(false);
                }
                dialogPaper.transform.GetChild(1).gameObject.SetActive(true);
                foldPaper.transform.GetChild(1).gameObject.SetActive(true);
                break;
            // 챕터 3 시작
            case 20:
                // 지금 문장 번호(챕터)에 따라 자막 종이 달라지게
                for (int i = 0; i < dialogPaper.transform.childCount; i++)
                {
                    dialogPaper.transform.GetChild(i).gameObject.SetActive(false);
                    foldPaper.transform.GetChild(i).gameObject.SetActive(false);
                }
                dialogPaper.transform.GetChild(2).gameObject.SetActive(true);
                foldPaper.transform.GetChild(2).gameObject.SetActive(true);
                break;
            // 챕터 4 시작
            case 29:
                // 지금 문장 번호(챕터)에 따라 자막 종이 달라지게
                for (int i = 0; i < dialogPaper.transform.childCount; i++)
                {
                    dialogPaper.transform.GetChild(i).gameObject.SetActive(false);
                    foldPaper.transform.GetChild(i).gameObject.SetActive(false);
                }
                dialogPaper.transform.GetChild(3).gameObject.SetActive(true);
                foldPaper.transform.GetChild(3).gameObject.SetActive(true);
                break;
            // 챕터 4 깨끗해진 후
            case 35:
                // 지금 문장 번호(챕터)에 따라 자막 종이 달라지게
                for (int i = 0; i < dialogPaper.transform.childCount; i++)
                {
                    dialogPaper.transform.GetChild(i).gameObject.SetActive(false);
                    foldPaper.transform.GetChild(i).gameObject.SetActive(false);
                }
                dialogPaper.transform.GetChild(4).gameObject.SetActive(true);
                foldPaper.transform.GetChild(4).gameObject.SetActive(true);
                break;
            // 챕터 5 시작
            case 38:
                // 지금 문장 번호(챕터)에 따라 자막 종이 달라지게
                for (int i = 0; i < dialogPaper.transform.childCount; i++)
                {
                    dialogPaper.transform.GetChild(i).gameObject.SetActive(false);
                    foldPaper.transform.GetChild(i).gameObject.SetActive(false);
                }
                dialogPaper.transform.GetChild(0).gameObject.SetActive(true);
                foldPaper.transform.GetChild(0).gameObject.SetActive(true);
                break;
        }
        callFrom = fromScript;

        // CSV파일 파싱한 거 저장하기
        sentence.text = data_Dialog[textNumber]["Content"].ToString();

        // 글씨종이 펼치기
        isTextOpen = true;
        isTextClose = false;

        Invoke("EnterOn", 3f);
    }

    // 자막의 다음 버튼 누르면 호출
    public void TextClose()
    {
        // 다음 문장으로 저장하기
        textNumber++;

        // 엔터 안 보이게
        EnterOff();

        // 다음 문장이 비어있다면
        if (data_Dialog[textNumber]["Content"].ToString() == "")
        {
            // 글씨종이 접기
            isTextClose = true;
            isTextOpen = false;

            // 다음 할 일 하기
            callFrom.SendMessage("NextToDo", textNumber);

            // 다음 문장으로 저장하기
            textNumber++;

            Invoke("EnterOn", 3f);
            //print(textNumber + "자막 들어가기");
        }

        // 비어있지 않다면 대사만 바꾸기
        else
        {
            sentence.text = data_Dialog[textNumber]["Content"].ToString();
            Invoke("EnterOn", 1f);
            print(textNumber + "대사바꾸기");

            if (textNumber == 49 || textNumber == 50)
            {
                // 다음 할 일 하기
                callFrom.SendMessage("NextToDo", textNumber);
            }
        }
    }

    public void EnterOn()
    {
        enter.gameObject.SetActive(true);
    }
    
    public void EnterOff()
    {
        enter.gameObject.SetActive(false);
    }

    void Update()
    {
        // 리셋 중일 때는 못 누르게
        if (popup.isReset)
        {
            enter.gameObject.SetActive(false);
        }
        
        // 글씨 종이 펴지기
        if (isTextOpen)
        {
            // 종이 보이기
            textTrans.gameObject.SetActive(true);

            // 종이 나오기
            textTrans.localPosition = Vector3.Lerp(textTrans.localPosition,
                new Vector3(textTrans.localPosition.x, textTrans.localPosition.y, -5f), Time.deltaTime * 3f);

            // 종이가 거의 다 나오면
            if (textTrans.localPosition.z < -4.9f)
            {
                // 종이 회전
                textTrans.localRotation = Quaternion.Slerp(textTrans.localRotation, Quaternion.Euler(-55, 0, 0), Time.deltaTime * 4.5f);
                foldTrans.localRotation = Quaternion.Slerp(foldTrans.localRotation, Quaternion.Euler(-91f, 40.95f, -90), Time.deltaTime * 4.5f);
            }
        }

        // 글씨 종이 접히기
        if (isTextClose)
        {
            // 종이 회전
            textTrans.localRotation = Quaternion.Slerp(textTrans.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 4.5f);
            foldTrans.localRotation = Quaternion.Slerp(foldTrans.localRotation, Quaternion.Euler(-265, 40.95f, -90), Time.deltaTime * 4.5f);

            // 종이의 회전이 거의 다 끝나면
            if (textTrans.localRotation.x > -0.05)
            {
                // 종이 들어가기
                textTrans.localPosition = Vector3.Lerp(textTrans.localPosition,
                    new Vector3(textTrans.localPosition.x, textTrans.localPosition.y, -3.6f), Time.deltaTime * 3);
            }

            // 다 접히면 안 보이게
            if (textTrans.localPosition.z >= -3.8f)
            {
                textTrans.gameObject.SetActive(false);
            }
        }
    }
}
