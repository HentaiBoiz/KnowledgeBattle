using Photon.Pun;
using UnityEngine;

public class PhotonChangeScene : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private string SCENE_NAME;

    public virtual void ChangeScene()
    {
        //Cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        PhotonNetwork.LoadLevel(SCENE_NAME);
    }
}
