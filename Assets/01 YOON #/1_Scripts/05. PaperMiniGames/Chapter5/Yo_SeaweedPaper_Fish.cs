using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 움직여질 물건(종이)에 넣을 스크립트
public class Yo_SeaweedPaper_Fish : MonoBehaviour
{
    [Header("챕터 5")]
    public GameObject chapter5;
    [Header("자막")]
    public GameObject textUI;
    
    [Header("물고기")]
    public GameObject fish;

    [Header("다른 종이")]
    public GameObject otherPaper;

    [Header("물고기 처음 위치(Z)")]
    public float inter_firsPosZ = 4;

    [Header("물고기 목표 위치(Z)")]
    public float goal_RotX = 1;

    Yo_Chapter_5 CH5;
    Yo_TextCtrl dialog;

    public bool isRight, isEnd, isCanMove;
    private Vector2 curScreenSpace;
    private float firstZ;

    private void Start()
    {
        CH5 = chapter5.GetComponent<Yo_Chapter_5>();
        dialog = textUI.GetComponent<Yo_TextCtrl>();

        firstZ = transform.localRotation.z;
    }

    public void RealStart()
    {
        isCanMove = false;
        isEnd = false;
    }

    void OnMouseDrag()
    {
        if (isCanMove && !CH5.popup.isReset)
        {
            // 이 종이가 오른쪽 종이라면
            if (isRight)
            {
                // 마우스를 오른쪽으로 옮겼다면
                if (Input.mousePosition.x - curScreenSpace.x > 0)
                {
                    // 이 종이는 오른쪽으로 (-0.25f 이하일 경우만)
                    if (transform.localRotation.z >= -0.25f)
                    {
                        transform.Rotate(0, 0, -6);
                    }
                    else
                    {
                        // 자막 나오기 -> 움직일 수는 있으나 자막은 한번밖에 안 나오게
                        if (!isEnd)
                        {
                            InterEnd();
                        }
                    }

                    // 다른 종이는 왼쪽으로 (-0.25f 이하일 경우만)
                    if (otherPaper.transform.localRotation.z <= 0.25f)
                    {
                        otherPaper.transform.Rotate(0, 0, 6);
                    }
                }

                // 마우스를 왼쪽으로 옮겼다면
                else if (Input.mousePosition.x - curScreenSpace.x < 0)
                {
                    // 이 종이는 왼쪽으로 (0.25f 이상일 경우만)
                    if (transform.localRotation.z <= 0f)
                    {
                        transform.Rotate(0, 0, 6);
                    }

                    // 다른 종이는 오른쪽으로 (-0.25f 이상일 경우만)
                    if (otherPaper.transform.localRotation.z >= 0f)
                    {
                        otherPaper.transform.Rotate(0, 0, -6);
                    }
                }

                InterStart_Position_R();
            }

            // 이 종이가 왼쪽 종이라면
            else
            {
                // 마우스를 오른쪽으로 옮겼다면
                if (Input.mousePosition.x - curScreenSpace.x > 0)
                {
                    // 이 종이는 오른쪽으로 (-0.25f 이하일 경우만)
                    if (transform.localRotation.z >= 0)
                    {
                        transform.Rotate(0, 0, -6);
                    }

                    // 다른 종이는 왼쪽으로 (-0.25f 이하일 경우만)
                    if (otherPaper.transform.localRotation.z <= 0)
                    {
                        otherPaper.transform.Rotate(0, 0, 6);
                    }
                }

                // 마우스를 왼쪽으로 옮겼다면
                else if (Input.mousePosition.x - curScreenSpace.x < 0)
                {
                    // 이 종이는 왼쪽으로 (0.25f 이하일 경우만)
                    if (transform.localRotation.z <= 0.25f)
                    {
                        transform.Rotate(0, 0, 6);
                    }
                    else
                    {
                        // 자막 나오기 -> 움직일 수는 있으나 자막은 한번밖에 안 나오게
                        if (!isEnd)
                        {
                            InterEnd();
                        }
                    }

                    // 다른 종이는 오른쪽으로 (-0.25f 이상일 경우만)
                    if (otherPaper.transform.localRotation.z >= -0.25f)
                    {
                        otherPaper.transform.Rotate(0, 0, -6);
                    }
                }

                InterStart_Position_L();
            }

            // 이전 마우스 위치 저장
            curScreenSpace = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
    }

    // 인터렉션 오브젝트 점점 조절
    public void InterStart_Position_R()
    {
        // 0보다 작거나 55보다 크다면 10, 50에 제한두기
        if (inter_firsPosZ - ((firstZ - transform.localRotation.z) / 0.25f) * 3 >= inter_firsPosZ)
        {
            fish.transform.localPosition = new Vector3(-2, 2.3f, inter_firsPosZ);
        }
        else if (inter_firsPosZ - ((firstZ - transform.localRotation.z) / 0.25f) * 3 <= goal_RotX)
        {
            fish.transform.localPosition = new Vector3(-2, 2.3f, goal_RotX);
        }

        // 0보다 크고 55보다 작을때는 종이의 움직임에 따라 움직임
        else
        {
            fish.transform.localPosition = new Vector3(-2, 2.3f, inter_firsPosZ - ((firstZ - transform.localRotation.z) / 0.25f) * 3);
        }
    }

    // 인터렉션 오브젝트 점점 조절 // 3 -> 1
    public void InterStart_Position_L()
    {
        // 0보다 작거나 55보다 크다면 10, 50에 제한두기
        if (inter_firsPosZ - ((transform.localRotation.z - firstZ) / 0.25f) * 3 >= inter_firsPosZ)
        {
            fish.transform.localPosition = new Vector3(-2, 2.3f, inter_firsPosZ);
        }
        else if (inter_firsPosZ - ((transform.localRotation.z - firstZ) / 0.25f) * 3 <= goal_RotX)
        {
            fish.transform.localPosition = new Vector3(-2, 2.3f, goal_RotX);
        }

        // 0보다 크고 55보다 작을때는 종이의 움직임에 따라 움직임
        else
        {
            fish.transform.localPosition = new Vector3(-2, 2.3f, inter_firsPosZ - ((transform.localRotation.z - firstZ) / 0.25f) * 3);
        }
    }

    // 인터렉션 완성된 상태로 바꾸기(고정시키기)
    private void InterEnd()
    {
        // 돌이 다 열렸으니 자막 나오게
        dialog.textNumber = 41;
        dialog.TextOpen(chapter5);

        // 모든 종이들 드래그할 수 없도록
        CH5.PaperDrag(false);

        // 챕터에서 paperCount 감소
        CH5.paperCount--;
        
        // 반복 방지
        otherPaper.GetComponent<Yo_SeaweedPaper_Fish>().isEnd = true;
        isEnd = true;

    }

    public void Drag(bool isDrag)
    {
        isCanMove = isDrag;
    }
}

