using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yo_SettingContinue : MonoBehaviour
{
    public GameObject popupMng;

    Yo_PopupCtrl popup;

    private void Start()
    {
        popup = popupMng.GetComponent<Yo_PopupCtrl>();
    }

    // 게임으로 돌아가기 누르면
    private void OnMouseDown()
    {
        popup.GoContinue();
    }
}
