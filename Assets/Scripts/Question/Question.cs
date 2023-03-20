using Newtonsoft.Json;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Card;

[System.Serializable]
public class Question 
{
    public string questionId;
    public CardAttribute questionAttribute;
    public int questionLevel; //Cấp độ của câu hỏi
    public Sprite mainQuestion; //Câu hỏi chính
    public string[] realAnswers; //Câu trả lời đúng

    //Another
    public string[] currentAnswer; //Câu trả lời hiện tại Player đang attach vào
    public List<Card> currentCardAttach;

    public Question()
    {

    }


    public Question(SOQuestion sOQuestion)
    {
        this.questionId = sOQuestion.questionId;
        this.questionAttribute = sOQuestion.questionAttribute;
        this.questionLevel = sOQuestion.questionLevel;
        this.mainQuestion = sOQuestion.mainQuestion;
        this.realAnswers = sOQuestion.realAnswers;
        this.currentAnswer = new string[sOQuestion.realAnswers.Length];
        this.currentCardAttach = new List<Card>();
    }

    public Question(string questionId, CardAttribute questionAttribute, int questionLevel, Sprite mainQuestion, string[] realAnswers)
    {
        this.questionId = questionId;
        this.questionAttribute = questionAttribute;
        this.questionLevel = questionLevel;
        this.mainQuestion = mainQuestion;
        this.realAnswers = realAnswers;
        this.currentAnswer = new string[realAnswers.Length];
        this.currentCardAttach = new List<Card>();
    }

    public Question(Question question)
    {
        this.questionId = question.questionId;
        this.questionAttribute = question.questionAttribute;
        this.questionLevel = question.questionLevel;
        this.mainQuestion = question.mainQuestion;
        this.realAnswers = question.realAnswers;
        this.currentAnswer = new string[realAnswers.Length];
        this.currentCardAttach = new List<Card>();
    }

    public Question(CatalogItem cQuestion)
    {
        //Gán dữ liệu
        var customData = JsonConvert.DeserializeObject<Dictionary<string, string>>(cQuestion.CustomData.ToString());

        CardAttribute attribute = Enum.Parse<CardAttribute>(cQuestion.ItemClass.ToString());

        this.questionId = cQuestion.ItemId;
        this.questionAttribute = attribute;
        this.questionLevel = int.Parse(cQuestion.DisplayName);
        this.mainQuestion = null;

        //Assign Real Answer

        List<string> rAnswer = new List<string>();
        for (int i = 0; i < 4; i++) //Hiện tại 1 câu hỏi chỉ có tối đa 4 input/real answers
        {
            if (customData.ContainsKey($"input{i + 1}"))
            {
                rAnswer.Add(customData[$"input{i + 1}"]);
            }
        }
        this.realAnswers = rAnswer.ToArray();

        this.currentAnswer = new string[realAnswers.Length];
        this.currentCardAttach = new List<Card>();
    }
}
