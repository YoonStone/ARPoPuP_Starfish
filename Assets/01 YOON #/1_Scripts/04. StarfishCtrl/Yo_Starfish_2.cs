using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yo_Starfish_2 : MonoBehaviour
{
    public GameObject chapter2;

    [Header("베지어커브 (불가사리)")]
    public GameObject bgCurve_Starfish2;

    Animator anim;
    Yo_Chapter_2 CH2;
    GameObject tempBG;

    public bool isLadderTop, isStarfishTurn;

    private void Start()
    {
        anim = GetComponent<Animator>();
        CH2 = chapter2.GetComponent<Yo_Chapter_2>();
    }

    // 불가사리 진짜 시작
    public void RealStart()
    {
        // 불가사리 애니메이션 초기화
        anim.SetBool("IsWalk", false);
    }

    // 사다리 앞까지 이동
    public void Move()
    {
        tempBG = Instantiate(bgCurve_Starfish2.transform.GetChild(0).gameObject, bgCurve_Starfish2.transform.GetChild(0).gameObject.transform.parent);
        tempBG.SetActive(true);

        anim.SetBool("IsWalk", true);

        Invoke("Stop", 6f);
    }

    // 사다리 위로 이동
    public void LadderUp()
    {
        tempBG = Instantiate(bgCurve_Starfish2.transform.GetChild(1).gameObject, bgCurve_Starfish2.transform.GetChild(1).gameObject.transform.parent);
        tempBG.SetActive(true);

        anim.SetBool("IsWalk", true);

        CH2.isLadderPaper = true; 
        isLadderTop = true;
        Invoke("Stop", 3f);
    }

    // 멈추기
    public void Stop()
    {
        anim.SetBool("IsWalk", false);
        Destroy(tempBG);

        // 불가사리가 사다리 위에 도착하면
        if (isLadderTop)
        {
            // 글씨 적히도록
            GameObject.FindWithTag("GameController").GetComponent<Yo_Chapter_2>().ShowDialog(14);
            isStarfishTurn = true;
            isLadderTop = false;
        }
    }

    private void Update()
    {
        // 불가사리 앞에보기
        if (isStarfishTurn)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, -180, 0), Time.deltaTime * 5);
        }
    }
}
