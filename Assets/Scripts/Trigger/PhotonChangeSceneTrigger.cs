
using UnityEngine;

public class PhotonChangeSceneTrigger : PhotonChangeScene
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {

            ChangeScene();
        }
    }
}
