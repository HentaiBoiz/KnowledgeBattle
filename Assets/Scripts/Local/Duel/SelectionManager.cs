using UnityEngine;
using static TurnManager;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance;

    #region POPUP PANEL
    public HandPopup handPopup;
    public BattlePopup battlePopup;
    public ResourceSelection resourceSelection;
    public QuestionPanelUI questionPanelUI;
    public CheatPopup cheatPopup;
    #endregion

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        DisableAllPanel();
    }


    public void DisableAllPanel()
    {
        handPopup.gameObject.SetActive(false);
        battlePopup.gameObject.SetActive(false);
        resourceSelection.gameObject.SetActive(false);
        questionPanelUI.gameObject.SetActive(false);
        cheatPopup.gameObject.SetActive(false);
    }

    #region OPEN PANEL
    public void OpenHandPanel(Card card, int handIndex)
    {
        if (TurnManager.Instance.currentTurn != TurnManager.Instance.localSide)
            return;

        if (TurnManager.Instance.currentStep != TurnStep.ReadyStep && TurnManager.Instance.currentStep != TurnStep.BonusStep)
            return;

        //if (Field_Manager_Id.Instance.queueZoneIsFull(TurnManager.Instance.localSide))
        //    return;

        handPopup.curentSelectCard = card;
        handPopup.handIndex = handIndex;
        handPopup.gameObject.SetActive(true);
    }
    public void HideHandPanel()
    {

        handPopup.gameObject.SetActive(false);
        handPopup.curentSelectCard = null;
        handPopup.handIndex = -1;
    }

    public void OpenBattlePanel(int side, int battleIndex)
    {
        if (TurnManager.Instance.currentTurn != TurnManager.Instance.localSide)
            return;


        if (TurnManager.Instance.currentStep != TurnStep.BattleStep)
            return;

        battlePopup.gameObject.SetActive(true);
        battlePopup.battleZoneIndex = battleIndex;
        battlePopup.side = side;
    }

    public void HideBattlePanel()
    {
        battlePopup.gameObject.SetActive(false);
        battlePopup.battleCard = null;
        battlePopup.battleZoneIndex = -1;
        battlePopup.side = -1;
    }

    public void ShowResourcesSelection(int index)
    {
        resourceSelection.gameObject.SetActive(true);
        resourceSelection.Setup(handPopup.curentSelectCard, index);
    }

    public void HideResourcesSelection()
    {
        resourceSelection.gameObject.SetActive(false);
        StateManager.Instance.DuelState = StateManager.ActionState.normalState;
    }

    public void OpenQuestionPanelUI(int thisSide, int thisIndex)
    {
        questionPanelUI.SetupQuestionUI(thisSide, thisIndex);
        questionPanelUI.gameObject.SetActive(true);
    }

    public void OpenCheatPanel(CardInCheat card, int handIndex)
    {
        //Không được kích hoạt trong lượt mình
        if (TurnManager.Instance.currentTurn == TurnManager.Instance.localSide)
            return;

        //if (TurnManager.Instance.currentStep != TurnStep.ReadyStep && TurnManager.Instance.currentStep != TurnStep.BonusStep)
        //    return;

        ////if (Field_Manager_Id.Instance.queueZoneIsFull(TurnManager.Instance.localSide))
        ////    return;

        cheatPopup.gameObject.SetActive(true);
    }
    public void HideCheatPanel()
    {

        cheatPopup.gameObject.SetActive(false);

    }
    #endregion


}
