using Photon.Pun;
using UnityEngine;

public class HandPopup : MonoBehaviour
{
    public Transform queueBtn;
    public Transform attachBtn;
    public Transform activateSPBtn;
    public Transform setBtn;

    public Card curentSelectCard = null; //Khi chọn 1 Card trên tay, thông tin của card đó sẽ được truyền vào
    public int handIndex = -1; //Khi chọn 1 Card trên tay, index của card đó trên tay sẽ được truyền vào

    private void OnEnable()
    {
        DisableAllBtn();

        if (PhotonNetwork.IsMasterClient)
        {
            EnableBtnCondition(0);
        }
        else
        {
            EnableBtnCondition(1);
        }
    }

    private void EnableBtnCondition(int side)
    {
        try
        {
            if (curentSelectCard == null)
                return;

            //Queue BTN
            if (!Field_Manager_Id.Instance.queueZoneIsFull(side))
            {
                if (curentSelectCard.type == Card.CardType.BattleCard)
                    queueBtn.gameObject.SetActive(true);
            }

            //Attach BTN
            if (!Question_Manager_Id.Instance.isAnswerSlotFull(side))
            {
                attachBtn.gameObject.SetActive(true);
            }

            //Activate SP BTN
            if (!Field_Manager_Id.Instance.queueZoneIsFull(side))
            {
                if (curentSelectCard.type == Card.CardType.SupportCard)
                    activateSPBtn.gameObject.SetActive(true);
            }

            //Cheat Card thì sẽ không phân biệt là loại bài nào
            setBtn.gameObject.SetActive(true);
        }
        catch
        {
        }
        
    }

    private void DisableAllBtn()
    {
        queueBtn.gameObject.SetActive(false);
        attachBtn.gameObject.SetActive(false);
        activateSPBtn.gameObject.SetActive(false);
        setBtn.gameObject.SetActive(false);
    }
}
