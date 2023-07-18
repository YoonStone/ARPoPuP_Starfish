using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 움직여질 물건(종이)에 넣을 스크립트
public class Yo_StonePaper : MonoBehaviour
{
    [Header("챕터 5")]
    public GameObject chapter5;
    [Header("자막")]
    public GameObject textUI;
    [Header("상호작용 할 오브젝트")]
    public GameObject urchin;

    Yo_Chapter_5 CH5;
    Yo_TextCtrl dialog;

    private Vector2 curScreenSpace;
    private float urchin_scale, firstX;
    public bool isEnd, isCanMove;

    private void Start()
    {
        CH5 = chapter5.GetComponent<Yo_Chapter_5>();
        dialog = textUI.GetComponent<Yo_TextCtrl>();

        firstX = transform.localRotation.x;
    }

    public void RealStart()
    {
        isCanMove = false;
        isEnd = false;
    }

    void OnMouseDrag()
    {
        if (isCanMove)
        {
            // 마우스를 위쪽으로 옮겼다면
            if (Input.mousePosition.y - curScreenSpace.y > 0)
            {
                if (transform.localRotation.x <= firstX)
                {
                    transform.Rotate(8f, 0, 0);
                }
            }

            // 마우스를 아래쪽으로 옮겼다면
            else if (Input.mousePosition.y - curScreenSpace.y < 0)
            {
                if (transform.localRotation.x >= -0.995f)
                {
                    transform.Rotate(-8f, 0, 0);
                }
                else
                {
                    // 자막 나오기 -> 움직일 수는 있으나 자막은 한번밖에 안 나오게
                    if (!isEnd)
                    {
                        InterEnd();
                    }
                }
            }

            // 이전 마우스 위치 저장
            curScreenSpace = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
    }

    private void Update()
    {
        urchin.transform.localScale = new Vector3(urchin_scale, urchin_scale, urchin_scale);

        // 0 -> 1
        urchin_scale = (firstX - transform.localRotation.x) / (firstX + 0.995f);
    }

    // 인터렉션 완성된 상태로 바꾸기(고정시키기)
    private void InterEnd()
    {
        // 돌이 다 열렸으니 자막 나오게
        dialog.textNumber = 47;
        dialog.TextOpen(chapter5);

        // 모든 종이들 드래그할 수 없도록
        CH5.PaperDrag(false);

        // 챕터에서 paperCount 감소
        CH5.paperCount--;

        // 반복 방지
        isEnd = true;
    }

    public void Drag(bool isDrag)
    {
        isCanMove = isDrag;
    }
}

