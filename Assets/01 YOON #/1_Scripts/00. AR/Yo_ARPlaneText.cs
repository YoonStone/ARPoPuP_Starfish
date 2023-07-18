using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Yo_ARPlaneText : MonoBehaviour
{
    private void Start()
    {
        // AR플레인이 활성화 되면 (평평한 곳을 인식하면) 이전에 움직이라는 UI 안 보이게
        GameObject.FindWithTag("Panel_UI").SetActive(false);
    }

    private void Update()
    {
        // 만약 게임이 시작 된 후에
        if (GameObject.FindWithTag("ARController").GetComponent<ARTabToPlaceObejct>().isGameStart)
        {
            // AR플레인이 활성화 된다면 AR플레인 전체 끄기
            transform.parent.gameObject.SetActive(false);
        }
    }
}
