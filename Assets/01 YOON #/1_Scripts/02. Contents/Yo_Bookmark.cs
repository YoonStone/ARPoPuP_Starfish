using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yo_Bookmark : MonoBehaviour
{
    public bool isSetting;

    public GameObject popup;

    Yo_PopupCtrl popup_script;

    bool isChange;

    private void Start()
    {
        popup_script = popup.GetComponent<Yo_PopupCtrl>();
    }

    private void OnMouseDown()
    {
        // 세팅
        if (isSetting)
        {
            popup_script.GoSetting();
        }
        // 목차
        else
        {
            // 지금 챕터의 리셋함수 호출
            if(popup_script.beforeState != 6)
            {
                popup_script.chapter[popup_script.beforeState].SendMessage("ResetChapter");
                print("북마크 누름 이전챕터를 " + popup_script.beforeState + "으로 저장");
            }
            else
            {
                popup_script.GoContent(6);
            }
            popup_script.isContent = true;

            //popup_script.GoContent(popup_script.beforeState);
            //print("북마크 누름 이전챕터를 " + popup_script.beforeState + "으로 저장");
        }
    }
}
