using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Card;

public class QuestionDatabase : MonoBehaviour
{
    public static QuestionDatabase Instance { get; private set; }

    //Question Catalog
    public Dictionary<string, CatalogItem> questionCatalogItemsDB = new Dictionary<string, CatalogItem>();
    //public List<SOQuestion> soQuestionsDatabase = new List<SOQuestion>(); //List chứ mảng các lá bài dạng Scriptable Object

    #region QUESTION ATTRIBUTE ARRAY
    public List<Question> questionsAlgebraDB = new List<Question>();
    public List<Question> questionsChemistryDB = new List<Question>();

    #endregion


    private void Awake()
    {

        Instance = this;

    }

    //Tìm câu hỏi dựa theo id và thuộc tính
    public Question FindQuestionWithId(string id, string attribute)
    {
        CardAttribute questionAttribute;
        Question temp = null;

        if(Enum.TryParse<CardAttribute>(attribute, out questionAttribute))
        {
            switch (questionAttribute)
            {
                case CardAttribute.Algebra:
                    temp = FindQuestion(id, questionsAlgebraDB);
                    break;
                case CardAttribute.Chemistry:
                    temp = FindQuestion(id, questionsChemistryDB);
                    break;
            }

        }

        return new Question(temp);
    }

    //Tìm câu hỏi dựa theo id (biết sẵn thuộc tính)
    public Question FindQuestion(string id, List<Question> db)
    {
        foreach (var item in db)
        {
            if (id == item.questionId)
                return new Question(item); 
        }
        return null;
    }

    //Tìm random câu hỏi dựa theo level và thuộc tính
    public Question RandomQuestionWithConditions(int level, string attribute)
    {
        List<Question> conditionDb = new List<Question>();
        CardAttribute questionAttribute;

        if (Enum.TryParse<CardAttribute>(attribute, out questionAttribute))
        {
            switch (questionAttribute)
            {
                case CardAttribute.Algebra:
                    conditionDb = TakeAllQuestionWithLv(level, questionsAlgebraDB);
                    break;
                case CardAttribute.Chemistry:
                    conditionDb = TakeAllQuestionWithLv(level, questionsChemistryDB);
                    break;
            }

        }

        int rnd = UnityEngine.Random.Range(0, conditionDb.Count);


        return new Question(conditionDb[rnd]);
    }

    //Tìm random câu hỏi dựa theo level (biết sẵn thuộc tính)
    public List<Question> TakeAllQuestionWithLv(int level, List<Question> db)
    {
        List<Question> conditionDb = new List<Question>();

        foreach (var item in db)
        {
            if (item.questionLevel == level)
                conditionDb.Add(item);
        }

        return conditionDb;
    }


    #region PLAYFAB METHOD
    public void StartLoadQuestionCatalog(List<string> deckIds)
    {
        List<int> tempLevel = new List<int>(); //Các Level đã Load Question

        foreach (var id in deckIds)
        {
            int cardLvl = PlayfabCardDB.Instance.ReturnCardLvById(id);

            if (!tempLevel.Contains(cardLvl) && cardLvl > 0)
                tempLevel.Add(cardLvl);
        }
        Debug.Log("Temp Level: " + tempLevel.Count);

        foreach (var lv in tempLevel)
        {
            LoadQuestionFromPlayfab(lv);
        }
    }

    public void LoadQuestionFromPlayfab(int level)
    {
        Debug.Log("START LOAD");

        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest
        {
            CatalogVersion = $"Question_lv{level}"
        },
        result => {
            foreach (CatalogItem item in result.Catalog)
            {

                questionCatalogItemsDB.Add(item.ItemId, item);
                //Phân loại câu hỏi vào Database
                ClassifyQuestions(item);

            }
            Debug.Log($"LOAD QUESTION WITH LEVEL {level} SUCCESS");
        },
        error => {
            Debug.Log(error.GenerateErrorReport());
            ErrorsManager.Instance.PushError(error.GenerateErrorReport());
        });
    }
    #endregion

    #region LOCAL METHOD
    public void ClassifyQuestions(CatalogItem catalogQuestion)
    {
        //Convert Catalog to Question
        Question tempQuestion = new Question(catalogQuestion);

        switch (tempQuestion.questionAttribute)
        {
            case CardAttribute.Algebra:
                questionsAlgebraDB.Add(new Question(tempQuestion));
                break;
            case CardAttribute.Chemistry:
                questionsChemistryDB.Add(new Question(tempQuestion));
                break;
        }
    }
    #endregion
}
