using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    public List<Quest> questsList = new List<Quest>(); //Master Quest List
    public List<Quest> currentQuestsList = new List<Quest>(); // Current Quest List

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
        else if (Instance != this)
        {
            Debug.Log("Found more than one Player_Quest_Manager in the scene");
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    //Quest Request
    public void QuestRequest(DialogueMission NPCQuest)
    {
        if(NPCQuest.availableQuestIDs.Count >0)
        {
            for(int i = 0; i< questsList.Count; i++)
            {
                for (int j = 0; j < NPCQuest.availableQuestIDs.Count; j++)
                {
                    if (questsList[i].id == NPCQuest.availableQuestIDs[j] && questsList[i].progress == Quest.QuestProgress.AVAILABLE)
                    {
                        Debug.Log("Quest Id: " +NPCQuest.availableQuestIDs[j] +" is "+ questsList[i].progress);
                        //Test
                        //AcceptedQuest(NPCQuest.availableQuestIDs[j]);
                        //Quest UI
                        QuestUIManager.Instance.questAvailable = true;
                        QuestUIManager.Instance.availableQuests.Add(questsList[i]);
                    }

                }
            }
        }

        for (int i = 0; i < currentQuestsList.Count; i++)
        {
            for (int j = 0; j < NPCQuest.receivableQuestIDs.Count; j++)
            {
                if (currentQuestsList[i].id == NPCQuest.receivableQuestIDs[j] && currentQuestsList[i].progress == Quest.QuestProgress.ACCEPTED || currentQuestsList[i].progress == Quest.QuestProgress.COMPLETE )
                {
                    Debug.Log("Quest Id: " + NPCQuest.receivableQuestIDs[j] + " is " + currentQuestsList[i].progress);

                    //CompleteQuest(NPCQuest.receivableQuestIDs[j]);
                    //Quest UI
                    QuestUIManager.Instance.questRunning = true;
                    QuestUIManager.Instance.activeQuests.Add(currentQuestsList[i]);
                }

            }
        }

    }

    public void ShowQuestLog(int questID)
    {
        for (int i = 0; i < currentQuestsList.Count; i++)
        {
            if (currentQuestsList[i].id == questID)
            {
                QuestUIManager.Instance.ShowQuestLog(currentQuestsList[i]);
            }
        }
    }



    //Accepted Quest
    public void AcceptedQuest(int questID)
    {
        for (int i = 0; i < questsList.Count; i++)
        {
            if (questsList[i].Id == questID && questsList[i].progress == Quest.QuestProgress.AVAILABLE)
            {
                currentQuestsList.Add(questsList[i]);
                questsList[i].progress = Quest.QuestProgress.ACCEPTED;
            }
        }
    }

    //Give up Quest
    public void GiveUpQuest(int questID)
    {
        for (int i = 0; i < currentQuestsList.Count; i++)
        {
            if (currentQuestsList[i].Id == questID && currentQuestsList[i].progress == Quest.QuestProgress.ACCEPTED)
            {
                currentQuestsList[i].progress = Quest.QuestProgress.AVAILABLE;
                currentQuestsList[i].QuestObjectiveCount = 0;
                currentQuestsList.Remove(currentQuestsList[i]);
            }
        }
    }

    //Complete Quest
    public void CompleteQuest(int questID)
    {
        for (int i = 0; i < currentQuestsList.Count; i++)
        {
            if (currentQuestsList[i].Id == questID && currentQuestsList[i].progress == Quest.QuestProgress.COMPLETE)
            {
                currentQuestsList[i].progress = Quest.QuestProgress.DONE;
                currentQuestsList.Remove(currentQuestsList[i]);

                //Reward 
            }
        }
        //Check for chain quests
        CheckChainQuest(questID);
    }
    
    //Check chain quest
    public void CheckChainQuest(int questID)
    {
        int tempID = 0;
        for (int i = 0; i < questsList.Count; i++)
        {
            if (questsList[i].id == questID && questsList[i].nextQuest >0)
            {
                tempID = questsList[i].nextQuest;
            }
        }

        if (tempID > 0)
        {
            for (int i = 0; i < questsList.Count; i++)
            {
                if (questsList[i].id == questID && questsList[i].progress == Quest.QuestProgress.NOT_AVAILABLE)
                {
                   questsList[i].progress = Quest.QuestProgress.AVAILABLE;
                }
            }
        }

    }

    //Add Quest
    public void AddQuest(string questObjective, int itemAmount)
    {
        for (int i = 0; i < currentQuestsList.Count; i++)
        {
            if (currentQuestsList[i].QuestObjective == questObjective && currentQuestsList[i].progress == Quest.QuestProgress.ACCEPTED)
            {
                currentQuestsList[i].QuestObjectiveCount += itemAmount;
            }

            if (currentQuestsList[i].QuestObjectiveCount >= currentQuestsList[i].QuestObjectiveRequirement && currentQuestsList[i].progress == Quest.QuestProgress.ACCEPTED)
            {
                currentQuestsList[i].progress = Quest.QuestProgress.COMPLETE;
            }
        }
    }

    //Check 
    public bool RequestAcceptedQuest(int questID)
    {
        for (int i = 0; i < questsList.Count; i++)
        {
            if (questsList[i].Id == questID && questsList[i].progress == Quest.QuestProgress.ACCEPTED)
            {
                return true;
            }
        }
        return false;
    }
    public bool RequestAvailableQuest(int questID)
    {
        for (int i = 0; i < questsList.Count; i++)
        {
            if (questsList[i].Id == questID && questsList[i].progress == Quest.QuestProgress.AVAILABLE)
            {
                return true;
            }
        }
        return false;
    }
    public bool RequestCompleteQuest(int questID)
    {
        for (int i = 0; i < questsList.Count; i++)
        {
            if (questsList[i].Id == questID && questsList[i].progress == Quest.QuestProgress.COMPLETE)
            {
                return true;
            }
        }
        return false;
    }


    //Check 2
    public bool CheckAvailableQuest(DialogueMission NPCQuestObject)
    {
        for (int i = 0; i < questsList.Count; i++)
        {
            for (int j = 0; j < NPCQuestObject.availableQuestIDs.Count; j++)
            {
                if (questsList[i].Id == NPCQuestObject.availableQuestIDs[j] &&questsList[i].progress == Quest.QuestProgress.AVAILABLE)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool CheckAcceptedQuest(DialogueMission NPCQuestObject)
    {
        for (int i = 0; i < questsList.Count; i++)
        {
            for (int j = 0; j < NPCQuestObject.receivableQuestIDs.Count; j++)
            {
                if (questsList[i].Id == NPCQuestObject.receivableQuestIDs[j] && questsList[i].progress == Quest.QuestProgress.ACCEPTED)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool CheckCompleteQuest(DialogueMission NPCQuestObject)
    {
        for (int i = 0; i < questsList.Count; i++)
        {
            for (int j = 0; j < NPCQuestObject.receivableQuestIDs.Count; j++)
            {
                if (questsList[i].Id == NPCQuestObject.receivableQuestIDs[j] && questsList[i].progress == Quest.QuestProgress.COMPLETE)
                {
                    return true;
                }
            }
        }
        return false;
    }

}
