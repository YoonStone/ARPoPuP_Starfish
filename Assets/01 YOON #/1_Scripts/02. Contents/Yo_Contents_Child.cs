using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Yo_Contents_Child : MonoBehaviour
{
    public GameObject popupCtrl;

    Yo_Contents parentContent_script;

    private void Start()
    {
        parentContent_script = transform.parent.parent.GetComponent<Yo_Contents>();
    }

    // 해당 챕터를 터치하면
    void OnMouseDown()
    {
        parentContent_script.IslandClick();
    }
}
