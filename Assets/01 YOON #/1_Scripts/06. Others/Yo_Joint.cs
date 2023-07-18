using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 움직여질 물건(종이)에 넣을 스크립트
public class Yo_Joint : MonoBehaviour
{
    private Vector2 screenSpace;
    private Vector3 firstPressPos, secondPressPos;

    private float swipeDir;
    private bool isTouch;

    // 이 글자를 눌렀을 때
    void OnMouseDown()
    {
        firstPressPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
    }

    // 드래그하는 중
    void OnMouseDrag()
    {
        isTouch = true;
    }

    // 마우스를 떼면 끝
    private void OnMouseUp()
    {
        isTouch = false;
    }

    private void Update()
    {

        if (isTouch)
        {
            secondPressPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
            swipeDir = firstPressPos.x - secondPressPos.x;
            
            // 오른쪽으로 스와이프
            if (swipeDir < 0)
            {
                GetComponent<Rigidbody>().AddForce(Vector3.right * 100);
            }
            // 왼쪽으로 스와이프
            else if (swipeDir > 0)
            {
                GetComponent<Rigidbody>().AddForce(Vector3.left * 100);
            }

            firstPressPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
        }

    }
}
