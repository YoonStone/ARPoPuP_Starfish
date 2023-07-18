using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yo_TrashThrow : MonoBehaviour
{
    public int trashKinds;

    Rigidbody rid;
    Yo_TrashMission trashMission;
    Yo_AudioCtrl audioCtrl;

    void Start()
    {
        rid = GetComponent<Rigidbody>();
        trashMission = GameObject.FindWithTag("TrashSpawn").GetComponent<Yo_TrashMission>();
        audioCtrl = GameObject.FindWithTag("EditorOnly").GetComponent<Yo_AudioCtrl>();

        switch (trashKinds)
        {
            case 1: rid.AddForce(Vector3.right * 850 + Vector3.up * 600); break;
            case 2: rid.AddForce(Vector3.right * 1050 + Vector3.up * 650); break;
            case 3: rid.AddForce(Vector3.right * 1255 + Vector3.up * 700); break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 쓰레기통 안에 들어가면
        if (other.name.Contains("coll"))
        {
            // 점수 증가
            trashMission.goalCount++;
            // 팻말에 표시
            trashMission.NumberSetting(trashMission.goalCount);

            // 부딪히는 효과음 (탁)
            audioCtrl.TouchLight();

            // 1초 후 쓰레기 사라지기
            Invoke("DestroyTrash", 1);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 뚜껑에 맞으면
        if (collision.gameObject.name.Contains("Trash_Cover"))
        {
            // 부딪히는 효과음 (탁)
            audioCtrl.TouchLight();
            rid.AddForce(Vector3.forward * 200 + Vector3.up * 200);
        }
    }

    public void DestroyTrash()
    {
        Destroy(gameObject);
    }
}
