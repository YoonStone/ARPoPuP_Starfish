using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// 움직여질 물건(종이)에 넣을 스크립트
public class Yo_FishesPaper : MonoBehaviour
{
    [Header("상호작용 할 오브젝트")]
    public GameObject inter_Obj;

    [Header("물고기들 처음 위치(X)")]
    public float inter_firstPosX = -9.5f;

    [Header("물고기들 목표 위치(X)")]
    public float goal_PosX = 18;

    public GameObject starfish;
    private Vector3 screenSpace;
    private Vector3 offset;
    private float firstX_Pos;
    private bool isEnd, isLeft;

    private void Start()
    {
        // 종이의 원래 X 위치값 저장
        firstX_Pos = transform.localPosition.x;
    }

    void Update()
    {
        // 종이 제한
        transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, firstX_Pos, firstX_Pos + 12f),
            transform.localPosition.y, transform.localPosition.z);

        if (isLeft)
        {
            // 종이 바꾸기
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            // 종이 바꾸기
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    // 이 종이를 눌렀을 때
    void OnMouseDown()
    {
        // 물고기들 보이게
        inter_Obj.SetActive(true);

        // 움직일 물건의 위치를 스크린좌표로 저장
        screenSpace = Camera.main.WorldToScreenPoint(transform.position);

        // 마우스와 움직일 물건 사이의 거리 저장
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));
    }

    void OnMouseDrag()
    {
        if (!isEnd)
        {
            // 종이를 오른쪽으로 밀어야하는 상태
            if (!isLeft)
            {
                // 종이가 오른쪽끝까지 다 나오면 방향 바꾸기
                if (transform.localPosition.x >= firstX_Pos + 12f)
                {
                    // 인터렉션 완성된 상태로 바꾸기(고정시키기)
                    inter_Obj.transform.localPosition = new Vector3(goal_PosX, 4.8f, -3);
                    inter_Obj.transform.localRotation = Quaternion.Euler(0, -180, 0);

                    // 왼쪽으로 밀어야하는 상태로 바꾸기
                    isLeft = true;
                }
                else
                {
                    // 마우스 위치 저장(계속)
                    Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);

                    // 저장된 마우스위치를 월드좌표로 바꾸고 + offset 
                    Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;


                    // x축으로만 이동
                    transform.position = new Vector3(curPosition.x, transform.position.y, transform.position.z);

                    // 인터렉션 오브젝트 조절
                    InterStart_Position_Right();
                }
            }
            // 종이를 왼쪽으로 밀어야하는 상태
            else
            {
                // 종이가 왼쪽 끝까지 다 나오면(상호작용이 완료되면) 안 움직이도록
                if (transform.localPosition.x <= firstX_Pos)
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

                    // 인터렉션 오브젝트 조절
                    InterStart_Position_Left();
                }
            }
        }
    }

    // 왼쪽으로 이동해야 할 때
    public void InterStart_Position_Left()
    {

        // -15보다 작거나 -7.5보다 크다면 -15, -7.5에 제한두기
        if (goal_PosX - (goal_PosX - inter_firstPosX + 8.5f) * ((firstX_Pos + 12f - transform.localPosition.x) / 12f) <= inter_firstPosX - 7)
        {
            inter_Obj.transform.localPosition = new Vector3(inter_firstPosX - 7, 4.37f, -2.34f);
        }
        else if (goal_PosX - (goal_PosX - inter_firstPosX + 8.5f) * ((firstX_Pos + 12f - transform.localPosition.x) / 12f) >= goal_PosX)
        {
            inter_Obj.transform.localPosition = new Vector3(goal_PosX, 4.37f, -2.34f);
        }

        // -15보다 크고 -7.5보다 작을때는 종이의 움직임에 따라 움직임
        else
        {
            inter_Obj.transform.localPosition = new Vector3(goal_PosX - (goal_PosX - inter_firstPosX + 8.5f) * ((firstX_Pos + 12f - transform.localPosition.x) / 12f),
                inter_Obj.transform.localPosition.y, inter_Obj.transform.localPosition.z);
        }
    }

    // 오른쪽으로 이동해야 할 때
    public void InterStart_Position_Right()
    {        
        // -15보다 작거나 -7.5보다 크다면 -15, -7.5에 제한두기
        if(inter_firstPosX + (goal_PosX - inter_firstPosX) * ((transform.localPosition.x - firstX_Pos) / 12f)  <= inter_firstPosX)
        {
            inter_Obj.transform.localPosition = new Vector3(inter_firstPosX, 4.37f, -2.34f);
        }
        else if (inter_firstPosX + (goal_PosX - inter_firstPosX) * ((transform.localPosition.x - firstX_Pos) / 12f) >= goal_PosX)
        {
            inter_Obj.transform.localPosition = new Vector3(goal_PosX, 4.37f, -2.34f);
        }

        // -15보다 크고 -7.5보다 작을때는 종이의 움직임에 따라 움직임
        else
        {
            inter_Obj.transform.localPosition = new Vector3(inter_firstPosX + (goal_PosX - inter_firstPosX) * ((transform.localPosition.x - firstX_Pos) / 12f),
                inter_Obj.transform.localPosition.y, inter_Obj.transform.localPosition.z);
        }
    }


    // 인터렉션 완성된 상태로 바꾸기(고정시키기)
    private void InterEnd()
    {
        inter_Obj.transform.localPosition = new Vector3(inter_firstPosX, 4.8f, -3f);
        inter_Obj.transform.localRotation = Quaternion.Euler(0, 0, 0);

        // 물고기가 불가사리 밀치라고 전달
        starfish.GetComponent<Yo_Starfish_1>().StartCoroutine("BadFishes");

        // 왼쪽으로 밀어야하는 상태로 바꾸기
        isLeft = false;

        // 반복 방지
        isEnd = false;
    }
}
