using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yo_StarfishPaper_5 : MonoBehaviour
{
    [Header("상호작용 할 오브젝트")]
    public GameObject inter_Obj;

    [Header("불가사리 처음 크기")]
    public float inter_Scale_min = 0.5f;

    [Header("불가사리 목표 크기")]
    public float inter_Scale_max = 0.8f;

    public GameObject chapter5;
    private Vector3 screenSpace;
    private Vector3 offset;
    private float firstZ_Pos, inter_Scale;
    private bool isEnd;

    Yo_Chapter_5 CH5;

    private void Start()
    {
        CH5 = chapter5.GetComponent<Yo_Chapter_5>();

        // 종이의 원래 z 위치값 저장
        firstZ_Pos = transform.localPosition.z;
    }

    void Update()
    {
        // 종이를 1만큼만 당길 수 있도록 제한
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y,
            Mathf.Clamp(transform.localPosition.z, firstZ_Pos - 1f, firstZ_Pos));
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
            // 종이가 끝까지 다 나오면(상호작용이 완료되면) 안 움직이도록
            if (transform.localPosition.z <= firstZ_Pos - 1f)
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

                // z축으로만 이동
                transform.position = new Vector3(transform.position.x, transform.position.y, curPosition.z);

                // 인터렉션 오브젝트 조절
                InterStart_Scale();
            }
        }
    }

    // 인터렉션 오브젝트 점점 조절
    public void InterStart_Scale()
    {
        // 0보다 작거나 55보다 크다면 10, 50에 제한두기
        if (inter_Scale_min + (inter_Scale_max - inter_Scale_min) * (firstZ_Pos - transform.localPosition.z) <= inter_Scale_min)
        {
            inter_Obj.transform.localScale = new Vector3(inter_Scale_min, inter_Scale_min, inter_Scale_min);
        }
        else if (inter_Scale_min + (inter_Scale_max - inter_Scale_min) * (firstZ_Pos - transform.localPosition.z) >= inter_Scale_max)
        {
            inter_Obj.transform.localScale = new Vector3(inter_Scale_max, inter_Scale_max, inter_Scale_max);
        }

        // 0보다 크고 55보다 작을때는 종이의 움직임에 따라 움직임
        else
        {
            inter_Obj.transform.localScale = new Vector3(inter_Scale_min + (inter_Scale_max - inter_Scale_min) * (firstZ_Pos - transform.localPosition.z),
                inter_Scale_min + (inter_Scale_max - inter_Scale_min) * (firstZ_Pos - transform.localPosition.z),
                inter_Scale_min + (inter_Scale_max - inter_Scale_min) * (firstZ_Pos - transform.localPosition.z));
        }
    }

    // 인터렉션 완성된 상태로 바꾸기(고정시키기)
    private void InterEnd()
    {
        inter_Obj.transform.localScale = new Vector3(inter_Scale_max, inter_Scale_max, inter_Scale_max);

        // 불가사리 다 커졌으니 불가사리 폴짝
        CH5.StartCoroutine("StarfishJump");

        // 반복 방지
        isEnd = false;
    }

}
