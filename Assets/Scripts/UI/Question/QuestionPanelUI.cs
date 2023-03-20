using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Photon.Pun;
using System.Collections.Generic;


public class QuestionPanelUI : MonoBehaviour
{
    public Image mainQuestionImg;
    public TextMeshProUGUI currentAnswerTxt;

    public Button solveBtn;

    public List<GameObject> hintIcons;

    //Sửa lỗi Sprite của Question bị trắng vì chưa load xong
    int tempSide = -1;
    int tempQIndex = -1;


    private void Update()
    {

        if (tempSide >= 0 && tempQIndex >= 0)
        {
            mainQuestionImg.sprite = Question_Manager_Id.Instance.questionsArrays[tempSide].fieldQuestion[tempQIndex].mainQuestion;
        }
    }

    private void OnDisable()
    {
        ResetTempVariables();
    }

    public void SetupQuestionUI(int side, int qIndex)
    {
        Question question = Question_Manager_Id.Instance.questionsArrays[side].fieldQuestion[qIndex];

        //Set Img
        tempSide = side;
        tempQIndex = qIndex;

        solveBtn.interactable = false;

        string temp = "";

        HideIcons();

        for (int i = 0; i < question.realAnswers.Length; i++)
        {
            hintIcons[i].SetActive(true);

            temp += $"{question.currentAnswer[i]}<br>";
        }

        currentAnswerTxt.text = temp;

        //Kiểm tra để không mở nhầm Question của đối thủ
        if (PhotonNetwork.IsMasterClient)
        {
            if (side == 1)
                return;
        }
        else
        {
            if (side == 0)
                return;
        }
        //Kiểm tra để chỉ Solve Question trong bước Ready và Bonus 
        if (TurnManager.Instance.currentStep != TurnManager.TurnStep.ReadyStep && TurnManager.Instance.currentStep != TurnManager.TurnStep.BonusStep)
            return;
        //Kiểm tra để chỉ Solve Question trong lượt của mình
        if (TurnManager.Instance.currentTurn != TurnManager.Instance.localSide)
            return;
        //Kiểm tra để chỉ Solve Question trong normal state
        if (StateManager.Instance.DuelState != StateManager.ActionState.normalState)
            return;
        //Chỉ được Solve Question khi còn lượt Push
        if (DuelRuleManager.Instance.wasPush == true)
            return;


        int battleZoneId = Field_Manager_Id.Instance.CheckBattleZoneEmpty(side);
        //Enable Btn
        if (question.realAnswers.Length == question.currentCardAttach.Count && battleZoneId != -1)
        {

            solveBtn.interactable = true;

            solveBtn.onClick.RemoveAllListeners();

            solveBtn.onClick.AddListener(() =>
            {
                Question_Manager_Id.Instance.SolveQuestion(side, qIndex);

                this.gameObject.SetActive(false);
            });
        }
    }

    public void HideIcons()
    {
        foreach (var item in hintIcons)
        {
            item.SetActive(false);
        }
    }

    public void ResetTempVariables()
    {
        tempSide = -1;
        tempQIndex = -1;
    }
    
}
