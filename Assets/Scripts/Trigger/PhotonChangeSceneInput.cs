using TMPro;
using UnityEngine;

public class PhotonChangeSceneInput : PhotonChangeScene
{
    public GameObject messageTxt; 
    bool isTrigger = false;

    private void Update()
    {
        if (isTrigger == false)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayerInfoDDOL.Instance.SetPlayerLastPos(GameObject.FindGameObjectWithTag("Player").transform.position);
            Invoke("ChangeScene", 0.1f);
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isTrigger = true;
            messageTxt.SetActive(true);
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isTrigger = false;
            messageTxt.SetActive(false);
        }
            
    }
}
