using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 움직여질 물건(종이)에 넣을 스크립트
public class Yo_RotationPaper : MonoBehaviour
{
    [Header("상호작용 할 오브젝트")]
    public GameObject inter_Obj;

    public float speed_Back = 5;

    private Vector2 curScreenSpace;
    private bool isBack, isThisClick;

    void Update()
    {
        // 이 종이를 드래그하다가 마우스를 떼면
        if (Input.GetMouseButtonUp(0) && isThisClick)
        {
            isBack = true;
            isThisClick = false;
        }

        // 원상태로 돌아가기
        if (isBack)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * speed_Back);
        }
    }

    // 이 종이를 눌렀을 때
    void OnMouseDown()
    {
        isThisClick = true;
        isBack = false;
    }

    void OnMouseDrag()
    {
        // 마우스를 오른쪽으로 옮겼다면
        if (Input.mousePosition.x - curScreenSpace.x > 0)
        {
            transform.Rotate(0, -2, 0);
            inter_Obj.transform.Rotate(0, -2f, 0);
        }

        // 마우스를 왼쪽으로 옮겼다면
        else if (Input.mousePosition.x - curScreenSpace.x < 0)
        {
            transform.Rotate(0, 2f, 0);
            inter_Obj.transform.Rotate(0, 2f, 0);
        }

        // 이전 마우스 위치 저장
        curScreenSpace = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }
}

