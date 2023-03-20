using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    public string sceneToLoad ;
    public string spawPointName;
    Transform playerPos;

    private void OnTriggerEnter(Collider other)
    {
        QuestManager.Instance.AddQuest("Go to Shop", 1);
        SceneManager.LoadScene("Shop"); 
    }
}
