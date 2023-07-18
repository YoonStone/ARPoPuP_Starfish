using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yo_GameEnd : MonoBehaviour
{

    private void OnMouseDown()
    {
        GameEnd();
    }

    public void GameEnd()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
         Application.OpenURL(webplayerQuitURL);
#else
         Application.Quit();
#endif   
    }
}
