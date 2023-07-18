using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Yo_ARReset : MonoBehaviour
{
    public GameObject askPanel;
    GameObject saveCtrl;
    DataController dataCtrl;

    public void Start()
    {
        saveCtrl = GameObject.FindWithTag("SaveController");
        dataCtrl = saveCtrl.GetComponent<DataController>();

    }

    // AR씬 다시 시작하기 버튼 눌렀을 때
    public void ReARSceneAsk()
    {
        // 물어보는 판넬보이게
        dataCtrl.SaveGameData();
        askPanel.SetActive(true);
    }


    // AR씬 다시 시작하기
    public void ReARScene()
    {
        dataCtrl.SaveGameData();
        SceneManager.LoadScene(1);
    }

    // AR씬 다시 시작 안함
    public void DontReAR()
    {
        askPanel.SetActive(false);
    }
}
