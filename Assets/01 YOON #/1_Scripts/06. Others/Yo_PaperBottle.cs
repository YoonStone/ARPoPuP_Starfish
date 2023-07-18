using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yo_PaperBottle : MonoBehaviour
{
    public GameObject paperBottleUI;

    bool isUIOn;

    private void OnMouseDown()
    {
        // 병 터치하면 UI 뜨기
        paperBottleUI.SetActive(true);

        Invoke("UIOn", 1f);
    }

    private void Update()
    {
        // UI가 떠있는 동안
        if (isUIOn)
        {
            // 화면 아무곳이나 터치하면
            if (Input.GetMouseButtonDown(0))
            {
                // UI 끄기
                paperBottleUI.SetActive(false);

                // UI가 떠있지 않음
                isUIOn = false;
            }
        }
    }

    public void UIOn()
    {
        // UI가 떠있음
        isUIOn = true;
    }
}
