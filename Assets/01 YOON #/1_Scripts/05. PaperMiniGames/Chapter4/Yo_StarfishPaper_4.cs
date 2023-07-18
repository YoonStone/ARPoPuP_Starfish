using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// 움직여질 물건(종이)에 넣을 스크립트
public class Yo_StarfishPaper_4 : MonoBehaviour
{
    [Header("상호작용 할 오브젝트")]
    public GameObject inter_Obj;

    [Header("불가사리 처음 위치(X)")]
    public float inter_firstPosX = -7.2f;

    [Header("불가사리 목표 위치(X)")]
    public float goal_PosX = 0.4f;

    public GameObject chapter4;
    private Vector3 screenSpace;
    private Vector3 offset;
    private float firstX_Pos, lastX_Pos;

    [HideInInspector]
    public bool isStarfishTurn, isEnd;

    Yo_Chapter_4 CH4;

    private void Start()
    {
        CH4 = chapter4.GetComponent<Yo_Chapter_4>();

        // 종이의 원래 X 위치값 저장
        firstX_Pos = transform.localPosition.x;

        lastX_Pos = transform.localPosition.x + 7.6f;
    }

    void Update()
    {
        // 종이 제한
        transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, firstX_Pos, lastX_Pos),
            transform.localPosition.y, transform.localPosition.z);

        // 불가사리 제한
        inter_Obj.transform.localPosition = new Vector3(Mathf.Clamp(inter_Obj.transform.localPosition.x, firstX_Pos, lastX_Pos),
            inter_Obj.transform.localPosition.y, inter_Obj.transform.localPosition.z);
        // 불가사리 앞에보기
        if (isStarfishTurn)
        {
            inter_Obj.transform.localRotation = Quaternion.Slerp(inter_Obj.transform.localRotation, Quaternion.Euler(0, 180, 0), Time.deltaTime * 4);
            inter_Obj.transform.localPosition = Vector3.Lerp(inter_Obj.transform.localPosition, new Vector3(0.4f, 0, -4.15f), Time.deltaTime * 4);
        }
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
            // 마우스 위치 저장(계속)
            Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);

            // 저장된 마우스위치를 월드좌표로 바꾸고 + offset 
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;

            // x축으로만 이동
            transform.position = new Vector3(curPosition.x, transform.position.y, transform.position.z);
            inter_Obj.transform.localPosition = new Vector3(transform.localPosition.x, inter_Obj.transform.localPosition.y, inter_Obj.transform.localPosition.z);

            // 이동하는 동안에 애니메이션 켜기
            inter_Obj.GetComponent<Animator>().SetBool("IsWalk", true);

            // 종이가 오른쪽끝까지 다 나오면
            if (transform.localPosition.x >= lastX_Pos)
            {
                if (CH4.trashChangeCount == 3)
                {
                    InterEnd();
                }
            }
        }
    }

    private void OnMouseUp()
    {
        // 이동 안 하는 동안에 애니메이션 끄기
        inter_Obj.GetComponent<Animator>().SetBool("IsWalk", false);
    }

    // 인터렉션 완성된 상태로 바꾸기(고정시키기)
    private void InterEnd()
    {
        // 불가사리 앞에 보고 있기
        isStarfishTurn = true;

        // 불가사리 미션 시작
        CH4.StartCoroutine("PaperToDialog", 0);

        // 반복 방지
        isEnd = false;
    }
}
