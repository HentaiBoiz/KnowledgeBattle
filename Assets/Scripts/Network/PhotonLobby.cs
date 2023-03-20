using Photon.Pun;
using UnityEngine;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    public string SCENE_NAME = "";

    //Kết nối thành công rồi thì join lobby
    public override void OnConnectedToMaster()
    {
        //Trừ điểm Rank

        Debug.Log("ConnectedToMaster");
        PhotonNetwork.LoadLevel(SCENE_NAME);
    }

}
