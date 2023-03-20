using Photon.Pun;
using UnityEngine;
using ExitGames.Client.Photon;

public class PhotonEventManager : MonoBehaviour
{
    protected void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += this.EventReceived;
    }

    protected void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= this.EventReceived;
    }

    private void EventReceived(EventData obj)
    {
        switch (obj.Code)
        {
            case (byte)EventCode.onUpdateDeck:
                UpdateDeckUI();
                break;
            case (byte)EventCode.onUpdateHand:
                UpdateHandUI();
                UpdateDeckUI();
                break;
            case (byte)EventCode.onUpdateQueue:
                UpdateHandUI();
                UpdateQueueUI();
                break;
            case (byte)EventCode.onUpdateQuestion:
                UpdateQuestionUI();
                break;
            case (byte)EventCode.onUpdateBattle:
                UpdateBattleZoneUI();
                break;
            case (byte)EventCode.onUpdateDrop:
                UpdateDropZoneUI();
                break;
            case (byte)EventCode.onLoadCardImg:
                StartSetupCardDatabase();
                break;
            case (byte)EventCode.onUpdateCheat:
                UpdateCheatZoneUI();
                break;
        }
    }

    #region EVENT ACTION
    private void UpdateDeckUI()
    {
        Field_Manager_Id.Instance.isUpdateDeck = true;
    }

    private void UpdateHandUI()
    {
        Field_Manager_Id.Instance.isUpdateHand = true;
    }

    private void UpdateQueueUI()
    {
        Field_Manager_Id.Instance.isUpdateQueue = true;
    }

    private void UpdateQuestionUI()
    {
        Question_Manager_Id.Instance.isUpdateQuestion = true;
    }

    private void UpdateBattleZoneUI()
    {
        Field_Manager_Id.Instance.isUpdateBattle = true;
    }

    private void UpdateDropZoneUI()
    {
        Field_Manager_Id.Instance.isUpdateDrop = true;
    }

    private void StartSetupCardDatabase()
    {
        //Checking
        foreach (var item in PhotonDuelistStats.Instance.duelists)
        {
            if (item.deckCardIds.Length == 0 || item.deckCardIds == null) //Chưa Load xong Deck
                return;
        }

        CardDatabase.Instance.StartSetupCard();
    }

    private void UpdateCheatZoneUI()
    {
        Field_Manager_Id.Instance.isUpdateCheat = true;
    }
    #endregion
}
