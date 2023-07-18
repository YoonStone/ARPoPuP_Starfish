using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 상호작용 종이에 들어가는 스크립트
public class Yo_StarLightPaper : MonoBehaviour
{
    [Header("상호작용 할 오브젝트")]
    public GameObject inter_Obj;

    public Animator starLight;

    // 이 종이를 눌렀을 때
    void OnMouseDown()
    {
        // 눌러지는 애니메이션
        GetComponent<Animator>().SetTrigger("Down");
    }

    // 마우스를 놓으면
    private void OnMouseUp()
    {
        // 눌러지는 애니메이션
        GetComponent<Animator>().SetTrigger("Up");

        // 별전구 화내기
        starLight.enabled = true;

        Invoke("StarfishJump", 0.5f);
    }

    public void StarfishJump()
    {
        // 점프 (선물 위에서 떨어지게)
        inter_Obj.GetComponent<Rigidbody>().AddForce(Vector3.back * 100 + Vector3.up * 255);

        // 떨어질 때 불가사리 표정 변화
        inter_Obj.GetComponentInChildren<Renderer>().material = Resources.Load("Starfish_Material/Starfish_sad") as Material;

        // 떨어지고 나서 엔딩 진행
        inter_Obj.GetComponent<Yo_Starfish_3>().isStarfishArrive = false;
        inter_Obj.GetComponent<Yo_Starfish_3>().CH3.StartCoroutine("JumpEnd");
    }
}
