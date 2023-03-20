
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueMission : DialogueTrigger
{
    public DialogueMission dialogueMission;

    public List<int> availableQuestIDs = new List<int>();
    public List<int> receivableQuestIDs = new List<int>();

    public GameObject iconQuesting;
    public Image theImage;

    public Sprite questAvailableSprite;
    public Sprite questReceivableSprite;

    private void Start()
    {
        SetQuestMaker();
    }

    // Update is called once per frame
    private void Update()
    {
        TalkToNPC(inkTxt);
    }

    public void SetQuestMaker()
    {
        if (QuestManager.Instance.CheckCompleteQuest(this))
        {
            iconQuesting.SetActive(true);
            theImage.sprite = questReceivableSprite;
            theImage.color = Color.blue;
        }
        else if(QuestManager.Instance.CheckAvailableQuest(this))
        {
            iconQuesting.SetActive(true);
            theImage.sprite = questAvailableSprite;
            theImage.color = Color.blue;
        }
        else if (QuestManager.Instance.CheckAcceptedQuest(this))
        {
            iconQuesting.SetActive(true);
            theImage.sprite = questReceivableSprite;
            theImage.color = Color.yellow;
        }
        else
        {
            iconQuesting.SetActive(false);
        }
    }
    public override void TalkToNPC(TextAsset Txt)
    {
        if (playerInRange && !DialogueManager.Instance.dialogueActive)
        {
            dialogueGUI.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F))
            {
                DialogueManager.Instance.StartDialogue(Txt);
                //Quest UI Manager
                QuestUIManager.Instance.CheckQuests(this);

                //QuestManager.Instance.QuestRequest(this);
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
