using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Khi đủ 2 Player trong phòng rồi thì Spawn ra các Object và bắt đầu Duel
public class DuelObjectSpawner : MonoBehaviour
{
    public List<GameObject> prefabsNetwork;
    public List<GameObject> prefabsLocal;
    private bool isSpawn = false; //Đã Spawn hay chưa ?


    private void Update()
    {

        if (isSpawn)
            return;

        //Chưa đủ Player thì không bắt đầu
        if (PhotonNetwork.CurrentRoom.Players.Count < 2)
            return;

        SpawnObjectsNetwork();

        SpawnObjectsLocal();

        isSpawn = true;

    }

    public void SpawnObjectsNetwork()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        foreach (GameObject prefab in prefabsNetwork)
        {
            PhotonNetwork.Instantiate(prefab.name, Vector3.zero, Quaternion.identity);
        }

    }

    public void SpawnObjectsLocal()
    {

        foreach (GameObject prefab in prefabsLocal)
        {
            Instantiate(prefab, Vector3.zero, Quaternion.identity);
        }
    }
}
