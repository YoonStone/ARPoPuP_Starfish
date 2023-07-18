using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yo_StarPaper : MonoBehaviour
{
    public GameObject starMission;
    public bool isCanTouch;
    string[] nameSplit;
    int starNum;

    Yo_StarMissionCtrl starMission_script;
    Animator anim;

    private void Start()
    {
        starMission_script = starMission.GetComponent<Yo_StarMissionCtrl>();
        anim = GetComponent<Animator>();

        // 이름에서 번호만 따서 저장
        nameSplit = transform.gameObject.name.Split('_');
        starNum = int.Parse(nameSplit[2]);
    }

    private void OnMouseDown()
    {
        // 무조건 누르면 애니메이션
        anim.SetTrigger("Down");

        // 이 건반의 번호 알려주기 +지금 누른 별 색깔 반짝
        starMission_script.Shining(starNum);
    }

    private void OnMouseUp()
    {
        // 애니메이션
        anim.SetTrigger("Up");
    }
}
