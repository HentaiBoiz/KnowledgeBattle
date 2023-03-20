using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    public string sceneToLoad = "1_OfflineScene";
    public string spawPointName;
    Transform playerPos;

    private void OnTriggerEnter(Collider other)
    {
        
        
    }

    public void btnExit()
    {
        PhotonNetwork.LoadLevel(sceneToLoad);
    }
}
