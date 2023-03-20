using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflinePlayerManager : MonoBehaviour
{
    private void Start()
    {

        MovePlayerToLastPos();

    }

    //Di chuyển Player về vị trí cuối cùng được gắn
    public void MovePlayerToLastPos()
    {
        GetComponent<CharacterController>().enabled = false;
        transform.position = PlayerInfoDDOL.Instance.GetPlayerLastPos();
        GetComponent<CharacterController>().enabled = true;
    }
}
