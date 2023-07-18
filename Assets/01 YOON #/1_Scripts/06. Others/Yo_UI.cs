using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yo_UI : MonoBehaviour
{
    private void Update()
    {
        // 카메라 쳐다보기
        transform.LookAt(Camera.main.transform);
    }
}
