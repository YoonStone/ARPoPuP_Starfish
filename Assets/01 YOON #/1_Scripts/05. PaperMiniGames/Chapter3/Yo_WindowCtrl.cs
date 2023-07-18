using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yo_WindowCtrl : MonoBehaviour
{
    bool isRight;
    private Vector2 curScreenSpace, nowScreenSpace;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();

        if (name.Contains("R"))
        {
            isRight = true;
        }
        else
        {
            isRight = false;
        }
    }

    private void OnMouseDown()
    {
        // 이전 마우스 위치 저장
        curScreenSpace = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }

    private void OnMouseUp()
    {
        // 마우스 뗀 위치 저장
        nowScreenSpace = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        // 오른쪽 창문
        if (isRight)
        {
            // 마우스를 오른쪽으로 옮겼다면
            if(nowScreenSpace.x - curScreenSpace.x >= 100)
            {
                anim.SetTrigger("IsOpen");
            }

            // 마우스를 왼쪽으로 옮겼다면
            if (nowScreenSpace.x - curScreenSpace.x <= -100)
            {
                anim.SetTrigger("IsClose");
            }
        }
        // 왼쪽 창문
        else
        {
            // 마우스를 오른쪽으로 옮겼다면
            if (nowScreenSpace.x - curScreenSpace.x >= 100)
            {
                anim.SetTrigger("IsClose");
            }

            // 마우스를 왼쪽으로 옮겼다면
            if (nowScreenSpace.x - curScreenSpace.x <= -100)
            {
                anim.SetTrigger("IsOpen");
            }
        }
    }

    // 드래그하는만큼 열리기
    private void OnMouseDrag()
    {
        //// 오른쪽 창문이라면
        //if (isRight)
        //{
        //    print(Input.mousePosition.x - curScreenSpace.x);
        //    // 마우스를 오른쪽으로 옮겼다면

        //}

        //// 오른쪽 창문이라면
        //if (isRight)
        //{
        //    // 마우스를 오른쪽으로 옮겼다면
        //    if (Input.mousePosition.x - curScreenSpace.x > 0)
        //    {
        //        // 오른쪽으로 (-0.9f 이상일 경우만)
        //        if (transform.localRotation.y > -0.9f)
        //        {
        //            transform.Rotate(0, -4f, 0);
        //        }
        //        else
        //        {
        //            transform.localRotation = Quaternion.Euler(0, -130, 0);
        //        }
        //    }

        //    // 마우스를 왼쪽으로 옮겼다면
        //    else if (Input.mousePosition.x - curScreenSpace.x < 0)
        //    {
        //        // 왼쪽으로 (0 이하일 경우만)
        //        if (transform.localRotation.y < 0f)
        //        {
        //            transform.Rotate(0, 4f, 0);
        //        }
        //        else
        //        {
        //            transform.localRotation = Quaternion.Euler(0, 0, 0);
        //        }
        //    }
        //}

        //// 왼쪽 창문이라면
        //else
        //{
        //    // 마우스를 오른쪽으로 옮겼다면
        //    if (Input.mousePosition.x - curScreenSpace.x > 0)
        //    {
        //        // 오른쪽으로 (0 이상일 경우만)
        //        if (transform.localRotation.y > 0f)
        //        {
        //            transform.Rotate(0, -4f, 0);
        //        }
        //        else
        //        {
        //            transform.localRotation = Quaternion.Euler(0, 0, 0);
        //        }
        //    }

        //    // 마우스를 왼쪽으로 옮겼다면
        //    else if (Input.mousePosition.x - curScreenSpace.x < 0)
        //    {
        //        // 왼쪽으로 (0.9 이하일 경우만)
        //        if (transform.localRotation.y < 0.9f)
        //        {
        //            transform.Rotate(0, 4f, 0);
        //        }
        //        else
        //        {
        //            transform.localRotation = Quaternion.Euler(0, 130, 0);
        //        }
        //    }
        //}

        //// 이전 마우스 위치 저장
        //curScreenSpace = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }
    void Update()
    {
        
    }
}
