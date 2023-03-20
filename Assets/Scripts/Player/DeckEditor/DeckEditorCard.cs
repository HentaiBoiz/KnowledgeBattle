using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static Card;
using UnityEngine.EventSystems;

public class DeckEditorCard : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public string cardId;

    #region CARD UI
    public TextMeshProUGUI nameText;
    public Transform levelStones;
    public TextMeshProUGUI atkText;
    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI timeLifeText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI[] cardHintsText = new TextMeshProUGUI[4];

    public Image cardTemplate;
    public Image cardImage;
    public Image cardIcon;

    public Transform cardBorder;
    public Transform cardBack;
    #endregion


    public void OnPointerEnter(PointerEventData eventData)
    {
        WhenMouseHover();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            WhenLeftMouseClick();
        }
        else if(eventData.button == PointerEventData.InputButton.Right)
        {
            WhenRightMouseClick();
        }
       
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        WhenMouseExit();
    }

    #region SETUP METHOD
    public virtual void SetupCardUI(Card card)
    {
        cardBorder.gameObject.SetActive(false);
        cardId = card.id;

        //Card Type
        switch (card.type)
        {
            case CardType.BattleCard:
                BattleCardSetup(card);
                break;
            case CardType.SupportCard:
                SupportCardSetup(card);
                break;
            case CardType.CheatCard:
                CheatCardSetup(card);
                break;
        }

    }

    public void BattleCardSetup(Card cardMono)
    {
        //Set Text UI
        nameText.text = "" + cardMono.cardName;
        atkText.text = "" + cardMono.atk;
        lifeText.text = "" + cardMono.life;
        timeLifeText.text = "";
        descriptionText.text = "" + cardMono.cardDescription;

        //Level Stone
        levelStones.gameObject.SetActive(true);

        for (int i = 0; i < 9; i++)
        {
            if (i < cardMono.level)
                levelStones.GetChild(i).gameObject.SetActive(true);
            else
                levelStones.GetChild(i).gameObject.SetActive(false);

        }
        //Card Hints
        for (int j = 0; j < cardHintsText.Length; j++)
        {
            cardHintsText[j].text = cardMono.cardHints[j];
        }

        cardImage.sprite = Resources.Load<Sprite>($"CardImg/BattleCard/{cardMono.id}"); 

        //Card Template
        cardTemplate.sprite = Resources.Load<Sprite>("Sprites/battlecard"); 

        
        Color newColor;
        ColorUtility.TryParseHtmlString("#" + cardMono.templateColor, out newColor);
        cardTemplate.color = newColor;

        //Card Icon
        cardIcon.gameObject.SetActive(true);
        CardIconSet cardIconSet = new CardIconSet();
        cardIcon.sprite = cardIconSet.ReturnCardIcon(cardMono.attribute);
    }

    public void SupportCardSetup(Card cardMono)
    {
        //Set Text UI
        nameText.text = "" + cardMono.cardName;
        atkText.text = "";
        lifeText.text = "";
        timeLifeText.text = "" + cardMono.timeLife;
        descriptionText.text = "" + cardMono.cardDescription;

        //Level Stone
        levelStones.gameObject.SetActive(false);

        //Card Hints
        for (int j = 0; j < cardHintsText.Length; j++)
        {
            cardHintsText[j].text = cardMono.cardHints[j];
        }

        cardImage.sprite = cardImage.sprite = Resources.Load<Sprite>($"CardImg/SupportCard/{cardMono.id}");

        //Card Template
        cardTemplate.sprite = Resources.Load<Sprite>("Sprites/supportcard");

        Color newColor;
        ColorUtility.TryParseHtmlString("#" + cardMono.templateColor, out newColor);
        cardTemplate.color = newColor;

        //Card Icon
        cardIcon.gameObject.SetActive(false);
    }

    public void CheatCardSetup(Card cardMono)
    {
        //Set Text UI
        nameText.text = "" + cardMono.cardName;
        atkText.text = "";
        lifeText.text = "";
        timeLifeText.text = "";
        descriptionText.text = "" + cardMono.cardDescription;

        //Level Stone
        levelStones.gameObject.SetActive(false);

        //Card Hints
        for (int j = 0; j < cardHintsText.Length; j++)
        {
            cardHintsText[j].text = cardMono.cardHints[j];
        }

        cardImage.sprite = cardImage.sprite = Resources.Load<Sprite>($"CardImg/CheatCard/{cardMono.id}");

        //Card Template
        cardTemplate.sprite = Resources.Load<Sprite>("Sprites/cheatcard");

        Color newColor;
        ColorUtility.TryParseHtmlString("#" + cardMono.templateColor, out newColor);
        cardTemplate.color = newColor;

        //Card Icon
        cardIcon.gameObject.SetActive(false);
    }



    #endregion

    #region EVENT METHOD
    protected virtual void WhenMouseHover()
    {
        cardBorder.gameObject.SetActive(true);
    }

    protected virtual void WhenLeftMouseClick()
    {
        
    }

    protected virtual void WhenRightMouseClick()
    {

    }

    protected virtual void WhenMouseExit()
    {
        cardBorder.gameObject.SetActive(false);
    }
    #endregion
}
