using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 상호작용 종이에 들어가는 스크립트
public class Yo_JumpPaper : MonoBehaviour
{
    [Header("상호작용 할 오브젝트")]
    public GameObject inter_Obj;

    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    // 이 종이를 눌렀을 때
    void OnMouseDown()
    {
        // 무조건 누르면 애니메이션
        anim.SetTrigger("Down");
    }

    // 마우스를 놓으면
    private void OnMouseUp()
    {
        // 애니메이션
        anim.SetTrigger("Up");

        inter_Obj.GetComponent<Rigidbody>().AddForce(inter_Obj.transform.forward * 70 + inter_Obj.transform.up * 255);

        // 점프를 한번 하면 바로 미션 켜지도록
        inter_Obj.GetComponent<Yo_Starfish_3>().isMission = true;
    }
}
