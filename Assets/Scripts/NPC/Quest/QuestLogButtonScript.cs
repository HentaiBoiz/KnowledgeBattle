using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestLogButtonScript : MonoBehaviour
{
    public int questID;
    public TextMeshProUGUI questTitle;

    public void ShowInfo()
    {
        //QuestUIManager.Instance.ShowQuestLog(questID);
        QuestManager.Instance.ShowQuestLog(questID);
    }

    public void CloseLogQuest()
    {
        QuestUIManager.Instance.HideQuestLog();
    }
}
