using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// 움직여질 물건(종이)에 넣을 스크립트
public class Yo_RopeCtrl : MonoBehaviour
{
    [Header("밧줄 올라가는 속도")]
    public float speed_RopeUp;

    [HideInInspector]
    public bool isUp, isCanUp;
    private Vector2 curScreenSpace;

    private void Update()
    {
        // 밧줄이 서서히 올라가도록
        if (isUp)
        {
            transform.Translate(0, speed_RopeUp * Time.deltaTime, 0);

            if (transform.localPosition.y >= 0.15f)
            {
                isUp = false;
                isCanUp = false;
            }
        }
    }

    void OnMouseDrag()
    {
        if (!isUp)
        {
            // 마우스를 아래쪽으로 옮겼다면
            if (Input.mousePosition.y - curScreenSpace.y < -5)
            {
                transform.Translate(0, -0.05f, 0);

                isCanUp = true;
            }

            // 이전 마우스 위치 저장
            curScreenSpace = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
    }

    // 마우스를 놓으면 올라가도록
    public void OnMouseUp()
    {
        if (!isUp && isCanUp)
        {
            isUp = true;

            // 별 내려오고
            GameObject.FindWithTag("Star").GetComponent<Animator>().enabled = true;

            // 불가사리 움직임 시작
            GameObject.FindWithTag("GameController").GetComponent<Yo_Chapter_2>().StartCoroutine("StarfishStart");
        }
    }
}
