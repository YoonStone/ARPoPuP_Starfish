using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yo_TrashChange : MonoBehaviour
{
    public GameObject trashCan;

    public Yo_Chapter_4 CH4;

    private void OnTriggerExit(Collider other)
    {
        // 불가사리랑 닿았다가 떨어지면
        if (other.transform.name == "Starfish_4")
        {
            Change();
        }
    }

    public void Change()
    {
        if (!CH4.popup.isReset)
        {
            // 효과음
            CH4.audioCtrl.StarEffect();
            trashCan.GetComponent<Animator>().enabled = true;
            gameObject.SetActive(false);
            CH4.trashChangeCount++;
        }
    }
}
