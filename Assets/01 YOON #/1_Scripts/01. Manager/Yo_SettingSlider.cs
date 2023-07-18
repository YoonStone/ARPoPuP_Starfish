using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yo_SettingSlider : MonoBehaviour
{
    public GameObject popup;
    Yo_AudioCtrl audioCtrl;

    private Vector3 screenSpace;
    private Vector3 offset;
    private float firstX_Pos = 0;

    private void Start()
    {
        audioCtrl = popup.GetComponent<Yo_AudioCtrl>();
    }

    private void Update()
    {
        // 슬라이드 제한 (0 ~ -5)
        transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, firstX_Pos - 5, firstX_Pos),
            transform.localPosition.y, transform.localPosition.z);
        // 슬라이드 제한 (0 ~ -5)
        transform.localPosition = new Vector3(transform.localPosition.x,
            Mathf.Clamp(transform.localPosition.y, 0.4f, 0.4f), transform.localPosition.z);
        // 슬라이드 제한 (0 ~ -5)
        transform.localPosition = new Vector3(transform.localPosition.x,
            transform.localPosition.y, Mathf.Clamp(transform.localPosition.z, 0, 0));


        if (name.Contains("BGM"))
            audioCtrl.bgmSlider = transform.localPosition.x * -0.2f;

        if(name.Contains("Effect"))
            audioCtrl.effectSlider = transform.localPosition.x * -0.2f;
    }

    private void OnMouseDown()
    {
        // 움직일 물건의 위치를 스크린좌표로 저장
        screenSpace = Camera.main.WorldToScreenPoint(transform.position);

        // 마우스와 움직일 물건 사이의 거리 저장
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));
    }

    private void OnMouseDrag()
    {
        // 마우스 위치 저장(계속)
        Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);

        // 저장된 마우스위치를 월드좌표로 바꾸고 + offset 
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;

        // x축으로만 이동
        transform.position = new Vector3(curPosition.x, transform.position.y, transform.position.z);
    }
}
