using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 움직여질 물건(종이)에 넣을 스크립트
public class Yo_ColorPaper : MonoBehaviour
{
    [Header("챕터 1")]
    public GameObject chapter1;
    [Header("가자미 미션")]
    public GameObject flatfishMission;
    [Header("상호작용 할 오브젝트")]
    public GameObject inter_Obj;
    [Header("빠밤 파티클")]
    public ParticleSystem glitter_particle;

    Yo_FlatfishMission mission;
    Yo_Chapter_1 CH1;

    Renderer flatColor;

    public GameObject starfish;
    private Vector3 screenSpace;
    private Vector3 offset;
    private float firstX_Pos, lastX_Pos, goal_min, goal_max;
    private bool isEnd, isStillHere;

    private void Start()
    {
        mission = flatfishMission.GetComponent<Yo_FlatfishMission>();
        CH1 = chapter1.GetComponent<Yo_Chapter_1>();

        flatColor = inter_Obj.GetComponent<Renderer>();

        // 종이의 맨 왼쪽, 맨 오른쪽 값 저장
        firstX_Pos = -5.55f;
        lastX_Pos = firstX_Pos + 11.1f;
    }

    void Update()
    {
        // 종이 제한 -6.5 ~ 6.5
        transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, firstX_Pos, lastX_Pos),
            transform.localPosition.y, transform.localPosition.z);

        goal_min = mission.goal_hue - 0.02f;
        goal_max = mission.goal_hue + 0.02f;
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

            // 인터렉션 오브젝트 조절
            InterStart_Color();
        }
    }

    private void OnMouseUp()
    {
        // 목표 색으로부터 오차범위 0.01
        if ((transform.localPosition.x - firstX_Pos) * 0.9f / 11.1f > goal_min && (transform.localPosition.x - firstX_Pos) * 0.9f / 11.1f < goal_max)
        {
            //print("맞춤");
            isStillHere = true;

            // 1초 뒤에도 그대로라면
            Invoke("Still", 0.5f);
        }
    }

    public void InterStart_Color()
    {
        // 0 : 가장 어두운 색, 0.9 : 가장 밝은 색

        // 0보다 작거나 1보다 크다면 0, 1에 제한두기        
        if ((transform.localPosition.x - firstX_Pos) * 0.9f / 11.1f <= 0)
        {
            flatColor.material.SetFloat("_HueShift", 0);
        }
        else if ((transform.localPosition.x - firstX_Pos) * 0.9f / 11.1f >= 1)
        {
            flatColor.material.SetFloat("_HueShift", 0.9f);
        }

        // 0보다 크고 -1보다 작을때는 종이의 움직임에 따라 움직임
        else
        {
            flatColor.material.SetFloat("_HueShift", (transform.localPosition.x - firstX_Pos) * 0.9f / 11.1f);
        }
    }

    // 아직도 그 자리에 있는지 확인
    public void Still()
    {
        // 성공하면
        if (isStillHere)
        {
            // 파티클 나오기
            glitter_particle.Play();

            // 가자미종이 들어가기
            CH1.paperNumber = 2;
            CH1.isPaperOpen = false;

            isStillHere = false;

            mission.PlayerSuccess();
        }
    }

    // 인터렉션 완성된 상태로 바꾸기(고정시키기)
    public void InterEnd()
    {
        // 불가사리 가자미 위로 이동하라고 전달
        starfish.GetComponent<Yo_Starfish_1>().StartCoroutine("FlatfishPaperOff", inter_Obj.transform.parent.gameObject);

        // 반복 방지
        isEnd = false;
    }
}
