using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TurnManager;

public class StepsManager : MonoBehaviour
{
    public Color playerColor;
    public Color opponentColor;

    public SpriteRenderer[] steps;

    Color currentColor;

    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
            transform.localScale = new Vector3(-1f, 1f, -1f);
    }

    private void Update()
    {
        if (Field_Manager_Id.Instance.isDuelStart == false)
            return;

        switch (TurnManager.Instance.currentTurn)
        {
            case 0:
                if (PhotonNetwork.IsMasterClient)
                {
                    currentColor = playerColor;
                }
                else
                {
                    currentColor = opponentColor;
                }
                break;
            case 1:
                if (PhotonNetwork.IsMasterClient)
                {
                    currentColor = opponentColor;
                }
                else
                {
                    currentColor = playerColor;
                }
                break;
        }

        switch (TurnManager.Instance.currentStep)
        {
            case TurnStep.DrawStep:
                SetStepOn(0, currentColor);
                break;
            case TurnStep.ReadyStep:
                SetStepOn(1, currentColor);
                break;
            case TurnStep.BattleStep:
                SetStepOn(2, currentColor);
                break;
            case TurnStep.BonusStep:
                SetStepOn(3, currentColor);
                break;
            case TurnStep.EndStep:
                SetStepOn(4, currentColor);
                break;
        }
    }

    public void SetStepOn(int index, Color color)
    {
        foreach (var step in steps)
        {
            step.color = Color.white;
        }

        steps[index].color = color;
    }
}
