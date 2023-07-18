using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ARTabToPlaceObejct1 : MonoBehaviour
{
    public GameObject gameObjectToInstantiate;
    private GameObject spawnObejct;


    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            spawnObejct = Instantiate(gameObjectToInstantiate, new Vector3(0, 0, 0), gameObjectToInstantiate.transform.rotation);
        }
    }
}
