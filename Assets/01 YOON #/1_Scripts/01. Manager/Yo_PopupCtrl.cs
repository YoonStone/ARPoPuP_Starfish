using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// -------------------------------------- 챕터 열리는 기능 구현 (표지 제외) ------------------------------------------
// ---이전 챕터 접기---
//  1. 이전 챕터의 왼쪽 면) 접히는 애니메이션
//  2. 이전 챕터의 팝업종이) 책 속으로 들어가는 애니메이션

// ---종이 넘기기---
//  1. 이전 챕터의 오른쪽 면) 넘어가면서 + 접히는 애니메이션
//  2. 지금 챕터의 왼쪽 면) 넘어가면서 + 열리는 애니메이션

// ---지금 챕터 펴지기---
//  1. 지금 챕터의 오른쪽 면) 열리는 애니메이션
//  2. 지금 챕터의 챕터) 챕터 켜기

// ---주변환경 변경---
//  1. 뒷배경 '책장 넘기듯' 바꾸기
//  2. 스카이박스 서서히 바꾸기
//  3. 디렉셔널라이트 서서히 바꾸기

// ---마무리---
//  1. 이전 챕터의 왼쪽 면) 없애기
//  2. 이전 챕터의 오른쪽 면) 없애기
//  3. 이전 챕터의 챕터) 챕터 끄기
//  4. 지금 챕터를 이전 챕터로 저장해두기

// -------------------------------------------------- 표지 & 목차 -------------------------------------------------
// 표지 -> 목차
//  1. 책 사이드 열리기
//  2. 책 표지 열리기
public class Yo_PopupCtrl : MonoBehaviour
{
    [Header("현재 챕터 (개발자용)")]
    public int nowState, beforeState;
    [Header("처음부터 시작하지 않음 (개발자용)")]
    public bool isNoFirst;
    public bool isReset;
    
    public bool isRe0, isRe1, isRe2345, isRe12, isContinue, isBookmark, isSetting, isContent;
    private bool isReSaveState, isFromContent;

    [Header("펼쳐지는 속도 (default=1)")]
    public float speed_Open = 0.9f;
    [Header("책 앞표지")]
    public GameObject cover;
    [Header("책 뒷표지")]
    public GameObject book_Back;
    [Header("책 앞표지 미드")]
    public GameObject cover_Mid_Front;
    [Header("책 뒷표지 미드")]
    public GameObject cover_Mid_Back;
    [Header("책 앞표지 사이드")]
    public GameObject cover_Side_Front;
    [Header("책 뒷표지 사이드")]
    public GameObject cover_Side_Back;
    [Header("입체동화책")]
    public GameObject popupBook;
    [Header("디렉셔널 라이트 (main)")]
    public GameObject light_main;
    [Header("페이지 (표지에서 바로 2,3,4,5)")]
    public GameObject newPage;
    [Header("목차 오른쪽 페이지")]
    public GameObject page_ContentR;
    [Header("표지 팝업 종이")]
    public GameObject bookPaper;
    [Header("목차) 챕터3 섬")]
    public GameObject map_3;
    [Header("목차) 판넬")]
    public GameObject[] panels;

    [Header("페이지 (표지에서 1장으로, 순서대로)")]
    public GameObject[] page;
    public GameObject[] pageSave;
    [Header("맵 왼쪽")]
    public GameObject[] map_L;
    [Header("맵 오른쪽")]
    public GameObject[] map_R;
    [Header("챕터 (script)")]
    public GameObject[] chapter;
    [Header("북마크")]
    public GameObject[] bookmark;
    [Header("설정페이지")]
    public GameObject settingPage;

    //[Header("디렉셔널 라이트")]
    //public GameObject[] lights;

    Yo_AudioCtrl audioCtrl;
    Animator anim_Setting;

    private void Awake()
    {
        audioCtrl = GetComponent<Yo_AudioCtrl>();
        anim_Setting = settingPage.GetComponent<Animator>();

        if (!isNoFirst)
            FirstState();
    }

    void Update()
    {
        switch (nowState)
        {
            // 표지
            case 0:
                isSetting = true;
                break;

            // 목차
            case 10:
                State_Content();
                break;
            // 목차(북마크)
            case 11:
                State_Content_Bookmark();
                break;

            // 1장
            case 1:
                State_Chapter1();
                break;

            // 2장~
            case 2:
            case 3:
            case 4:
            case 5:
                State_Chapter2345();
                break;
            case 6:
                State_BookEnd();

                break;
        }

        if (isSetting)
        {
            bookmark[1].transform.localScale = Vector3.Lerp(bookmark[1].transform.localScale, new Vector3(0, 1, 1), Time.deltaTime * 2f);
        }
        else
        {
            bookmark[1].transform.localScale = Vector3.Lerp(bookmark[1].transform.localScale, new Vector3(1, 1, 1), Time.deltaTime * 2f);
        }
    }

    // 목차
    public void State_Content()
    {
        if (!isRe0)
        {
            audioCtrl.PaperMove();
            isReSaveState = false;
            isSetting = false;

            // 목차 스크립트
            for (int i = 0; i < panels.Length; i++)
            {
                panels[i].SetActive(true);
                panels[i].GetComponent<Yo_Contents>().Start1();
                panels[i].GetComponent<Yo_Contents>().isDown = false;
            }
            isRe1 = false;
            isRe2345 = false;
            isRe12 = false;
            isRe0 = true;
        }

        // 배경음 서서히 켜지기
        audioCtrl.BGMOn();
        audioCtrl.bgmAuto = Mathf.Lerp(audioCtrl.bgmAuto, 0.6f, Time.deltaTime * 1);

        // 표지 북마크 안 보이게
        bookmark[0].transform.localScale = Vector3.Lerp(bookmark[0].transform.localScale, new Vector3(0, 1, 1), Time.deltaTime * 2f);

        // 표지 팝업종이 없어지기
        bookPaper.transform.localScale = Vector3.Lerp(bookPaper.transform.localScale, new Vector3(1, 1, 0), Time.deltaTime * 2f);

        // 더 열리면
        if (cover_Mid_Front.transform.localRotation.z >= 0.8f)
        {
            // 표지 팝업종이 사라지기
            bookPaper.SetActive(false);
        }

        // 표지 팝업종이가 어느정도 들어가면 펴지기
        if (bookPaper.transform.localScale.z <= 0.2f)
            // 입체동화책 없어지기
            popupBook.GetComponent<Animator>().enabled = true;

        // 입체동화책이 어느정도 올라가면 펴지기
        if (popupBook.transform.localPosition.y >= 0.3f)
        {
            // 책 내려가기
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, 0, 0), Time.deltaTime * speed_Open);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * speed_Open);

            // ---표지---
            //  0. 앞표지 사이드 보이게, 뒷표지 사이드 안 보이게
            cover_Side_Front.SetActive(true);
            cover_Side_Back.SetActive(false);
            //  1. 책 사이드 열리기
            cover_Mid_Front.transform.localRotation = Quaternion.Slerp(cover_Mid_Front.transform.localRotation, Quaternion.Euler(0, 0, 90), Time.deltaTime * speed_Open);
            //  2. 책 표지 열리기
            cover.transform.localRotation = Quaternion.Slerp(cover.transform.localRotation, Quaternion.Euler(0, 0, 90), Time.deltaTime * speed_Open);

            // ---페이지---
            //  1. 목차 왼쪽 페이지) 의 2번면 y축 두꺼워지기
            page[0].transform.GetChild(1).transform.localScale = Vector3.Lerp(page[0].transform.GetChild(1).transform.localScale,
                new Vector3(1, 1, 1), Time.deltaTime * speed_Open);
            //  2. 목차 오른쪽 페이지) 의 1번면 y축 두꺼워지기
            page_ContentR.transform.GetChild(0).GetChild(1).transform.localScale = Vector3.Lerp(page_ContentR.transform.GetChild(0).GetChild(1).transform.localScale,
                new Vector3(1, 1, 1), Time.deltaTime * speed_Open);

            // ---주변환경 변경---
            ////  3. 디렉셔널라이트 각도 서서히 바꾸기
            //light_main.transform.localRotation = Quaternion.Slerp(light_main.transform.localRotation, lights[7].transform.localRotation, Time.deltaTime * 2);
            ////  4. 디렉셔널라이트 색깔 서서히 바꾸기
            //light_main.GetComponent<Light>().color = Color.Lerp(light_main.GetComponent<Light>().color, lights[7].GetComponent<Light>().color, Time.deltaTime * 2);
            ////  5. 디렉셔널라이트 밝기 서서히 바꾸기
            //light_main.GetComponent<Light>().intensity = Mathf.Lerp(light_main.GetComponent<Light>().intensity, lights[7].GetComponent<Light>().intensity, Time.deltaTime * 2);
            
            // ---거의 다 열렸을 때---
            if (cover_Mid_Front.transform.localRotation.z >= 0.6f)
            {
                // ---종이 넘기기---
                //  3. 목차의 왼쪽 면) 생기기
                map_L[0].SetActive(true);
                ////  2. 목차의 왼쪽 면) 넘어가기 (-30 ->  0)
                map_L[0].transform.localRotation = Quaternion.Slerp(map_L[0].transform.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * speed_Open);
                //  5. 목차의 왼쪽 면) 열리는 애니메이션
                map_L[0].transform.localScale = Vector3.Lerp(map_L[0].transform.localScale, new Vector3(1, 1, 1), Time.deltaTime * speed_Open);
                //  1-3. 목차의 오른쪽 면) 올라오는 애니메이션
                map_L[0].transform.localPosition = Vector3.Lerp(map_L[0].transform.localPosition, new Vector3(0, 0, 0), Time.deltaTime * speed_Open);

                // ---목차 펴지기---
                //  1-1. 목차의 오른쪽 면) 생기기
                map_R[0].SetActive(true);
                //  1-2. 목차의 오른쪽 면) 열리는 애니메이션
                map_R[0].transform.localScale = Vector3.Lerp(map_R[0].transform.localScale, new Vector3(1, 1, 1), Time.deltaTime * speed_Open);
                //  1-3. 목차의 오른쪽 면) 올라오는 애니메이션
                map_R[0].transform.localPosition = Vector3.Lerp(map_R[0].transform.localPosition, new Vector3(0, -0.05f, 0), Time.deltaTime * speed_Open);

                //  3. 챕터3 섬) 왼쪽으로 이동
                map_3.transform.localPosition = Vector3.Lerp(map_3.transform.localPosition, new Vector3(0.78f, 0.097f, 3.2f), Time.deltaTime * 0.8f);

                //  6. (5초 후에) 지금 챕터를 이전 챕터로 저장해두기 : 지금 페이지가 모두 열리기 전에 저장하면 멈추는 오류 예방
                if (!isReSaveState)
                {
                    StartCoroutine("SaveState", 0);
                    for (int i = 0; i < 4; i++)
                    {
                        newPage.transform.GetChild(i).gameObject.SetActive(false);
                        newPage.transform.GetChild(i).GetChild(1).localScale = new Vector3(1, 0.05f, 1);
                    }
                    for (int i = 0; i < page.Length; i++)
                    {
                        page[i] = pageSave[i];
                    }
                    // 5장에서 책 덮히고 넘어오느라 달라진 위치각도 초기화
                    newPage.transform.localRotation = Quaternion.Euler(0, 0, 0.2f);
                    newPage.SetActive(true);
                    page[6].transform.localRotation = Quaternion.Euler(0, 0, 0.2f);
                    map_L[5].transform.localRotation = Quaternion.Euler(0, 0, -179);
                    map_R[5].transform.localRotation = Quaternion.Euler(0, 0, 0.2f);
                    chapter[5].SetActive(false);
                    isReSaveState = true;
                }
            }
        }
    }

    // 목차(북마크) 뒷장에서 넘어옴
    public void State_Content_Bookmark()
    {
        if (!isRe0)
        {
            audioCtrl.PaperMove();
            isReSaveState = false;

            // 목차 섬들 다 원상태로
            map_L[0].transform.GetChild(0).gameObject.SetActive(true);
            map_L[0].transform.GetChild(1).gameObject.SetActive(true);
            map_R[0].transform.GetChild(0).gameObject.SetActive(true);
            map_R[0].transform.GetChild(1).gameObject.SetActive(true);
            map_R[0].transform.GetChild(2).gameObject.SetActive(true);
            
            // 섬들 올라오기
            for (int i = 0; i < panels.Length; i++)
            {
                panels[i].SetActive(true);
                panels[i].GetComponent<Yo_Contents>().Start1();
                panels[i].GetComponent<Yo_Contents>().isDown = false;
            }
            isRe1 = false;
            isRe2345 = false;
            isRe12 = false;
            isRe0 = true;
        }

        // 표지 북마크 안 보이게
        bookmark[0].transform.localScale = Vector3.Lerp(bookmark[0].transform.localScale, new Vector3(0, 1, 1), Time.deltaTime * 2f);

        if (beforeState != 6)
        {
            // 이전 챕터 저장한 거랑 충돌하지 않도록
            if (beforeState != nowState && beforeState != 0)
            {
                // ---목차 열기---
                //  1. 목차의 왼쪽 면) 열리는 애니메이션
                map_L[0].SetActive(true);
                map_L[0].transform.localScale = Vector3.Lerp(map_L[0].transform.localScale, new Vector3(1, 1, 1), Time.deltaTime * speed_Open);

                // ---목차 종이 넘기기---
                //  1. 목차의 오른쪽 면) 넘어오기
                map_R[0].SetActive(true);
                map_R[0].transform.localRotation = Quaternion.Slerp(map_R[0].transform.localRotation, Quaternion.Euler(0, 0, 0.2f), Time.deltaTime * speed_Open);
                //  2. 목차의 오른쪽 면) 열리는 애니메이션
                map_R[0].transform.localScale = Vector3.Lerp(map_R[0].transform.localScale, new Vector3(1, 1, 1), Time.deltaTime * speed_Open);

                // ---목차 페이지---
                //  1. 목차 왼쪽 페이지) 의 2번면 y축 두꺼워지기
                page[0].SetActive(true);
                page[0].transform.GetChild(1).transform.localScale = Vector3.Lerp(page[0].transform.GetChild(1).transform.localScale,
                    new Vector3(1, 1, 1), Time.deltaTime * 2.5f);
                //  2. 목차 오른쪽 페이지) 넘어오기 (목차오른쪽)
                page_ContentR.SetActive(true);
                page_ContentR.transform.localRotation = Quaternion.Slerp(page_ContentR.transform.localRotation, Quaternion.Euler(0, 0, 0.2f), Time.deltaTime * speed_Open);
                //  3. 목차 오른쪽 페이지) 의 1번면 y축 두꺼워지기
                page_ContentR.transform.GetChild(0).GetChild(1).transform.localScale = Vector3.Lerp(page_ContentR.transform.GetChild(0).GetChild(1).transform.localScale,
                    new Vector3(1, 1, 1), Time.deltaTime * 2.5f);

                // ---이전 맵 넘어가기---
                //  2. 이전 맵의 왼쪽 면) 넘어가기
                map_L[beforeState].transform.localRotation = Quaternion.Slerp(map_L[beforeState].transform.localRotation, Quaternion.Euler(0, 0, -179), Time.deltaTime * speed_Open);
                //  3. 이전 맵의 왼쪽 면) 없어지는 애니메이션
                map_L[beforeState].transform.localScale = Vector3.Lerp(map_L[beforeState].transform.localScale, new Vector3(1, 0, 1), Time.deltaTime * 2.5f);

                // ---이전 맵 닫히기---
                //  이전 맵의 오른쪽 면) 닫히는 애니메이션
                map_R[beforeState].transform.localScale = Vector3.Lerp(map_R[beforeState].transform.localScale, new Vector3(1, 0, 1), Time.deltaTime * 2.5f);

                // 원래 챕터의 오른쪽 페이지) 의 1번면 y축 얇아지기
                page[beforeState + 1].transform.GetChild(0).GetChild(1).transform.localScale = Vector3.Lerp(page[beforeState + 1].transform.GetChild(0).GetChild(1).transform.localScale,
                    new Vector3(1, 0.05f, 1), Time.deltaTime * 2.5f);

                // ---오른쪽 뒷 페이지---
                // 원래 챕터의 왼쪽 페이지) 넘어오기
                page[beforeState].transform.localRotation = Quaternion.Slerp(page[beforeState].transform.localRotation, Quaternion.Euler(0, 0, 0.2f), Time.deltaTime * speed_Open);

                switch (beforeState)
                {
                    // 1장에서 넘어온 거라면
                    case 1:
                        // 왼쪽 절벽은 더 빠르게 닫히기
                        map_L[1].transform.GetChild(3).localScale = Vector3.Lerp(map_L[1].transform.GetChild(3).localScale, new Vector3(1, 0, 1), Time.deltaTime * 1.2f);

                        // 원래 챕터의 왼쪽 페이지) 의 2번면 y축 얇아지기
                        page[1].transform.GetChild(0).GetChild(1).transform.localScale = Vector3.Lerp(page[1].transform.GetChild(0).GetChild(1).transform.localScale,
                            new Vector3(1, 0.05f, 1), Time.deltaTime * 2.5f);
                        break;

                    // 2~5장에서 넘어온 거라면
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                        // 원래 챕터의 왼쪽 페이지) 의 2번면 y축 얇아지기
                        page[beforeState].transform.GetChild(1).GetChild(1).transform.localScale = Vector3.Lerp(page[beforeState].transform.GetChild(1).GetChild(1).transform.localScale,
                            new Vector3(1, 0.05f, 1), Time.deltaTime * 2.5f);

                        break;
                }

                // --- 어느정도 닫혔을 때 ---
                if (map_R[0].transform.localRotation.z <= 0.5f)
                {
                    // 이전 챕터 왼쪽 페이지, 맵) 없어지기
                    page[beforeState].SetActive(false);
                    map_L[beforeState].SetActive(false);
                }
                // ---거의 다 닫혔을 때---
                if (map_R[0].transform.localRotation.z <= 0.02f)
                {
                    // 이전 챕터 오른쪽 페이지, 맵) 없어지기
                    page[beforeState + 1].SetActive(false);
                    map_R[beforeState].SetActive(false);

                    //  챕터3 섬) 왼쪽으로 이동
                    map_3.transform.localPosition = Vector3.Lerp(map_3.transform.localPosition, new Vector3(0.78f, 0.097f, 3.2f), Time.deltaTime * 0.8f);

                    //  6. (5초 후에) 지금 챕터를 이전 챕터로 저장해두기 : 지금 페이지가 모두 열리기 전에 저장하면 멈추는 오류 예방
                    if (!isReSaveState)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            newPage.transform.GetChild(i).gameObject.SetActive(false);
                            newPage.transform.GetChild(i).GetChild(1).localScale = new Vector3(1, 0.05f, 1);
                        }
                        for (int i = 0; i < page.Length; i++)
                        {
                            page[i] = pageSave[i];
                        }
                        // 5장에서 책 덮히고 넘어오느라 달라진 위치각도 초기화
                        newPage.transform.localRotation = Quaternion.Euler(0, 0, 0.2f);
                        newPage.SetActive(true);
                        page[6].transform.localRotation = Quaternion.Euler(0, 0, 0.2f);
                        map_L[5].transform.localRotation = Quaternion.Euler(0, 0, -179);
                        map_R[5].transform.localRotation = Quaternion.Euler(0, 0, 0.2f);
                        chapter[5].SetActive(false);

                        print("넘어간 뒤 초기화");
                        for (int i = 1; i < 6; i++)
                        {
                            page[i].transform.localRotation = Quaternion.Euler(0, 0, 0.2f);
                            page[i + 1].transform.localRotation = Quaternion.Euler(0, 0, 0.2f);
                            map_L[i].transform.localRotation = Quaternion.Euler(0, 0, -179);
                            map_R[i].transform.localRotation = Quaternion.Euler(0, 0, 0.2f);
                            chapter[i].SetActive(false);
                        }

                        StartCoroutine("SaveState", 0);
                        isReSaveState = true;
                    }
                }
            }
        }
        // 맨 뒷장에서 넘어온거라면
        else
        {
            // ---표지---
            //  0. 앞표지 사이드 안 보이게, 뒷표지 사이드 보이게
            cover_Side_Front.SetActive(false);
            cover_Side_Back.SetActive(true);
            //  1. 책 사이드 열리기
            cover_Mid_Back.transform.localRotation = Quaternion.Slerp(cover_Mid_Back.transform.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * speed_Open);
            //  2. 책 표지 열리기
            book_Back.transform.localRotation = Quaternion.Slerp(book_Back.transform.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * speed_Open);

            // ---목차 열기---
            //  1. 목차의 왼쪽 면) 열리는 애니메이션
            map_L[0].SetActive(true);
            map_L[0].transform.localScale = Vector3.Lerp(map_L[0].transform.localScale, new Vector3(1, 1, 1), Time.deltaTime * 2.5f);

            // ---목차 종이 넘기기---
            //  1. 목차의 오른쪽 면) 넘어오기
            map_R[0].SetActive(true);
            map_R[0].transform.localRotation = Quaternion.Slerp(map_R[0].transform.localRotation, Quaternion.Euler(0, 0, 0.2f), Time.deltaTime * speed_Open);
            //  2. 목차의 오른쪽 면) 열리는 애니메이션
            map_R[0].transform.localScale = Vector3.Lerp(map_R[0].transform.localScale, new Vector3(1, 1, 1), Time.deltaTime * 2.5f);

            // ---목차 페이지---
            //  1. 목차 왼쪽 페이지) 의 2번면 y축 두꺼워지기
            page[0].SetActive(true);
            page[0].transform.GetChild(1).transform.localScale = Vector3.Lerp(page[0].transform.GetChild(1).transform.localScale,
                new Vector3(1, 1, 1), Time.deltaTime * 2.5f);
            //  2. 목차 오른쪽 페이지) 넘어오기 (목차오른쪽)
            page_ContentR.SetActive(true);
            page_ContentR.transform.localRotation = Quaternion.Slerp(page_ContentR.transform.localRotation, Quaternion.Euler(0, 0, 0.2f), Time.deltaTime * speed_Open);
            //  3. 목차 오른쪽 페이지) 의 1번면 y축 두꺼워지기
            page_ContentR.transform.GetChild(0).GetChild(1).transform.localScale = Vector3.Lerp(page_ContentR.transform.GetChild(0).GetChild(1).transform.localScale,
                new Vector3(1, 1, 1), Time.deltaTime * 2.5f);

            // 거의 다 열렸을 때
            if (book_Back.transform.localRotation.z <= 0.09f)
            {
                //  챕터3 섬) 왼쪽으로 이동
                map_3.transform.localPosition = Vector3.Lerp(map_3.transform.localPosition, new Vector3(0.78f, 0.097f, 3.2f), Time.deltaTime * 0.8f);

                //  6. (5초 후에) 지금 챕터를 이전 챕터로 저장해두기 : 지금 페이지가 모두 열리기 전에 저장하면 멈추는 오류 예방
                if (!isReSaveState)
                {
                    StartCoroutine("SaveState", 0);
                    for (int i = 0; i < 4; i++)
                    {
                        newPage.transform.GetChild(i).gameObject.SetActive(false);
                        newPage.transform.GetChild(i).GetChild(1).localScale = new Vector3(1, 0.05f, 1);
                    }
                    for (int i = 0; i < page.Length; i++)
                    {
                        page[i] = pageSave[i];
                    }
                    // 5장에서 책 덮히고 넘어오느라 달라진 위치각도 초기화
                    newPage.transform.localRotation = Quaternion.Euler(0, 0, 0.2f);
                    newPage.SetActive(true);
                    page[6].transform.localRotation = Quaternion.Euler(0, 0, 0.2f);
                    map_L[5].transform.localRotation = Quaternion.Euler(0, 0, -179);
                    map_R[5].transform.localRotation = Quaternion.Euler(0, 0, 0.2f);
                    chapter[5].SetActive(false);

                    print("넘어간 뒤 초기화");
                    for (int i = 1; i < 6; i++)
                    {
                        page[i].transform.localRotation = Quaternion.Euler(0, 0, 0.2f);
                        page[i + 1].transform.localRotation = Quaternion.Euler(0, 0, 0.2f);
                        map_L[i].transform.localRotation = Quaternion.Euler(0, 0, -179);
                        map_R[i].transform.localRotation = Quaternion.Euler(0, 0, 0.2f);
                        chapter[i].SetActive(false);
                    }
                    isReSaveState = true;
                }
            }
        }
    }

    // 1장
    public void State_Chapter1()
    {
        // 시작할 때 한번만
        if (!isRe1)
        {
            audioCtrl.PaperMove();
            isBookmark = false;
            isContent = false;
            isReSaveState = false;
            isRe0 = false;
            isRe2345 = false;
            isReset = false;
            for (int i = 0; i < 4; i++)
            {
                newPage.transform.GetChild(i).gameObject.SetActive(false);
                newPage.transform.GetChild(i).GetChild(1).localScale = new Vector3(1, 0.05f, 1);
            }
            for (int i = 0; i < page.Length; i++)
            {
                page[i] = pageSave[i];
            }
            page[1].transform.localRotation = Quaternion.Euler(0, 0, 0.2f);
            map_L[1].transform.localRotation = Quaternion.Euler(0, 0, -179);
            map_R[1].transform.localRotation = Quaternion.Euler(0, 0, 0.2f);
            chapter[1].SetActive(false);
            isRe12 = false;
            isRe1 = true;
        }

        if (!isBookmark)
        {
            // 북마크 누를 수 없게
            bookmark[0].transform.localScale = Vector3.Lerp(bookmark[0].transform.localScale, new Vector3(0, 1, 1), Time.deltaTime * 2f);
        }
        else
        {
            if (isContent)
            {
                bookmark[0].transform.localScale = Vector3.Lerp(bookmark[0].transform.localScale, new Vector3(0, 1, 1), Time.deltaTime * 2f);
            }
            else
            {
                bookmark[0].transform.localScale = Vector3.Lerp(bookmark[0].transform.localScale, new Vector3(1, 1, 1), Time.deltaTime * 2f);
            }
        }

        // 이전 챕터 저장한 거랑 충돌하지 않도록
        if (beforeState != 1)
        {
            // 1장의 왼쪽 페이지, 맵) 생기기
            map_L[1].SetActive(true);
            page[1].SetActive(true);
            //  1장의 오른쪽 페이지, 맵) 생기기
            page[2].SetActive(true);
            map_R[1].SetActive(true);

            // 왼쪽 절벽은 더 느리게 펴지기
            map_L[1].transform.GetChild(3).localScale = Vector3.Lerp(map_L[1].transform.GetChild(3).localScale, new Vector3(1, 1, 1), Time.deltaTime * 0.8f);

            // ---카메라,책표지 마저 끝내기---
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, 0, 0), Time.deltaTime * speed_Open);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * speed_Open);
            // 책 사이드 열리기
            cover_Mid_Front.transform.rotation = Quaternion.Slerp(cover_Mid_Front.transform.rotation, Quaternion.Euler(0, 0, 90), Time.deltaTime * speed_Open);
            // 책 표지 열리기
            cover.transform.localRotation = Quaternion.Slerp(cover.transform.localRotation, Quaternion.Euler(0, 0, 90), Time.deltaTime * speed_Open);

            // ---목차 접기---
            // 목차의 왼쪽 맵) 접히는 애니메이션
            map_L[0].transform.localScale = Vector3.Lerp(map_L[0].transform.localScale, new Vector3(1, 0, 1), Time.deltaTime * speed_Open);

            // 목차의 오른쪽 맵) 넘어가기
            map_R[0].transform.localRotation = Quaternion.Slerp(map_R[0].transform.localRotation, Quaternion.Euler(0, 0, 180), Time.deltaTime * speed_Open);
            // 목차의 오른쪽 맵) 접히는 애니메이션
            map_R[0].transform.localScale = Vector3.Lerp(map_R[0].transform.localScale, new Vector3(1, 0, 1), Time.deltaTime * speed_Open);

            // ---1장 맵 펼치기---
            // 1장의 왼쪽 맵) 넘어가기
            map_L[1].transform.localRotation = Quaternion.Slerp(map_L[1].transform.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * speed_Open);
            // 1장의 왼쪽 맵) 열리는 애니메이션
            map_L[1].transform.localScale = Vector3.Lerp(map_L[1].transform.localScale, new Vector3(1, 1, 1), Time.deltaTime * speed_Open);
            // 1장의 오른쪽 맵) 열리는 애니메이션
            map_R[1].transform.localScale = Vector3.Lerp(map_R[1].transform.localScale, new Vector3(1, 1, 1), Time.deltaTime * speed_Open);

            // ---페이지---
            //  1. 목차 왼쪽 페이지) 의 2번면 y축 얇아지기
            page[0].transform.GetChild(1).transform.localScale = Vector3.Lerp(page[0].transform.GetChild(1).transform.localScale,
                new Vector3(1, 0.05f, 1), Time.deltaTime * 2.5f);
            //  2. 목차 오른쪽 페이지) 넘어가기 (목차오른쪽 + 1장 왼쪽)
            page_ContentR.transform.localRotation = Quaternion.Slerp(page_ContentR.transform.localRotation, Quaternion.Euler(0, 0, 180), Time.deltaTime * speed_Open);
            page[1].transform.localRotation = Quaternion.Slerp(page[1].transform.localRotation, Quaternion.Euler(0, 0, 180), Time.deltaTime * speed_Open);
            //  3. 목차 오른쪽 페이지) 의 1번면 y축 얇아지기
            page_ContentR.transform.GetChild(0).GetChild(1).transform.localScale = Vector3.Lerp(page_ContentR.transform.GetChild(0).GetChild(1).transform.localScale,
                new Vector3(1, 0.05f, 1), Time.deltaTime * 2.5f);
            //  4. 목차 오른쪽 페이지) 의 2번면 y축 두꺼워지기
            page[1].transform.GetChild(0).GetChild(1).transform.localScale = Vector3.Lerp(page[1].transform.GetChild(0).GetChild(1).transform.localScale,
                new Vector3(1, 1, 1), Time.deltaTime * speed_Open);
            //  5. 오른쪽에 새로운 페이지) 생기기
            page[2].SetActive(true);
            //  6. 오른쪽에 새로운 페이지) 의 1번면 y축 두꺼워지기
            page[2].transform.GetChild(0).GetChild(1).transform.localScale = Vector3.Lerp(page[2].transform.GetChild(0).GetChild(1).transform.localScale,
                new Vector3(1, 1, 1), Time.deltaTime * speed_Open);

            // 어느정도 열렸을 때 해초 애니메이션 시작
            if (map_L[1].transform.localRotation.z >= -0.6f)
            {
                if (!chapter[1].activeInHierarchy)
                {
                    //  5. 1장의 챕터) 챕터 켜기
                    chapter[1].SetActive(true);
                    chapter[1].SendMessage("Start1");
                }
            }

            // ---거의 다 열렸을 때---
            if (map_L[1].transform.localRotation.z >= -0.05f)
            {
                //  1. 목차의 왼쪽 면) 없애기
                map_L[0].SetActive(false);

                //  2. 목차의 오른쪽 면) 없애기
                map_R[0].SetActive(false);

                //  0. 목차 페이지 없애기
                page[0].SetActive(false);

                // 섬들 끄기
                for (int i = 0; i < panels.Length; i++)
                {
                    panels[i].SetActive(false);
                }

                //  6. (5초 후에) 지금 챕터를 이전 챕터로 저장해두기 : 지금 페이지가 모두 열리기 전에 저장하면 멈추는 오류 예방
                if (!isReSaveState)
                {
                    StartCoroutine("SaveState", 1);
                    isReSaveState = true;
                }
            }

            // ---주변환경 변경---
            ////  3. 디렉셔널라이트 각도 서서히 바꾸기
            //light_main.transform.localRotation = Quaternion.Slerp(light_main.transform.localRotation, lights[nowState].transform.localRotation, Time.deltaTime * 2);
            ////  4. 디렉셔널라이트 색깔 서서히 바꾸기
            //light_main.GetComponent<Light>().color = Color.Lerp(light_main.GetComponent<Light>().color, lights[nowState].GetComponent<Light>().color, Time.deltaTime * 2);
            ////  5. 디렉셔널라이트 밝기 서서히 바꾸기
            //light_main.GetComponent<Light>().intensity = Mathf.Lerp(light_main.GetComponent<Light>().intensity, lights[nowState].GetComponent<Light>().intensity, Time.deltaTime * 2);
        }
    }

    // 2,3,4,5장
    public void State_Chapter2345()
    {
        // 시작할 때 한번만
        if (!isRe2345)
        {
            audioCtrl.PaperMove();
            isBookmark = false;
            isContent = false;
            isReSaveState = false;
            isRe0 = false;
            isRe1 = false;
            isReset = false;
            isRe12 = false;
            isFromContent = false;
            isRe12 = false;
            isRe2345 = true;
        }

        if (!isBookmark)
        {
            // 북마크 누를 수 없게
            bookmark[0].transform.localScale = Vector3.Lerp(bookmark[0].transform.localScale, new Vector3(0, 1, 1), Time.deltaTime * 2f);
        }
        else
        {
            if (isContent)
            {
                bookmark[0].transform.localScale = Vector3.Lerp(bookmark[0].transform.localScale, new Vector3(0, 1, 1), Time.deltaTime * 2f);
            }
            else
            {
                bookmark[0].transform.localScale = Vector3.Lerp(bookmark[0].transform.localScale, new Vector3(1, 1, 1), Time.deltaTime * 2f);
            }
        }

        // 이전 챕터 저장한 거랑 충돌하지 않도록
        if (beforeState != nowState)
        {
            // 만약 이전 챕터가 목차라면
            if (beforeState == 0 || beforeState == 10 || beforeState == 11)
            {
                // 순서대로 진행한 것이 아니라 목차에서 바로 넘어왔다는 것을 저장
                isFromContent = true;
            }
            // 만약 이전 챕터가 목차가 아니라면
            else
            {
                // 순서대로 진행 중이라는 것을 저장
                isFromContent = false;
            }

            // ---이전 챕터 접기---
            //  1. 이전 챕터의 왼쪽 면) 접히는 애니메이션
            map_L[beforeState].transform.localScale = Vector3.Lerp(map_L[beforeState].transform.localScale, new Vector3(1, 0, 1), Time.deltaTime * speed_Open);

            // ---이전 종이 넘기기---
            //  1. 이전 챕터의 오른쪽 면) 넘어가기
            map_R[beforeState].transform.localRotation = Quaternion.Slerp(map_R[beforeState].transform.localRotation, Quaternion.Euler(0, 0, 180), Time.deltaTime * speed_Open);
            //  2. 이전 챕터의 오른쪽 면) 접히는 애니메이션
            map_R[beforeState].transform.localScale = Vector3.Lerp(map_R[beforeState].transform.localScale, new Vector3(1, 0, 1), Time.deltaTime * speed_Open);

            // ---지금 종이 넘기기---
            //  1. 지금 챕터의 왼쪽 면) 생기기
            map_L[nowState].SetActive(true);
            //  2. 지금 챕터의 왼쪽 면) 넘어가기
            map_L[nowState].transform.localRotation = Quaternion.Slerp(map_L[nowState].transform.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * speed_Open);
            //  3. 지금 챕터의 왼쪽 면) 열리는 애니메이션
            map_L[nowState].transform.localScale = Vector3.Lerp(map_L[nowState].transform.localScale, new Vector3(1, 1, 1), Time.deltaTime * speed_Open);

            // ---지금 챕터 펴지기---
            //  1-1. 지금 챕터의 오른쪽 면) 생기기
            map_R[nowState].SetActive(true);
            //  1-2. 지금 챕터의 오른쪽 면) 열리는 애니메이션
            map_R[nowState].transform.localScale = Vector3.Lerp(map_R[nowState].transform.localScale, new Vector3(1, 1, 1), Time.deltaTime * speed_Open);

            // 만약 표지에서 바로 2,3,4,5장으로 간 상황이라면
            if (isFromContent)
            {
                // ---카메라,책표지 마저 끝내기---
                transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, 0, 0), Time.deltaTime * speed_Open);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * speed_Open);
                // 책 사이드 열리기
                cover_Mid_Front.transform.rotation = Quaternion.Slerp(cover_Mid_Front.transform.rotation, Quaternion.Euler(0, 0, 90), Time.deltaTime * speed_Open);
                // 책 표지 열리기
                cover.transform.localRotation = Quaternion.Slerp(cover.transform.localRotation, Quaternion.Euler(0, 0, 90), Time.deltaTime * speed_Open);

                // ---페이지---
                //  0. 원래 넘어가려고 있었던 목차의 오른쪽 페이지) 없애기
                page[1].SetActive(false);
                //  1. 넘어갈 오른쪽 페이지의 뒷면) 생기기
                newPage.SetActive(true);
                //  2. 넘어갈 오른쪽 페이지) 의 뒷면 제작 (지금 시작할 챕터의 왼쪽면)
                newPage.transform.GetChild(nowState - 2).gameObject.SetActive(true);
                //  3. 목차 왼쪽 페이지) 의 2번면 y축 얇아지기
                page[0].transform.GetChild(1).transform.localScale = Vector3.Lerp(page[0].transform.GetChild(1).transform.localScale,
                    new Vector3(1, 0.05f, 1), Time.deltaTime * 2.5f);
                //  4. 넘어갈 오른쪽 페이지) 넘어가기 (목차오른쪽면 + 새로생긴 오른쪽뒷면)
                page_ContentR.transform.localRotation = Quaternion.Slerp(page_ContentR.transform.localRotation, Quaternion.Euler(0, 0, 180), Time.deltaTime * speed_Open);
                newPage.transform.localRotation = Quaternion.Slerp(newPage.transform.localRotation, Quaternion.Euler(0, 0, 180), Time.deltaTime * speed_Open);
                //  5. 목차 오른쪽 페이지) 의 1번면 y축 얇아지기
                page_ContentR.transform.GetChild(0).GetChild(1).transform.localScale = Vector3.Lerp(page_ContentR.transform.GetChild(0).GetChild(1).transform.localScale,
                    new Vector3(1, 0.05f, 1), Time.deltaTime * 2.5f);
                //  6. 넘어갈 오른쪽 페이지) 의 2번면(새로 생긴면) y축 두꺼워지기
                newPage.transform.GetChild(nowState - 2).GetChild(1).transform.localScale = Vector3.Lerp(newPage.transform.GetChild(nowState - 2).GetChild(1).transform.localScale,
                    new Vector3(1, 1, 1), Time.deltaTime * 2.5f);
                //  7. 오른쪽에 새로운 페이지 생기기
                page[nowState + 1].SetActive(true);
                //  8. 오른쪽에 새로운 페이지) 의 1번면 y축 두꺼워지기
                page[nowState + 1].transform.GetChild(0).GetChild(1).transform.localScale = Vector3.Lerp(page[nowState + 1].transform.GetChild(0).GetChild(1).transform.localScale,
                    new Vector3(1, 1, 1), Time.deltaTime * 2.5f);
            }

            // 순서대로 진행 중이라면
            else
            {
                // 이전 챕터의 왼쪽 페이지 줄어들기
                if(beforeState == 1)
                {
                    page[beforeState].transform.GetChild(0).GetChild(1).transform.localScale = Vector3.Lerp(page[beforeState].transform.GetChild(0).GetChild(1).transform.localScale,
                        new Vector3(1, 0.05f, 1), Time.deltaTime * 2.5f);
                }
                else
                {
                    page[beforeState].transform.GetChild(1).GetChild(1).transform.localScale = Vector3.Lerp(page[beforeState].transform.GetChild(1).GetChild(1).transform.localScale,
                        new Vector3(1, 0.05f, 1), Time.deltaTime * 2.5f);
                }
                //  1. 원래 있던 오른쪽 페이지 넘어가기
                page[nowState].transform.localRotation = Quaternion.Slerp(page[nowState].transform.localRotation, Quaternion.Euler(0, 0, 180), Time.deltaTime * speed_Open);
                //  2. 원래 있던 오른쪽 페이지) 의 1번면 y축 얇아지기
                page[nowState].transform.GetChild(0).GetChild(1).transform.localScale = Vector3.Lerp(page[nowState].transform.GetChild(0).GetChild(1).transform.localScale,
                    new Vector3(1, 0.05f, 1), Time.deltaTime * 2.5f);
                //  3. 원래 있던 오른쪽 페이지) 의 2번면 y축 두꺼워지기
                page[nowState].transform.GetChild(1).GetChild(1).transform.localScale = Vector3.Lerp(page[nowState].transform.GetChild(1).GetChild(1).transform.localScale,
                    new Vector3(1, 1, 1), Time.deltaTime * 2.5f);
                //  4. 오른쪽에 새로운 페이지 생기기
                page[nowState + 1].SetActive(true);
                //  5. 오른쪽에 새로운 페이지) 의 1번면 y축 두꺼워지기
                page[nowState + 1].transform.GetChild(0).GetChild(1).transform.localScale = Vector3.Lerp(page[nowState + 1].transform.GetChild(0).GetChild(1).transform.localScale,
                    new Vector3(1, 1, 1), Time.deltaTime * 2.5f);

            }

            // ---주변환경 변경---
            ////  3. 디렉셔널라이트 각도 서서히 바꾸기
            //light_main.transform.localRotation = Quaternion.Slerp(light_main.transform.localRotation, lights[nowState].transform.localRotation, Time.deltaTime * 2);
            ////  4. 디렉셔널라이트 위치 서서히 바꾸기
            //light_main.transform.localPosition = Vector3.Lerp(light_main.transform.localPosition, lights[nowState].transform.localPosition, Time.deltaTime * 2);
            ////  5. 디렉셔널라이트 색깔 서서히 바꾸기
            //light_main.GetComponent<Light>().color = Color.Lerp(light_main.GetComponent<Light>().color, lights[nowState].GetComponent<Light>().color, Time.deltaTime * 2);

            // 2장, 4장 이라면
            if (nowState == 2 || nowState == 4)
            {
                //// 2/3쯤 바다 먼저 펼쳐지게
                if (map_L[nowState].transform.localRotation.z >= -0.5f)
                {
                    if (!chapter[nowState].activeInHierarchy)
                    {
                        // 오른쪽면 같이 펴지기
                        chapter[nowState].SetActive(true);
                        chapter[nowState].SendMessage("Start1");
                    }
                }
            }

            if (nowState == 3)
            {
                if (!chapter[3].activeInHierarchy)
                {
                    //  5. 5장의 챕터) 챕터 켜기
                    chapter[3].SetActive(true);
                    chapter[3].SendMessage("Start1");
                }
            }

            if (nowState == 5)
            {
                // 어느정도 열렸을 때 해초 애니메이션 시작
                if (map_L[nowState].transform.localRotation.z >= -0.6f)
                {
                    if (!chapter[nowState].activeInHierarchy)
                    {
                        //  5. 5장의 챕터) 챕터 켜기
                        chapter[nowState].SetActive(true);
                        chapter[nowState].SendMessage("Start1");
                    }
                }
            }

            // ---거의 다 열렸을 때---
            if (map_L[nowState].transform.localRotation.z >= -0.09f)
            {
                // 만약 표지에서 바로 2,3,4,5장으로 간 상황이라면
                if (isFromContent)
                {
                    //  0. 목차 페이지 없애기
                    page[0].SetActive(false);
                    //  1. 목차의 왼쪽 면) 없애기
                    map_L[0].SetActive(false);
                    //  2. 목차의 오른쪽 면) 없애기
                    map_R[0].SetActive(false);
                    //  3. 새로 만든 넘어갈 오른쪽 페이지를 다음 챕터 때 이전 페이지로 인식하도록
                    page[nowState] = newPage;
                    //  4. 이전 챕터의 챕터) 챕터 끄기
                    chapter[beforeState].SetActive(false);
                }

                // 순서대로 진행 중이라면
                else
                {
                    //  1. 이전 챕터의 왼쪽 면) 없애기
                    map_L[beforeState].SetActive(false);
                    //  2. 이전 챕터의 오른쪽 면) 없애기
                    map_R[beforeState].SetActive(false);
                    //  3. 이전 왼쪽 페이지) 없애기
                    page[beforeState].SetActive(false);
                    // + 혹시 page_ContentR이 켜져있다면 끄기
                    page_ContentR.SetActive(false);
                    //  4. 이전 챕터의 챕터) 챕터 끄기
                    chapter[beforeState].SetActive(false);

                    // 혹시 page_ContentR이 켜져있다면 끄기
                    page_ContentR.SetActive(false);
                }

                //  6. (5초 후에) 지금 챕터를 이전 챕터로 저장해두기 : 지금 페이지가 모두 열리기 전에 저장하면 멈추는 오류 예방
                if (!isReSaveState)
                {
                    StartCoroutine("SaveState", nowState);
                    isReSaveState = true;
                }

                // 다시 반복하지 않도록
                isFromContent = false;
            }
        }
    }

    // 맨 뒷장
    public void State_BookEnd()
    {
        // 시작할 때 한번만
        if (!isRe12)
        {
            audioCtrl.PaperMove();
            isBookmark = false;
            isContent = false;
            isReSaveState = false;
            isRe0 = false;
            isRe2345 = false;
            isReset = false;

            isRe12 = true;
        }

        if (!isBookmark)
        {
            // 북마크 누를 수 없게
            bookmark[0].transform.localScale = Vector3.Lerp(bookmark[0].transform.localScale, new Vector3(0, 1, 1), Time.deltaTime * 2f);
        }
        else
        {
            if (isContent)
            {
                bookmark[0].transform.localScale = Vector3.Lerp(bookmark[0].transform.localScale, new Vector3(0, 1, 1), Time.deltaTime * 2f);
            }
            else
            {
                bookmark[0].transform.localScale = Vector3.Lerp(bookmark[0].transform.localScale, new Vector3(1, 1, 1), Time.deltaTime * 2f);
            }
        }

        //  6. (5초 후에) 지금 챕터를 이전 챕터로 저장해두기 : 지금 페이지가 모두 열리기 전에 저장하면 멈추는 오류 예방
        if (!isReSaveState)
        {
            StartCoroutine("SaveState", 6);
            isReSaveState = true;
        }

        // 절벽 넘어가기
        chapter[5].GetComponent<Yo_Chapter_5>().isCliffOpen = false;

        // ---표지---
        //  0. 앞표지 사이드 안 보이게, 뒷표지 사이드 보이게
        cover_Side_Front.SetActive(false);
        cover_Side_Back.SetActive(true);
        //  1. 책 사이드 닫히기
        cover_Mid_Back.transform.localRotation = Quaternion.Slerp(cover_Mid_Back.transform.localRotation, Quaternion.Euler(0, 0, 90), Time.deltaTime * speed_Open);
        //  2. 책 표지 닫히기
        book_Back.transform.localRotation = Quaternion.Slerp(book_Back.transform.localRotation, Quaternion.Euler(0, 0, 90), Time.deltaTime * speed_Open);

        // ---이전 챕터 접기---
        //  1. 이전 챕터의 왼쪽 면) 접히는 애니메이션
        map_L[5].transform.localScale = Vector3.Lerp(map_L[5].transform.localScale, new Vector3(1, 0, 1), Time.deltaTime * speed_Open);

        // ---이전 종이 넘기기---
        //  1. 이전 챕터의 오른쪽 면) 넘어가기
        map_R[5].transform.localRotation = Quaternion.Slerp(map_R[5].transform.localRotation, Quaternion.Euler(0, 0, 180), Time.deltaTime * speed_Open);
        //  2. 이전 챕터의 오른쪽 면) 접히는 애니메이션
        map_R[5].transform.localScale = Vector3.Lerp(map_R[5].transform.localScale, new Vector3(1, 0, 1), Time.deltaTime * speed_Open);

        // ---지금 종이 넘기기---
        //  1. 원래 있던 오른쪽 페이지 넘어가기
        page[6].transform.localRotation = Quaternion.Slerp(page[6].transform.localRotation, Quaternion.Euler(0, 0, 180), Time.deltaTime * speed_Open);
        //  2. 원래 있던 오른쪽 페이지) 의 1번면 y축 얇아지기
        page[6].transform.GetChild(0).GetChild(1).transform.localScale = Vector3.Lerp(page[6].transform.GetChild(0).GetChild(1).transform.localScale,
            new Vector3(1, 0.05f, 1), Time.deltaTime * 2.5f);

        // 거의 다 닫혔을 때
        if (book_Back.transform.localRotation.z >= 0.7f)
        {
            //  1. 이전 챕터의 왼쪽 면) 없애기
            map_L[5].SetActive(false);
            //  2. 이전 챕터의 오른쪽 면) 없애기
            map_R[5].SetActive(false);
            //  3. 이전 왼쪽 페이지) 없애기
            page[5].SetActive(false);
            //  4. 이전 오른쪽 페이지) 없애기
            page[6].SetActive(false);

            for (int i = 0; i < 4; i++)
            {
                newPage.transform.GetChild(i).gameObject.SetActive(false);
                newPage.transform.GetChild(i).GetChild(1).localScale = new Vector3(1, 0.05f, 1);
            }
            for (int i = 0; i < page.Length; i++)
            {
                page[i] = pageSave[i];
            }
            // 5장에서 책 덮히고 넘어오느라 달라진 위치각도 초기화
            newPage.transform.localRotation = Quaternion.Euler(0, 0, 0.2f);
            newPage.SetActive(true);
            page[6].transform.localRotation = Quaternion.Euler(0, 0, 0.2f);
            map_L[5].transform.localRotation = Quaternion.Euler(0, 0, -179);
            map_R[5].transform.localRotation = Quaternion.Euler(0, 0, 0.2f);
            chapter[5].SetActive(false);
        }
    }

    //  6. 지금 챕터를 이전 챕터로 저장해두기
    public IEnumerator SaveState(int now)
    {
        yield return new WaitForSeconds(3);
        print(now + "로 저장");
        beforeState = now;

        // 북마크 누를 수 있게
        isBookmark = true;
    }

    // 처음 상태
    public void FirstState()
    {
        audioCtrl.bgmAuto = 0;

        // 처음 책 위치
        transform.localPosition = new Vector3(-4f, 8f, 2);
        transform.localRotation = Quaternion.Euler(-90, 0, 0);

        // 책 사이드 처음 위치
        cover_Mid_Front.transform.localRotation = Quaternion.Euler(0, 0, 0);
        // 책 표지 처음 위치
        cover.transform.localRotation = Quaternion.Euler(0, 0, 0);
        // 앞표지 사이드 보이게, 뒷표지 사이드 안 보이게
        cover_Side_Front.SetActive(true);
        cover_Side_Back.SetActive(false);

        // 페이지 처음 상태
        page[0].SetActive(true);
        page[1].SetActive(true);
        page[1].transform.localRotation = Quaternion.Euler(0, 0, 0.2f);
        page[0].transform.GetChild(1).localScale = new Vector3(1, 0, 1);
        page[1].transform.GetChild(0).GetChild(1).localScale = new Vector3(1, 0, 1);
        page[1].transform.GetChild(0).GetChild(1).localScale = new Vector3(1, 0, 1);
        page_ContentR.transform.GetChild(0).GetChild(1).localScale = new Vector3(1, 0, 1);

        for (int i = 2; i < page.Length; i++)
        {
            page[i].transform.localRotation = Quaternion.Euler(0, 0, 0.2f);
            page[i].transform.GetChild(0).GetChild(1).localScale = new Vector3(1, 0, 1);
            if (i != 6)
                page[i].transform.GetChild(1).GetChild(1).localScale = new Vector3(1, 0, 1);
            page[i].SetActive(false);
        }

        // 맵 처음 상태
        map_L[0].SetActive(false);
        map_R[0].SetActive(false);
        map_L[0].transform.localScale = new Vector3(1, 0, 1);
        map_R[0].transform.localScale = new Vector3(1, 0, 1);

        for (int i = 1; i < map_L.Length; i++)
        {
            map_L[i].transform.localRotation = Quaternion.Euler(0, 0, -179);
            map_L[i].transform.localScale = new Vector3(1, 0, 1);
            map_L[i].SetActive(false);
            map_R[i].transform.localRotation = Quaternion.Euler(0, 0, 0.2f);
            map_R[i].transform.localScale = new Vector3(1, 0, 1);
            map_R[i].SetActive(false);
        }

        // 챕터스크립트 처음 상태
        for (int i = 0; i < chapter.Length; i++)
        {
            chapter[i].SetActive(false);
        }

        // 표지 팝업종이 처음 상태
        bookPaper.transform.localScale = new Vector3(1, 1, 1);

        // 디렉셔널라이트 처음 상태
        //light_main.transform.localRotation = lights[0].transform.localRotation;
        //light_main.GetComponent<Light>().color = lights[0].GetComponent<Light>().color;
        //light_main.GetComponent<Light>().intensity = lights[0].GetComponent<Light>().intensity;

        nowState = 0;
    }

    // 목차 북마크로 이동
    public void GoContent(int before)
    {
        beforeState = before;
        nowState = 11;
    }

    // 다음 챕터로
    public void GoNext(int before)
    {
        beforeState = before;
        nowState = before + 1;
    }

    // 설정 내려오기
    public void GoSetting()
    {
        anim_Setting.SetTrigger("IsDown");
        isSetting = true;
    }

    // 설정 올라가기
    public void GoContinue()
    {
        anim_Setting.SetTrigger("IsUp");
        isSetting = false;
    }
}
