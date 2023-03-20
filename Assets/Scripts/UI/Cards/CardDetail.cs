using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Card;

public class CardDetail : MonoBehaviour
{
    public static CardDetail Instance; //Singleton

    [HideInInspector]
    public ThisCard thisCard;

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

    public Transform cardBack;

    public TextMeshProUGUI titleTxt;
    public TextMeshProUGUI titleName;
    public TextMeshProUGUI titleDescrip;
    #endregion

    private void Awake()
    {
        Instance = this;

        cardBack.gameObject.SetActive(true);

    }

    public virtual void ShowCardDetail(Card card)
    {
        cardBack.gameObject.SetActive(false);


        //Card Type
        switch (card.type)
        {
            case CardType.BattleCard:
                BattleCardSetup(card);

                titleTxt.text = card.attribute.ToString() + " - Level: " + card.level.ToString();
                titleDescrip.text = card.cardDescription.ToString() + "<br><br>ATK: " + card.atk + " - LIFE: " + card.life;
                break;
            case CardType.SupportCard:
                SupportCardSetup(card);

                titleTxt.text = "Support Card";
                titleDescrip.text = card.cardDescription.ToString() + "<br><br>TIME LIFE: " + card.timeLife;
                break;
            case CardType.CheatCard:
                CheatCardSetup(card);

                titleTxt.text = "Cheat Card";
                titleDescrip.text = card.cardDescription.ToString();
                break;
        }

        titleName.text = card.cardName.ToString();
    }

    public virtual void ShowCardDetail(string cardId)
    {
        Card card = CardDatabase.Instance.FindCardWithId(cardId);

        cardBack.gameObject.SetActive(false);

        //Card Type
        switch (card.type)
        {
            case CardType.BattleCard:
                BattleCardSetup(card);

                titleTxt.text = card.attribute.ToString() + " - Level: " + card.level.ToString();
                titleDescrip.text = card.cardDescription.ToString() + "<br><br>ATK: " + card.atk + " - LIFE: " + card.life;
                break;
            case CardType.SupportCard:
                SupportCardSetup(card);

                titleTxt.text = "Support Card";
                titleDescrip.text = card.cardDescription.ToString() + "<br><br>TIME LIFE: " + card.timeLife;
                break;
            case CardType.CheatCard:
                CheatCardSetup(card);

                titleTxt.text = "Cheat Card";
                titleDescrip.text = card.cardDescription.ToString();
                break;
        }

        titleName.text = card.cardName.ToString();
    }

    public virtual void HideCardDetail()
    {
        titleTxt.text = "";
        titleName.text = "";
        titleDescrip.text = "";

        if(cardBack.gameObject)
            cardBack.gameObject.SetActive(true);
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

        cardImage.sprite = cardImage.sprite = Resources.Load<Sprite>($"CardImg/BattleCard/{cardMono.id}");

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


}
