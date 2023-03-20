using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DialogueTrigger : MonoBehaviour
{
    public GameObject dialogueGUI;

    public bool playerInRange;

    public TextAsset inkTxt;

    private void Awake()
    {
        playerInRange = false;
        dialogueGUI.SetActive(false);
    }

    private void Update()
    {
        TalkToNPC(inkTxt);
    }

    public virtual void TalkToNPC(TextAsset Txt)
    {
        if (playerInRange && !DialogueManager.Instance.dialogueActive)
        {
            dialogueGUI.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F))
            {
                DialogueManager.Instance.StartDialogue(Txt);
            }
        }
        else
        {
            dialogueGUI.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }
}
