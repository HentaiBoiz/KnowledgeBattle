

public class DeckCard2D : DeckEditorCard
{
    public int cardIndex = -1; //Chỉ số khi nằm trong Deck

    #region EVENT METHOD
    protected override void WhenMouseHover()
    {
        cardBorder.gameObject.SetActive(true);
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

        DeckInterfact.Instance.RemoveCardFromDeck(cardIndex, cardId);
    }

    protected override void WhenMouseExit()
    {
        cardBorder.gameObject.SetActive(false);
    }
    #endregion
}
