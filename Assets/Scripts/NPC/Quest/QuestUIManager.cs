using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestUIManager : MonoBehaviour
{
    public static QuestUIManager Instance;

    //check
    public bool questAvailable = false;
    public bool questRunning = false;
    private bool questBoardlActive = false;
    private bool questLogActive = false;

    //UI
    public GameObject questBoard;
    public GameObject questLog;

    private DialogueMission currentQuestObject;

    //questList
    public List<Quest> availableQuests = new List<Quest>();
    public List<Quest> activeQuests = new List<Quest>();

    public GameObject questBtn;
    public GameObject questLogBtn;

    private List<GameObject> questTitlesList = new List<GameObject>();

    private GameObject acceptButton;
    private GameObject giveUpButton;
    private GameObject completeButton;

    //Layout content
    public Transform questBtnSpacer1;// questBtn Available
    public Transform questBtnSpacer2;// running questBtn
    public Transform questLogBtnSpacer;// 

    //Quest Board Info
    public TextMeshProUGUI quesTitle;
    public TextMeshProUGUI questDescription;
    public TextMeshProUGUI questSummary;

    //Quest Log Info
    public TextMeshProUGUI questLogTitle;
    public TextMeshProUGUI questLogDescription;
    public TextMeshProUGUI questLogSummary;

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
        HideQuestPanel();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            questLogActive = !questLogActive;
            //show Quest Log 
            ShowQuestLogBoard();
        }
    }

    //Call from dialogue mission
    public void CheckQuests(DialogueMission dialogueMission)
    {
        currentQuestObject = dialogueMission;
        QuestManager.Instance.QuestRequest(dialogueMission);

        if ((questRunning || questAvailable) && !questBoardlActive)
        {
            //show Quest Board
            ShowQuestPanel();
        }
        else
        {
            Debug.Log("No Quests Available");
        }
    }

    public void ShowQuestPanel()
    {
        questBoardlActive = true;
        questBoard.SetActive(questBoardlActive);
        //fill data
        FillQuestButtons();
    }

    public void ShowQuestLog(Quest activeQuest)
    {
        questLogTitle.text = activeQuest.title;
        if (activeQuest.progress == Quest.QuestProgress.ACCEPTED)
        {
            questLogDescription.text = activeQuest.hint;
            questLogSummary.text = activeQuest.questObjective + " : " + activeQuest.questObjectiveCount + " / " + activeQuest.questObjectiveRequirement;
        }else if (activeQuest.progress == Quest.QuestProgress.COMPLETE)
        {
            questLogDescription.text = activeQuest.congratulation;
            questLogSummary.text = activeQuest.questObjective + " : " + activeQuest.questObjectiveCount + " / " + activeQuest.questObjectiveRequirement;
        }
    }

    public void ShowQuestLogBoard()
    {
        questLog.SetActive(true);
        if (questLogActive && !questBoardlActive)
        {
            foreach (Quest curQuest in QuestManager.Instance.currentQuestsList)
            {
                GameObject questButton = Instantiate(questLogBtn);
                QuestLogButtonScript qButton = questButton.GetComponent<QuestLogButtonScript>();

                qButton.questID = curQuest.id;
                qButton.questTitle.text = curQuest.title;

                questButton.transform.SetParent(questLogBtnSpacer, false);
                questTitlesList.Add(questButton);

            }
        }
        else if (!questLogActive && !questBoardlActive)
        {
            HideQuestLog();
        }
    }
    //Quest Log


    //Hide Quest Panel
    public void HideQuestPanel()
    {
        questBoardlActive = false;
        questAvailable = false;
        questRunning = false;

        //clear text
        quesTitle.text = "";
        questDescription.text = "";
        questSummary.text = "";

        //clear list
        availableQuests.Clear();
        activeQuests.Clear();

        //clear Button list 
        for (int i = 0; i < questTitlesList.Count; i++)
        {
            Destroy(questTitlesList[i]);
        }
        questTitlesList.Clear();

        questBoard.SetActive(questBoardlActive);
    }

    public void HideQuestLog()
    {
        questLogActive = false;

        questLogTitle.text = "";
        questLogDescription.text = "";
        questLogSummary.text = "";

        //clear Button list 
        for (int i = 0; i < questTitlesList.Count; i++)
        {
            Destroy(questTitlesList[i]);
        }
        questTitlesList.Clear();

        questLog.SetActive(questBoardlActive);

    }

    //Fill buttons for quest panel
    public void FillQuestButtons()
    {
        foreach (Quest availableQuest in availableQuests)
        {
            GameObject questButton = Instantiate(questBtn);
            QuestButtonScript questButtonScript = questButton.GetComponent<QuestButtonScript>();

            questButtonScript.questID = availableQuest.id;
            questButtonScript.questTitle.text = availableQuest.title;

            questButton.transform.SetParent(questBtnSpacer1,false);

            questTitlesList.Add(questButton);
        }

        foreach (Quest activeQuest in activeQuests)
        {
            GameObject questButton = Instantiate(questBtn);
            QuestButtonScript questButtonScript = questButton.GetComponent<QuestButtonScript>();

            questButtonScript.questID = activeQuest.id;
            questButtonScript.questTitle.text = activeQuest.title;

            questButton.transform.SetParent(questBtnSpacer2, false);
            questTitlesList.Add(questButton);
        }
    }

    //show quest description in quest board
    public void ShowDescriptionQuest(int questID)
    {
        for (int i = 0; i < availableQuests.Count; i++)
        {
            if(availableQuests[i].Id == questID)
            {
                quesTitle.text = availableQuests[i].title;
                if (availableQuests[i].progress == Quest.QuestProgress.AVAILABLE)
                {
                    questDescription.text = availableQuests[i].des;
                    questSummary.text = availableQuests[i].questObjective + ": " + availableQuests[i].questObjectiveCount + "/" + availableQuests[i].questObjectiveRequirement;
                }
            }
        }

        for (int i = 0; i < activeQuests.Count; i++)
        {
            if (activeQuests[i].Id == questID)
            {
                quesTitle.text = activeQuests[i].title;
                if (activeQuests[i].progress == Quest.QuestProgress.ACCEPTED)
                {
                    questDescription.text = activeQuests[i].hint;
                    questSummary.text = activeQuests[i].questObjective + ": " + activeQuests[i].questObjectiveCount + "/" + activeQuests[i].questObjectiveRequirement;
                }else if (activeQuests[i].progress == Quest.QuestProgress.COMPLETE)
                {
                    questDescription.text = activeQuests[i].congratulation;
                }
            }
        }
    }
}

