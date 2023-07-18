using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class ARTabToPlaceObejct : MonoBehaviour
{
    public GameObject gameObjectToInstantiate;
    private GameObject spawnObejct;
    private ARRaycastManager _arRaycaseManager;
    private Vector2 touchPosition;

    public bool isGameStart;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    [Header("첫번째 판넬")]
    public GameObject arUI_First;

    [Header("물어보는 판넬")]
    public GameObject arUI_Ask;

    [Header("첫번째 판넬 바뀌는 시간")]
    public float fisrtPanelOff_time;

    //GameObject text_UI;

    private void Awake()
    {
        _arRaycaseManager = GetComponent<ARRaycastManager>();
    }

    // 터치했는지 안 했는지 체크
    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if(Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }

    // 잠시 후 판넬 바꾸기
    void Start()
    {
        Invoke("FirstPanelOff", fisrtPanelOff_time);
    }

    // 판넬 1 -> 2 바꾸기
    public void FirstPanelOff()
    {
        arUI_First.GetComponentInChildren<Text>().text = "인식 될 때까지 계속 움직여주세요.";
        arUI_First.transform.GetChild(0).gameObject.SetActive(true);
    }

    void Update()
    {
        BackKeyCtrl();

        if (!TryGetTouchPosition(out Vector2 touchPosition))
        {
            return;
        }

        // 평평한 곳에 터치했다면
        if(_arRaycaseManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;

            if(spawnObejct == null)
            {
                // 책 스폰하기
                spawnObejct = Instantiate(gameObjectToInstantiate, new Vector3(hitPose.position.x, hitPose.position.y, hitPose.position.z), gameObjectToInstantiate.transform.rotation);

                // 이전에 움직이라는 UI 안 보이게
                arUI_First.SetActive(false);
                // 묻는 캔버스 보이게
                arUI_Ask.SetActive(true);

                // 원하는 위치에 터치하라는 글씨 안 보이게
                GameObject.FindWithTag("Text_UI").SetActive(false);
            }
        }
    }

    // 위치 안 바꾼다고 하면
    public void PosisionChangeYes()
    {
        // 묻는 캔버스 없애고
        arUI_Ask.SetActive(false);

        // ARPlane 전체 안 보이게
        GameObject.FindWithTag("ARPlane").SetActive(false);

        // 게임 시작
        isGameStart = true;
    }

    // 위치 바꾼다고 하면
    public void PosisionChangeNo()
    {
        // 묻는 캔버스 없애고
        arUI_Ask.SetActive(false);

        // 책 없애기
        Destroy(spawnObejct);

        // 원하는 위치에 터치하라는 글씨 보이게
        GameObject.FindWithTag("Text_UI").SetActive(true);
    }


    // 뒤로가기누르면 꺼지기
    public void BackKeyCtrl()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }
}
