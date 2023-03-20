using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Card;

public class CardUI_VFX : MonoBehaviour
{
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

    public virtual void ShowCardDetail(string cardId)
    {
        Card card = CardDatabase.Instance.FindCardWithId(cardId);

        cardBack.gameObject.SetActive(false);


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

        cardImage.sprite = cardMono.cardSprite;

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

        cardImage.sprite = cardMono.cardSprite;

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

        cardImage.sprite = cardMono.cardSprite;

        //Card Template
        cardTemplate.sprite = Resources.Load<Sprite>("Sprites/cheatcard");

        Color newColor;
        ColorUtility.TryParseHtmlString("#" + cardMono.templateColor, out newColor);
        cardTemplate.color = newColor;

        //Card Icon
        cardIcon.gameObject.SetActive(false);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
