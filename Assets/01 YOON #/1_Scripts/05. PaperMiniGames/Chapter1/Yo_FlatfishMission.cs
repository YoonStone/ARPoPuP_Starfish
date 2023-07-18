using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yo_FlatfishMission : MonoBehaviour
{
    public GameObject chapter1;
    public GameObject paper;
    public GameObject[] flatFriends;
    public GameObject flat;

    private bool isFirendsChange, isSaturation;
    private float saturation = 0.8f;
    private float hue_friend= 0.08f;
    public float goal_hue;
    public int count = 3;

    Renderer flatFriendColor1, flatFriendColor2, flatColor;

    Yo_Chapter_1 CH1;

    void Start()
    {
        CH1 = chapter1.GetComponent<Yo_Chapter_1>();

        flatFriendColor1 = flatFriends[0].GetComponent<Renderer>();
        flatFriendColor2 = flatFriends[1].GetComponent<Renderer>();
        flatColor = flat.GetComponent<Renderer>();
    }

    // 가자미 친구들 색 변하기
    public void FriendsTurn(float goalHue)
    {
        goal_hue = goalHue;
        isFirendsChange = true;

        StartCoroutine("PlayerTurn");
    }

    // 플레이어 차례
    public IEnumerator PlayerTurn()
    {
        // 종이 먼저 나오기
        yield return new WaitForSeconds(3);
        CH1.paperNumber = 2;
        CH1.isPaperOpen = true;
    }

    // 플레이어가 알맞는 곳에 뒀다면
    public void PlayerSuccess()
    {
        count--;

        // 2번째 미션이라면 보라색
        if (count == 2) FriendsTurn(0.7f);

        // 3번째 미션이라면 주황색
        else if (count == 1) FriendsTurn(0.08f);

        // 마지막까지 성공했다면
        else if (count == 0)
        {
            isSaturation = true;
            paper.GetComponent<Yo_ColorPaper>().Invoke("InterEnd", 1);
        }
    }

    void Update()
    {
        // 가자미 친구들 색 설정
        flatFriendColor1.material.SetFloat("_HueShift", hue_friend);
        flatFriendColor2.material.SetFloat("_HueShift", hue_friend);

        // 모든 가자미 친구들 색 변하기
        if (isFirendsChange)
        {
            hue_friend = Mathf.Lerp(hue_friend, goal_hue, Time.deltaTime * 0.8f);
        }

        // 마지막으로 모든 가자미 채도 조절하기
        if (isSaturation)
        {
            // 모든 가자미 친구들
            flatFriendColor1.material.SetFloat("_SaturationShift", saturation);
            flatFriendColor2.material.SetFloat("_SaturationShift", saturation);

            // 주인공 가자미 채도 조절하기
            flatColor.material.SetFloat("_SaturationShift", saturation);

            // 채도를 0.8 -> 0.4
            saturation = Mathf.Lerp(saturation, 0.4f, Time.deltaTime * 1f);
        }
    }
}
