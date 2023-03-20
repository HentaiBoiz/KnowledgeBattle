using TMPro;
using UnityEngine;

public class InventoryCard : DeckEditorCard
{
    public int cardCount;
    public TextMeshProUGUI cardCountTxt;

    private void Update()
    {
        if (DeckEditorCardDB.Instance.cardCountCalculate.ContainsKey(cardId))
        {
            cardCountTxt.text = (cardCount + DeckEditorCardDB.Instance.cardCountCalculate[cardId]).ToString();
        }
    }

    public override void SetupCardUI(Card card)
    {
        base.SetupCardUI(card);

    }

    protected override void WhenLeftMouseClick()
    {
        base.WhenLeftMouseClick();
        CardDetail.Instance.ShowCardDetail(PlayfabCardDB.Instance.FindCardById(cardId));

    }

    protected override void WhenRightMouseClick()
    {
        base.WhenRightMouseClick();
        CardDetail.Instance.ShowCardDetail(PlayfabCardDB.Instance.FindCardById(cardId));

        if(int.Parse(cardCountTxt.text) > 0)
        {
            DeckInterfact.Instance.AddCardToDeck(cardId);
        }
        else
        {
            Debug.Log("You don't have any of this card in your inventory");
        }


    }
}
