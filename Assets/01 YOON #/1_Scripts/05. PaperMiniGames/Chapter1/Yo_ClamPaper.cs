using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 움직여질 물건(종이)에 넣을 스크립트
public class Yo_ClamPaper : MonoBehaviour
{
    [Header("상호작용 할 오브젝트")]
    public GameObject inter_Obj;

    [Header("조개 처음 각도(X)")]
    public float inter_firstRotX = 10;

    [Header("조개 목표 각도(X)")]
    public float goal_RotX = 45;

    public GameObject starfish;
    public ParticleSystem particle;
    private Vector3 screenSpace;
    private Vector3 offset;
    private float firstZ_Pos;
    private bool isEnd;

    private void Start()
    {
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
                InterStart_Rotation();
            }
        }
    }

    // 인터렉션 오브젝트 점점 조절
    public void InterStart_Rotation()
    {        
        // 0보다 작거나 55보다 크다면 10, 50에 제한두기
        if(inter_firstRotX - (inter_firstRotX - goal_RotX) * (firstZ_Pos - transform.localPosition.z) <= inter_firstRotX)
        {
            inter_Obj.transform.localRotation = Quaternion.Euler(inter_firstRotX, 0, 0);
        }
        else if (inter_firstRotX - (inter_firstRotX - goal_RotX) * (firstZ_Pos - transform.localPosition.z) >= goal_RotX)
        {
            inter_Obj.transform.localRotation = Quaternion.Euler(goal_RotX, 0, 0);
        }

        // 0보다 크고 55보다 작을때는 종이의 움직임에 따라 움직임
        else
        {
            inter_Obj.transform.localRotation = Quaternion.Euler(inter_firstRotX - (inter_firstRotX - goal_RotX) * (firstZ_Pos - transform.localPosition.z), 0, 0);
        }
    }

    // 인터렉션 완성된 상태로 바꾸기(고정시키기)
    private void InterEnd()
    {
        inter_Obj.transform.localRotation = Quaternion.Euler(goal_RotX, 0, 0);

        // 조개 다 펴졌으니, 불가사리 이동하라고 전달
        starfish.GetComponent<Yo_Starfish_1>().StartCoroutine("ClamWow", particle);

        // 반복 방지
        isEnd = false;
    }
}
