using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
// 움직여질 물건(종이)에 넣을 스크립트

public class Yo_CoverPaper : MonoBehaviour
{
    private Vector3 screenSpace;
    private Vector3 offset;
    private float firstX_Pos, LastX_Pos;
    private bool isEnd;
    public bool isStartPaper;

    public GameObject ui, arrow1, arrow2;
    public float uiOff_time, arrowChange_time;

    private void Start()
    {
        // 종이의 원래 X 위치값 저장
        firstX_Pos = transform.localPosition.x;

        // 종이의 최대 X 위치값 저장
        LastX_Pos = transform.localPosition.x + 1.8f;

        if (isStartPaper)
            StartCoroutine("UIOff");
    }

    public IEnumerator UIOff()
    {
        yield return new WaitForSeconds(uiOff_time);
        ui.GetComponent<Animator>().enabled = true;

        yield return new WaitForSeconds(1);
        arrow1.SetActive(true);

        yield return new WaitForSeconds(arrowChange_time);
        arrow1.SetActive(false);
        arrow2.SetActive(true);
    }

    void Update()
    {
        // 종이 제한
        transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, firstX_Pos, LastX_Pos),
            transform.localPosition.y, transform.localPosition.z);
    }

    // 이 종이를 눌렀을 때
    void OnMouseDown()
    { 
        // 움직일 물건의 위치를 스크린좌표로 저장
        screenSpace = Camera.main.WorldToScreenPoint(transform.position);

        // 마우스와 움직일 물건 사이의 거리 저장
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));
    }

    void OnMouseDrag()
    {
        if (!isEnd)
        {
            // 종이가 오른쪽끝까지 다 나오면 방향 바꾸기
            if (transform.localPosition.x >= LastX_Pos)
            {
                // 인터렉션 완성된 상태로 바꾸기(고정시키기)
                InterEnd();
            }
            else
            {
                // 마우스 위치 저장(계속)
                Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);

                // 저장된 마우스위치를 월드좌표로 바꾸고 + offset 
                Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;


                // x축으로만 이동
                transform.position = new Vector3(curPosition.x, transform.position.y, transform.position.z);
            }
        }
    }

    // 인터렉션 완성된 상태로 바꾸기(고정시키기)
    private void InterEnd()
    {
        if (isStartPaper)
        {
            // 게임 시작 (씬전환)
            SceneManager.LoadScene(1);
        }
        else
        {
            // 게임 끝
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
         Application.OpenURL(webplayerQuitURL);
#else
         Application.Quit();
#endif  
        }

        // 반복 방지
        isEnd = false;
    }

    // 뒤로가기누르면 꺼지기
    public void BackKeyCtrl()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
         Application.OpenURL(webplayerQuitURL);
#else
         Application.Quit();
#endif 
            }
        }
    }
}
