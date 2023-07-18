using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 움직여질 물건(종이)에 넣을 스크립트
public class Yo_ExpressionPaper : MonoBehaviour
{
    [Header("상호작용 할 오브젝트 (불가사리 표정)")]
    public GameObject inter_Obj_Ex;

    [Header("표정 처음 위치(Z)")]
    public float inter_firstPosZ = 0;

    [Header("표정 목표 위치(Z)")]
    public float goal_PosZ = -2;

    [Header("상호작용 할 오브젝트 (물고기들)")]
    public GameObject inter_Obj_Fishes;

    [Header("물고기 처음 각도(Y)")]
    public float inter_firstRotY = 0;

    [Header("물고기 목표 각도(Y)")]
    public float inter_GoalRotY = 180;

    public GameObject popup, swipeAnim;
    private Vector3 screenSpace;
    private Vector3 offset;
    private float firstZ_Pos;
    public Text t;

    float swipeDir, minSwipe, firstPressPosM, secondPressPosM;
    Vector3 firstPressPos, secondPressPos;
    bool isCanSwipe, isTouch;

    private void Start()
    {
        // 종이의 원래 z 위치값 저장 // 0
        firstZ_Pos = transform.localPosition.z;

        // 스와이프 가능 최소값
        minSwipe = Screen.width / 4;
    }

    void Update()
    {
        // 종이를 2만큼만 당길 수 있도록 제한 (0 -> -2)
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y,
            Mathf.Clamp(transform.localPosition.z, firstZ_Pos - 2f, firstZ_Pos));

        if (isCanSwipe)
        {
            CanSwipe();
        }
    }

    // 이 종이를 눌렀을 때
    void OnMouseDown()
    {
        // 움직일 물건의 위치를 스크린좌표로 저장
        screenSpace = Camera.main.WorldToScreenPoint(transform.position);

        // 마우스와 움직일 물건 사이의 거리 저장
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));

        // 하이라이트 없애기
        transform.GetChild(0).gameObject.SetActive(false);
    }

    void OnMouseDrag()
    {
        // 마우스 위치 저장(계속)
        Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);

        // 저장된 마우스위치를 월드좌표로 바꾸고 + offset 
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;


        // z축으로만 이동
        transform.position = new Vector3(transform.position.x, curPosition.y, transform.position.z);

        // 인터렉션 오브젝트 조절
        InterStart_Position();
        InterStart_Rotation();

        // 종이가 끝까지 다 나오면(상호작용이 완료되면)
        if (transform.localPosition.z <= firstZ_Pos - 2f)
        {
            // 스와이프 가능
            isCanSwipe = true;
            // 스와이프 이미지 o
            swipeAnim.SetActive(true);
        }
        else
        {
            // 스와이프 불가능
            isCanSwipe = false;
            // 스와이프 이미지 x
            swipeAnim.SetActive(false);

        }
    }

    // 표정 점점 조절
    public void InterStart_Position()
    {
        // 목표 위치 이상이면
        if (inter_firstPosZ - (inter_firstPosZ - goal_PosZ) * (firstZ_Pos - transform.localPosition.z) / 2f <= goal_PosZ)
        {
            inter_Obj_Ex.transform.localPosition = new Vector3(inter_Obj_Ex.transform.localPosition.x, inter_Obj_Ex.transform.localPosition.y, goal_PosZ);
        }
        // 처음 위치 이하면
        else if (inter_firstPosZ - (inter_firstPosZ - goal_PosZ) * (firstZ_Pos - transform.localPosition.z) / 2f >= inter_firstPosZ)
        {
            inter_Obj_Ex.transform.localPosition = new Vector3(inter_Obj_Ex.transform.localPosition.x, inter_Obj_Ex.transform.localPosition.y, inter_firstPosZ);
        }

        // 이 사이라면 움직이는대로 움직이기
        else
        {
            inter_Obj_Ex.transform.localPosition = new Vector3(inter_Obj_Ex.transform.localPosition.x, inter_Obj_Ex.transform.localPosition.y,
                inter_firstPosZ - (inter_firstPosZ - goal_PosZ) * (firstZ_Pos - transform.localPosition.z) / 2f);
        }
    }

    // 물고기 점점 조절
    public void InterStart_Rotation()
    {
        for (int i = 0; i < inter_Obj_Fishes.transform.childCount; i++)
        {
            // 목표 각도 이상이면
            if(inter_firstRotY - (inter_firstRotY - (inter_GoalRotY)) * (firstZ_Pos - transform.localPosition.z) / 2f >= inter_GoalRotY)
            {
                inter_Obj_Fishes.transform.GetChild(i).localRotation = Quaternion.Euler(0, inter_GoalRotY, 0);
            }
            // 처음 각도 이하면
            else if (inter_firstRotY - (inter_firstRotY - (inter_GoalRotY)) * (firstZ_Pos - transform.localPosition.z) / 2f <= inter_firstRotY)
            {
                inter_Obj_Fishes.transform.GetChild(i).localRotation = Quaternion.Euler(0, inter_firstRotY, 0);
            }

            // 이 사이라면 움직이는대로 움직이기
            else
            {
                inter_Obj_Fishes.transform.GetChild(i).localRotation = Quaternion.Euler(0,
                    inter_firstRotY - (inter_firstRotY - (inter_GoalRotY)) * (firstZ_Pos - transform.localPosition.z) / 2f, 0);
            }
        }
    }


    // 스와이프로 책 넘길 수 있음
    private void CanSwipe()
    {
        // 스와이프 모바일버전
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // 첫번째 터치 위치
                    firstPressPos = touch.position;
                    break;

                case TouchPhase.Moved:
                    // 움직이고 있어야 함
                    isTouch = true;
                    break;

                case TouchPhase.Ended:
                    // 떼는 순간 두번째 터치 위치
                    secondPressPos = touch.position;
                    swipeDir = firstPressPos.x - secondPressPos.x;

                    if (swipeDir > minSwipe && isTouch)
                    {
                        GameStart();
                    }
                    break;                    
            }
        }

        // 스와이프 마우스 버전
        if (Input.GetMouseButtonDown(0))
        {
            // 첫번째 터치 위치
            firstPressPosM = Input.mousePosition.x;
        }
        if (Input.GetMouseButtonUp(0))
        {
            // 떼는 순간 두번째 터치 위치
            secondPressPosM = Input.mousePosition.x;
            swipeDir = firstPressPosM - secondPressPosM;

            if (swipeDir > minSwipe)
            {
                GameStart();
            }
        }

    }

    // 표정이 다 바뀌었으니 책 펴지라고 전달
    public void GameStart()
    {
        // 스와이프 불가능
        isCanSwipe = false;
       
        // 스와이프 이미지 x
        swipeAnim.SetActive(false);

        popup.GetComponent<Yo_PopupCtrl>().nowState = 10;
    }
}
