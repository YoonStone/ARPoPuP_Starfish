using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BansheeGz.BGSpline.Curve;

public class Yo_TrashPaper : MonoBehaviour
{
    [Header("상호작용 할 오브젝트")]
    public GameObject inter_Obj;

    [Header("불가사리")]
    public GameObject starfish;

    [Header("챕터4")]
    public GameObject chapter4;

    [Header("쓰레기통들")]
    public GameObject[] trashes;

    [Header("불가사리 베지어커브 (F ->)")]
    public GameObject[] starfish_BG_F;

    [Header("불가사리 베지어커브 (1 ->)")]
    public GameObject[] starfish_BG_1;

    [Header("불가사리 베지어커브 (2 ->)")]
    public GameObject[] starfish_BG_2;

    [Header("불가사리 베지어커브 (3 ->)")]
    public GameObject[] starfish_BG_3;

    [Header("불가사리 팝업종이")]
    public GameObject starfish_Paper;

    public int trashNumber;
    public float _time;

    GameObject tempBG;
    Animator starfish_anim, trashCover_anim;
    Yo_StarfishPaper_4 starfishPaper_script;
    Yo_Chapter_4 CH4;

    private void Start()
    {
        starfish_anim = starfish.GetComponent<Animator>();
        trashCover_anim = inter_Obj.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Animator>();
        starfishPaper_script = starfish_Paper.GetComponent<Yo_StarfishPaper_4>();
        CH4 = chapter4.GetComponent<Yo_Chapter_4>();
    }

    // 이 종이를 눌렀을 때
    void OnMouseDown()
    {
        // 눌러지는 애니메이션
        GetComponent<Animator>().SetTrigger("Down");

        starfishPaper_script.isStarfishTurn = false;

        // 제자리에 있으면 아무 일도 X
        if (CH4.nowStarfishState != trashNumber)
        {
            // 이전 쓰레기통 뚜껑 닫기
            if(CH4.nowStarfishState != 0)
                trashes[CH4.nowStarfishState - 1].transform.GetChild(0).GetChild(0).gameObject.GetComponent<Animator>().SetBool("IsOpen", false);

            switch (CH4.nowStarfishState)
            {
                case 0:
                    switch (trashNumber)
                    {
                        // F -> 1
                        case 1: 
                            tempBG = Instantiate(starfish_BG_F[0], starfish_BG_F[0].transform.parent);
                            tempBG.SetActive(true);
                            Invoke("BGOff", _time);
                            break;
                        // F -> 2
                        case 2:
                            tempBG = Instantiate(starfish_BG_F[1], starfish_BG_F[1].transform.parent);
                            tempBG.SetActive(true);
                            Invoke("BGOff", _time);
                            break;
                        // F -> 3
                        case 3:
                            tempBG = Instantiate(starfish_BG_F[2], starfish_BG_F[2].transform.parent);
                            tempBG.SetActive(true);
                            Invoke("BGOff", _time);
                            break;
                    }
                    break;

                case 1:
                    // 1 -> 2
                    if (trashNumber == 2)
                    {
                        tempBG = Instantiate(starfish_BG_1[0], starfish_BG_1[0].transform.parent);
                        tempBG.SetActive(true);
                        Invoke("BGOff", _time);
                    }
                    // 1 -> 3
                    else
                    {
                        tempBG = Instantiate(starfish_BG_1[1], starfish_BG_1[1].transform.parent);
                        tempBG.SetActive(true);
                        Invoke("BGOff", _time);
                    }
                    break;
                case 2:
                    // 2 -> 1
                    if (trashNumber == 1)
                    {
                        tempBG = Instantiate(starfish_BG_2[0], starfish_BG_2[0].transform.parent);
                        tempBG.SetActive(true);
                        Invoke("BGOff", _time);
                    }
                    // 2 -> 3
                    else
                    {
                        tempBG = Instantiate(starfish_BG_2[1], starfish_BG_2[1].transform.parent);
                        tempBG.SetActive(true);
                        Invoke("BGOff", _time);
                    }
                    break;

                case 3:
                    // 3 -> 1
                    if (trashNumber == 1)
                    {
                        tempBG = Instantiate(starfish_BG_3[0], starfish_BG_3[0].transform.parent);
                        tempBG.SetActive(true);
                        Invoke("BGOff", _time);
                    }
                    // 3 -> 2
                    else
                    {
                        tempBG = Instantiate(starfish_BG_3[1], starfish_BG_3[1].transform.parent);
                        tempBG.SetActive(true);
                        Invoke("BGOff", _time);
                    }
                    break;
            }

            StartCoroutine("StarfishRot");

            CH4.nowStarfishState = trashNumber;
        }
    }

    public void BGOff()
    {
        tempBG.SetActive(false);
        Destroy(tempBG);
    }


    // 불가사리 누르는 애니메이션
    public IEnumerator StarfishRot()
    {
        yield return new WaitForSeconds(0.3f);
        if (!CH4.popup.isReset)
        {
            trashCover_anim.SetBool("IsOpen", true);
            starfish_anim.SetTrigger("IsClick");
        }
    }

    // 마우스를 놓으면
    private void OnMouseUp()
    {
        // 떼지는 애니메이션
        GetComponent<Animator>().SetTrigger("Up");
    }

}
