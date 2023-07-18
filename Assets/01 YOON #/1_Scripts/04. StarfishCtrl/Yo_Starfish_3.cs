using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yo_Starfish_3 : MonoBehaviour
{

    [Header("리스폰 시간")]
    public float time_Respawn = 2;

    [Header("챕터 3")]
    public GameObject Chpater3;

    [Header("베지어커브 (불가사리)")]
    public GameObject bgCurve_Starfish3;
    [Header("불가사리 리스폰 위치")]
    public GameObject respawn_Starfish3;
    [Header("리스폰 유도 UI")]
    public GameObject respawn_UI;

    Collider trigger;
    public Yo_Chapter_3 CH3;
    GameObject tempBG;

    public bool isStarfishArrive;

    public bool isMission, isStarFallDown;
    public bool isRe, isFallDown, isNotFirst;

    private void Start()
    {
        CH3 = Chpater3.GetComponent<Yo_Chapter_3>();
    }

    public void RealStart()
    {
        isMission = false;
        isStarFallDown = false;
        isRe = false;
        isFallDown = false;
        isNotFirst = false;
        trigger = null;
    }

    // 불가사리 선물 앞으로 움직이게
    public void GoPresent()
    {
        tempBG = Instantiate(bgCurve_Starfish3, bgCurve_Starfish3.transform.parent);
        tempBG.SetActive(true);

        Invoke("TempOff", 6f);

        // 어느정도 이동했으면 난로 켜지기
        CH3.Invoke("FireDelay", 4f);
    }

    public void TempOff()
    {
        Destroy(tempBG);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!CH3.popup.isReset)
        {
            // 바닥에 떨어졌을 때
            if (collision.transform.name.Contains("(Ch3"))
            {
                // 미션 중이라면
                if (isMission)
                {
                    print("미션 중 바닥에 떨어짐");
                    // 리스폰 유도
                    Invoke("RespawnUI", time_Respawn);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!CH3.popup.isReset)
        {
            trigger = other;

            // 목적지에 도착하면
            if (other.transform.name.Contains("Goal"))
            {
                // 도착 딜레이
                Invoke("GoalDelay", 2);
            }

            // 미션 중에
            if (isMission)
            {
                // 책 밖으로 떨어지면
                if (other.transform.name == "Outside_Collider")
                {
                    print("미션 중 책밖으로 떨어짐");
                    // 리스폰 유도
                    RespawnUI();
                }
            }
        }
    }

    // 리스폰 유도 한번만
    public void RespawnUI()
    {
        // 리스폰 유도
        if (!isNotFirst)
        {
            respawn_UI.SetActive(true);
            isNotFirst = true;
        }
    }

    // 리스폰 딜레이 (리스폰 버튼 클릭)
    public void RespawnDelay()
    {
        transform.position = respawn_Starfish3.transform.position;
        transform.rotation = respawn_Starfish3.transform.rotation;

        isMission = false;

        respawn_UI.SetActive(false);
    }

    // 넘어진 후 리스폰 딜레이
    public void FallDownDelay()
    {
        if (!CH3.popup.isReset)
        {
            // 계속 각도가 이상하다면
            if (isFallDown)
            {
                print("미션 중 각도가 이상함");
                // 리스폰 유도
                RespawnUI();
            }
        }
    }

    // 1초가 지난 뒤에도 선물 위에 있다면
    public void GoalDelay()
    {
        if (!CH3.popup.isReset)
        {
            if (trigger != null && trigger.transform.name.Contains("Goal"))
            {
                if (!isRe)
                {
                    // 콜라이더 없애기
                    trigger.gameObject.SetActive(true);

                    // 회전, 점프종이 들어가게
                    CH3.paperNumber = 0;
                    CH3.isPaperOpen = false;

                    // 리스폰 버튼 안 보이게
                    CH3.respawnUI.SetActive(false);

                    // 불가사리 각도 예쁘게 변경
                    isStarfishArrive = true;

                    // 떨어지지 않도록 물리작용 끄고
                    GetComponent<Rigidbody>().isKinematic = true;

                    // 미션 끝
                    isMission = false;

                    // 글씨 적히도록
                    CH3.ShowDialog(23);

                    isRe = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        trigger = null;
    }

    private void Update()
    {
        if (!CH3.popup.isReset)
        {
            // 미션 중에
            if (isMission)
            {
                // 불가사리 각도가 이상하다면
                if (transform.rotation.z < -0.1f || transform.rotation.z > 0.1f)
                {
                    isFallDown = true;

                    // 1초 후에도 그대로인지 확인
                    Invoke("FallDownDelay", 1);
                }
                else
                {
                    isFallDown = false;
                }
            }

            // 트리에 도착 후 불가사리 일어나기
            if (isStarfishArrive)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, -270, 0), Time.deltaTime * 5);
            }

            // 리스폰 유도 UI가 켜진 상태에서 화면 아무 곳이나 터치하면
            if (respawn_UI.activeInHierarchy)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    respawn_UI.SetActive(false);
                }
            }
        }
    }
}
