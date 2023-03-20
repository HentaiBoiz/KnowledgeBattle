using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class ResourceSelection : MonoBehaviour
{
    public int resourceIndex = -1;

    public Button[] hintBtns;

    public Button selectBtn;

    public int questionZoneIndex = -1;

    private void Update()
    {
        if (resourceIndex == -1)
        {
            selectBtn.interactable = false;
        }
        else
        {
            selectBtn.interactable = true;
        }
    }

    public void SelectIndex0()
    {
        resourceIndex = 0;
    }
    public void SelectIndex1()
    {
        resourceIndex = 1;
    }
    public void SelectIndex2()
    {
        resourceIndex = 2;
    }
    public void SelectIndex3()
    {
        resourceIndex = 3;
    }

    public void Setup(Card card, int index)
    {
        //4 hint
        for (int i = 0; i < 4; i++)
        {
            hintBtns[i].GetComponentInChildren<TextMeshProUGUI>().text = card.cardHints[i];
        }

        questionZoneIndex = index;
    }

    //Attach vào ô Queue
    public void AttachBtn()
    {
        if(PhotonNetwork.IsMasterClient)
            Question_Manager_Id.Instance.AttachACard(0, questionZoneIndex, SelectionManager.Instance.handPopup.handIndex, hintBtns[resourceIndex].GetComponentInChildren<TextMeshProUGUI>().text);
        else
            Question_Manager_Id.Instance.AttachACard(1, questionZoneIndex, SelectionManager.Instance.handPopup.handIndex, hintBtns[resourceIndex].GetComponentInChildren<TextMeshProUGUI>().text);

    }

    public void CancelBtn()
    {
        SelectionManager.Instance.HideResourcesSelection();
    }
}
