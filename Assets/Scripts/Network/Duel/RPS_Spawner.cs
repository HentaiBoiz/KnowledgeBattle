using Photon.Pun;
using UnityEngine;

//RPS = Rock, Paper, Scissor
public class RPS_Spawner : MonoBehaviour
{
    public GameObject prefabRPS;

    bool isInit = false;

    private void Start()
    {

        isInit = false;

    }

    private void Update()
    {
        if (isInit == true)
            return;

        //Đủ 2 Player thì mới spawn ra RPS
        if(PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            InitializeRPS();
            isInit = true;
        }
    }

    public void InitializeRPS()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        PhotonNetwork.Instantiate(prefabRPS.name, Vector3.zero, Quaternion.identity);
    }
}
