using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestButtonScript : MonoBehaviour
{
    public int questID;
    public TextMeshProUGUI questTitle;

    private GameObject acceptBtn;
    private GameObject giveUpBtn;
    private GameObject completeBtn;

    private QuestButtonScript acceptBtnScript;
    private QuestButtonScript giveUpBtnScript;
    private QuestButtonScript completeBtnScript;

    private void Start()
    {
        acceptBtn = GameObject.Find("QuestBoardTest").transform.Find("QuestBoard").transform.Find("QuestDescription").transform.Find("GameObject").transform.Find("BtnAccept").gameObject;
        acceptBtnScript = acceptBtn.GetComponent<QuestButtonScript>();

        giveUpBtn = GameObject.Find("QuestBoardTest").transform.Find("QuestBoard").transform.Find("QuestDescription").transform.Find("GameObject").transform.Find("BtnGiveUp").gameObject;
        giveUpBtnScript = giveUpBtn.GetComponent<QuestButtonScript>();

        completeBtn = GameObject.Find("QuestBoardTest").transform.Find("QuestBoard").transform.Find("QuestDescription").transform.Find("GameObject").transform.Find("BtnComplete").gameObject;
        completeBtnScript = completeBtn.GetComponent<QuestButtonScript>();

        acceptBtn.SetActive(false);
        giveUpBtn.SetActive(false);
        completeBtn.SetActive(false);
    }
    // show info quest
    public void ShowInfo()
    {
        QuestUIManager.Instance.ShowDescriptionQuest(questID);
        //Accept Button
        if (QuestManager.Instance.RequestAvailableQuest(questID))
        {
            acceptBtn.SetActive(true);
            acceptBtnScript.questID = questID;
        }
        else
        {
            acceptBtn.SetActive(false);
        }

        //GiveUp Button
        if (QuestManager.Instance.RequestAcceptedQuest(questID))
        {
            giveUpBtn.SetActive(true);
            giveUpBtnScript.questID = questID;
        }
        else
        {
            giveUpBtn.SetActive(false);
        } 

        //Complete Button
        if (QuestManager.Instance.RequestCompleteQuest(questID))
        {
            completeBtn.SetActive(true);
            completeBtnScript.questID = questID;
        }
        else
        {
            completeBtn.SetActive(false);
        }

    }

    public void AcceptQuest()
    {
        QuestManager.Instance.AcceptedQuest(questID);
        QuestUIManager.Instance.HideQuestPanel();

        //Update NPCs
        DialogueMission[] currentQuestNPCs = FindObjectsOfType(typeof(DialogueMission)) as DialogueMission[];

        foreach (DialogueMission currentQuestNPC in currentQuestNPCs)
        {
            //set quest icon
            currentQuestNPC.SetQuestMaker();
        }
    }
    public void GiveUPQuest()
    {
        QuestManager.Instance.GiveUpQuest(questID);
        QuestUIManager.Instance.HideQuestPanel();

        //Update NPCs
        DialogueMission[] currentQuestNPCs = FindObjectsOfType(typeof(DialogueMission)) as DialogueMission[];
        foreach (DialogueMission currentQuestNPC in currentQuestNPCs)
        {
            //set quest icon
            currentQuestNPC.SetQuestMaker();
        }
    }

    public void CompleteQuest()
    {
        QuestManager.Instance.CompleteQuest(questID);
        QuestUIManager.Instance.HideQuestPanel();

        //Update NPCs
        DialogueMission[] currentQuestNPCs = FindObjectsOfType(typeof(DialogueMission)) as DialogueMission[];

        foreach (DialogueMission currentQuestNPC in currentQuestNPCs)
        {
            //set quest icon
            currentQuestNPC.SetQuestMaker();
        }
    }

    public void CloseBoard()
    {
        QuestUIManager.Instance.HideQuestPanel();
    }
}
