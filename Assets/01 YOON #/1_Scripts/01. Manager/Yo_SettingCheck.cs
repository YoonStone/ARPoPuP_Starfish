using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yo_SettingCheck : MonoBehaviour
{
    public GameObject popup;
    Yo_SettingCtrl setting;

    private void Start()
    {
        setting = popup.GetComponent<Yo_SettingCtrl>();
    }

    private void OnMouseDown()
    {
        setting.CheckClick(name);
    }
}
