using Photon.Pun;
using UnityEngine;

public class CameraSide : MonoBehaviour
{
    public Transform cameraHolder_1;
    public Transform cameraHolder_2;

    public Camera mainCamera;

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            mainCamera.transform.SetParent(cameraHolder_1);
        }
        else
        {
            mainCamera.transform.SetParent(cameraHolder_2);
        }

        mainCamera.transform.localPosition = Vector3.zero;
        mainCamera.transform.localRotation = Quaternion.identity;
    }
}
