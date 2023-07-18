using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yo_SettingCtrl : MonoBehaviour
{
    [Header("진동 사용 여부")]
    public bool isViabrate;
    [Header("기울기 사용 여부")]
    public bool isAcceleration;

    [Header("챕터 2 잠금 상태")]
    public bool isClear2;
    [Header("챕터 3 잠금 상태")]
    public bool isClear3;
    [Header("챕터 4 잠금 상태")]
    public bool isClear4;
    [Header("챕터 5 잠금 상태")]
    public bool isClear5;

    public GameObject[] check;
    GameObject saveCtrl;
    Yo_PopupCtrl popup;
    DataController dataCtrl;

    private void Start()
    {
        popup = GetComponent<Yo_PopupCtrl>();
        saveCtrl = GameObject.FindWithTag("SaveController");
        dataCtrl = saveCtrl.GetComponent<DataController>();

        isClear2 = dataCtrl.gameData.isClear2;
        isClear3 = dataCtrl.gameData.isClear3;
        isClear4 = dataCtrl.gameData.isClear4;
        isClear5 = dataCtrl.gameData.isClear5;
    }

    private void Update()
    {
        check[0].SetActive(isViabrate);
        check[1].SetActive(isAcceleration);
    }

    public void CheckClick(string checkName)
    {
        // 진동버튼을 클릭
        if (checkName.Contains("V"))
        {
            if (isViabrate)
            {
                isViabrate = false;
            }
            else
            {
                isViabrate = true;
            }
        }

        // 기울기버튼을 클릭
        if (checkName.Contains("A"))
        {
            if (isAcceleration)
            {
                isAcceleration = false;
            }
            else
            {
                isAcceleration = true;
            }
        }
    }

    // 진동 여부
    public void IsVibrate(bool _isVibrate)
    {
        isViabrate = _isVibrate;
    }

    // 기울기 여부
    public void IsAcceleration(bool _isAcceleration)
    {
        isAcceleration = _isAcceleration;
    }

    public void ChaperUnlock(int chapterNumber)
    {
        switch (chapterNumber)
        {
            case 2:
                isClear2 = true;
                dataCtrl.gameData.isClear2 = isClear2;
                dataCtrl.SaveGameData();
                break;
            case 3:
                isClear3 = true;
                dataCtrl.gameData.isClear3 = isClear3;
                dataCtrl.SaveGameData();
                break;
            case 4:
                isClear4 = true;
                dataCtrl.gameData.isClear4 = isClear4;
                dataCtrl.SaveGameData();
                break;
            case 5:
                isClear5 = true;
                dataCtrl.gameData.isClear5 = isClear5;
                dataCtrl.SaveGameData();
                break;
        }
    }
}
