using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 불가사리 안에 들어있는 스크립트
public class Yo_Starfish_1 : MonoBehaviour
{
    [Header("챕터 1")]
    public GameObject chapter1;
    public GameObject popupMng;

    [Header("물고기들")]
    public GameObject fishes;
    [Header("성게")]
    public GameObject seaUrchin;
    [Header("거북이")]
    public GameObject turtle;
    
    [Header("가자미 미션")]
    public GameObject flatfishMission;

    [Header("베지어커브 (불가사리)")]
    public GameObject bgCurve_Starfish1;
    [Header("베지어커브 (가자미)")]
    public GameObject bgCurve_flatfish;

    [Header("불가사리 주변 (느낌표)")]
    public GameObject mark_Image;
    [Header("불가사리 주변 (부럽다-조개)")]
    public GameObject jealous_Image;
    [Header("불가사리 주변 (으앗-가자미)")]
    public GameObject oh_Image;
    [Header("불가사리 주변 (아야-성게)")]
    public GameObject ouch_Image;

    public GameObject tempBG_S, tempBG_S1, tempBG_F;
    GameObject flatfish;
    Rigidbody rid;
    Animator anim;

    Yo_Chapter_1 CH1;
    Yo_TextCtrl dialog;
    Yo_PopupCtrl popup;

    [HideInInspector]
    public bool isWakeUp, isflatfishRot, isUp;

    private void Start()
    {
        rid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        dialog = GameObject.FindWithTag("Dialog").GetComponent<Yo_TextCtrl>();
        popup = popupMng.GetComponent<Yo_PopupCtrl>();
    }

    // 진짜 시작
    public void RealStart()
    {
        CH1 = chapter1.GetComponent<Yo_Chapter_1>();
    }

    // 물고기가 불가사리 밀치기
    public IEnumerator BadFishes()
    {
        // 물고기들 종이 들어가기
        CH1.isPaperOpen = false;

        // 물고기들 오른쪽으로 이동 (완전히)
        fishes.GetComponent<Animator>().enabled = true;

        // 불가사리 떨어지는 베지어커브
        yield return new WaitForSeconds(1f);
        tempBG_S = Instantiate(bgCurve_Starfish1.transform.GetChild(0).gameObject, bgCurve_Starfish1.transform.GetChild(0).gameObject.transform.parent);
        tempBG_S.SetActive(true);

        // 부딪힐 때 느낌표
        mark_Image.GetComponent<Animator>().enabled = true;

        // 떨어지고 나서 불가사리 표정 변화
        yield return new WaitForSeconds(1.8f);
        GetComponentInChildren<Renderer>().material = Resources.Load("Starfish_Material/Starfish_sad") as Material;

        // 물고기들 안 보이게
        fishes.GetComponent<Animator>().enabled = false;
        fishes.SetActive(false);
        // 베지어커브 복제 삭제
        Destroy(tempBG_S);

        // 자막 나오기
        ShowDialog(2);
    }

    // 자막 닫은 후 할 일
    public void NextToDo(int textNumber)
    {
        switch (textNumber)
        {
            // 조개 팝업종이 열리기
            case 3:
                CH1.paperNumber = 1;
                CH1.isPaperOpen = true; break;

            // 시무룩하게 이동
            case 6: StartCoroutine("MoveSad"); break;
            
            // 가자미 위로 이동
            case 8: StartCoroutine("MoveOnFish"); break;
        }
    }

    // 자막 보이게
    public void ShowDialog(int text)
    {
        if (!popup.isReset)
        {
            dialog.textNumber = text;
            if(text != 9)
            {
                dialog.TextOpen(gameObject);
            }
            else
            {
                dialog.TextOpen(chapter1);
            }
        }
    }

    // 조개 보고 우와
    public IEnumerator ClamWow(ParticleSystem particle)
    {
        // 조개 종이 들어가기
        yield return new WaitForSeconds(0);
        CH1.isPaperOpen = false;

        // 반짝 파티클
        particle.Play();

        // 부럽다 글씨
        jealous_Image.GetComponent<Animator>().enabled = true;

        // 자막 나오기
        yield return new WaitForSeconds(2);
        ShowDialog(4);
    }

    // 조개 보고 불가사리 시무룩하게 이동
    public IEnumerator MoveSad()
    {        
        // 불가사리 오른쪽으로 이동
        yield return new WaitForSeconds(2f);
        tempBG_S = Instantiate(bgCurve_Starfish1.transform.GetChild(1).gameObject, bgCurve_Starfish1.transform.GetChild(1).gameObject.transform.parent);
        tempBG_S.SetActive(true);
        anim.SetBool("IsWalkSad", true);

        // 잠시 후 걷는 애니메이션 멈추기
        yield return new WaitForSeconds(4.2f);
        anim.SetBool("IsWalkSad", false);
        Destroy(tempBG_S);

        // 가자미 미션 시작 (첫번째는 초록색)
        yield return new WaitForSeconds(1);
        flatfishMission.SetActive(true);
        flatfishMission.GetComponent<Yo_FlatfishMission>().FriendsTurn(0.32f);
    }

    // 가자미 바닥 색 맞추면 종이 들어가기
    public IEnumerator FlatfishPaperOff(GameObject _flatfish)
    {
        // 가자미 저장
        yield return new WaitForSeconds(0);
        flatfish = _flatfish;

        // 글씨 나오기
        ShowDialog(7);

        // 놀란 표정
        GetComponentInChildren<Renderer>().material = Resources.Load("Starfish_Material/Starfish_suprise") as Material;
    }

    // 불가사리 가자미 위로 이동
    public IEnumerator MoveOnFish()    
    {
        // 불가사리 오른쪽으로 이동
        yield return new WaitForSeconds(2);
        tempBG_S = Instantiate(bgCurve_Starfish1.transform.GetChild(2).gameObject, bgCurve_Starfish1.transform.GetChild(2).gameObject.transform.parent);
        tempBG_S.SetActive(true);
        anim.SetBool("IsWalk", true);
        
        // 도착하면 멈춤
        yield return new WaitForSeconds(3.15f);
        anim.SetBool("IsWalk", false);
        Destroy(tempBG_S);

        // 가자미 올라가기
        tempBG_F = Instantiate(bgCurve_flatfish.transform.GetChild(0).gameObject, bgCurve_flatfish.transform.GetChild(0).gameObject.transform.parent);
        tempBG_F.SetActive(true);
        tempBG_S = Instantiate(bgCurve_Starfish1.transform.GetChild(3).gameObject, bgCurve_Starfish1.transform.GetChild(3).gameObject.transform.parent);
        tempBG_S.SetActive(true);

        // 으앗
        yield return new WaitForSeconds(0.5f);
        oh_Image.GetComponent<Animator>().enabled = true;

        // 가자미 기울이기
        yield return new WaitForSeconds(1.8f);
        isflatfishRot = true;
        Destroy(tempBG_F);
        Destroy(tempBG_S);

        // 불가사리 떨어지는 베지어
        yield return new WaitForSeconds(0.2f);
        tempBG_S = Instantiate(bgCurve_Starfish1.transform.GetChild(4).gameObject, bgCurve_Starfish1.transform.GetChild(4).gameObject.transform.parent);
        tempBG_S.SetActive(true);

        // 불가사리 떨어지면 가자미 다시 내려오기
        yield return new WaitForSeconds(1.5f);
        isflatfishRot = false;
        tempBG_F = Instantiate(bgCurve_flatfish.transform.GetChild(1).gameObject, bgCurve_flatfish.transform.GetChild(1).gameObject.transform.parent);
        tempBG_F.SetActive(true);

        // 성게에 닿은 불가사리 뛰어오르기
        tempBG_S1 = Instantiate(bgCurve_Starfish1.transform.GetChild(5).gameObject, bgCurve_Starfish1.transform.GetChild(5).gameObject.transform.parent);
        tempBG_S1.SetActive(true);

        // 성게 커지기
        seaUrchin.GetComponent<Animator>().enabled = true;

        // 아픈 표정
        GetComponentInChildren<Renderer>().material = Resources.Load("Starfish_Material/Starfish_ouch") as Material;

        // 아얏
        ouch_Image.GetComponent<Animator>().enabled = true;

        // 거북이 등에 타기
        yield return new WaitForSeconds(1);
        Destroy(tempBG_S);
        Destroy(tempBG_S1);
        GetComponent<Rigidbody>().isKinematic = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 불가사리가 거북이에 닿았다면
        if (collision.gameObject.name == "Turtle")
        {
            // 잠시 후에 올라가기
            StartCoroutine("Up");
        }
    }

    // 불가사리가 거북이 위에 올라탄지 1초 후,
    public IEnumerator Up()
    {
        // 엔딩 시작
        yield return new WaitForSeconds(0);
        ShowDialog(9);

        // 불가사리 물리작용 끄기
        yield return new WaitForSeconds(5);
        rid.isKinematic = true;

        // 불가사리 + 거북이 올라가기
        isUp = true;

        // 거북이 애니메이션
        turtle.GetComponent<Animator>().enabled = true;
    }

    private void Update()
    {
        // 불가사리 일어나기
        if (isWakeUp)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0, -180, 0), 1f * Time.deltaTime);
        }

        // 가자미 기울이기 (-76 -> -50)
        if (flatfish)
        {
            if (isflatfishRot)
            {
                flatfish.transform.localRotation = Quaternion.Slerp(flatfish.transform.localRotation, Quaternion.Euler(-40, 148, -155), 1f * Time.deltaTime);
            }
            // 가자미 되돌리기
            else
            {
                flatfish.transform.localRotation = Quaternion.Slerp(flatfish.transform.localRotation, Quaternion.Euler(-80, 148, -155), 1f * Time.deltaTime);
            }
        }

        if (isUp)
        {
            // 불가사리 방향 바꾸기
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 3);
            
            // 불가사리 + 거북이 올라가기
            turtle.transform.localPosition = Vector3.Lerp(turtle.transform.localPosition, new Vector3(7.5f, 10, turtle.transform.localPosition.z), 0.35f * Time.deltaTime);
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(7.45f, 10.5f, transform.localPosition.z), 0.35f * Time.deltaTime);

            // 올라갈 때, x축 각도
            turtle.transform.localRotation = Quaternion.Slerp(turtle.transform.localRotation, Quaternion.Euler(-10, 0, 0), 0.4f * Time.deltaTime);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(-10, -15.23f, 0.3f), 0.4f * Time.deltaTime);


            // 불가사리가 다 올라가면
            if (turtle.transform.localPosition.y >= 8f)
            {
                // 엔딩
                CH1.isStarfishEnd = true;

                // 사라지게
                transform.GetChild(0).gameObject.SetActive(false);
                turtle.SetActive(false);
            }
        }
    }
}