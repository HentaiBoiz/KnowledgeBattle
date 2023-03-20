using UnityEngine;
using TMPro;
using static Card;

public class ThisCard : MonoBehaviour
{
    [Header("Card Info")]
    public string cardId;
    public string cardOwner; //Gắn Id Player nào mà sỡ hữu lá bài này
    public Card cardMono = null;


    //================Card UI==================================
    [Header("Card UI")]
    #region CARD UI
    public TextMeshPro nameText;
    public Transform levelStones;
    public TextMeshPro atkText;
    public TextMeshPro lifeText;
    public TextMeshPro timeLifeText;
    public TextMeshPro descriptionText;
    public TextMeshPro[] cardHintsText = new TextMeshPro[4];

    public SpriteRenderer cardTemplate;
    public SpriteRenderer cardImage;
    public SpriteRenderer cardIcon;

    public Transform cardBack;
    public Transform cardBorder;

    #endregion

    [Header("Default Variable")]
    [HideInInspector]
    public int defaultAtk;
    [HideInInspector]
    public int defaultLife;
    [HideInInspector]
    public int defaultLifeTime;

    [Header("Card State")]

    public bool isGlobalBack; //True: Nếu như lá bài này ko ai có thể nhìn thấy
    public bool isEnemyBack; //True : Nếu như lá bài này chỉ có Owner nhìn thấy



    // Start is called before the first frame update
    void Start()
    {
        cardMono = null;
        //SetupCard(cardId);
    }


    public void SetupCard(string cardId)
    {
        Card tempCard = CardDatabase.Instance.FindCardWithId(cardId);

        if (tempCard == null)
        {
            Debug.LogError("Can't find Card with Id: " + cardId);
            return;
        }

        //Nếu chưa có lá bài nào thì mới setup mới, không thì vẫn giữ lại atk, def
        if (cardMono == null || cardMono.id == "")
        {
            cardMono = new Card(tempCard);

            //Các biến default để so sánh màu sắc
            defaultAtk = cardMono.atk;
            defaultLife = cardMono.life;
            defaultLifeTime = cardMono.timeLife;
        }
        else
        {
            tempCard.atk = cardMono.atk;
            tempCard.life = cardMono.life;
            tempCard.timeLife = cardMono.timeLife;

            cardMono = new Card(tempCard);
        }


        //Card Type
        switch (cardMono.type)
        {
            case CardType.BattleCard:
                BattleCardSetup(cardMono);
                break;
            case CardType.SupportCard:
                SupportCardSetup(cardMono);
                break;
            case CardType.CheatCard:
                CheatCardSetup(cardMono);
                break;
        }

    }

    public void SetupCardPlayfabDB(string cardId)
    {
        Card tempCard = PlayfabCardDB.Instance.FindCardById(cardId);

        if (tempCard == null)
        {
            Debug.LogError("Can't find Card with Id: " + cardId);
            return;
        }

        //Nếu chưa có lá bài nào thì mới setup mới, không thì vẫn giữ lại atk, def
        if (cardMono == null || cardMono.id == "")
        {
            cardMono = new Card(tempCard);

            //Các biến default để so sánh màu sắc
            defaultAtk = cardMono.atk;
            defaultLife = cardMono.life;
            defaultLifeTime = cardMono.timeLife;
        }
        else
        {
            tempCard.atk = cardMono.atk;
            tempCard.life = cardMono.life;
            tempCard.timeLife = cardMono.timeLife;

            cardMono = new Card(tempCard);
        }


        //Card Type
        switch (cardMono.type)
        {
            case CardType.BattleCard:
                BattleCardSetup(cardMono);
                break;
            case CardType.SupportCard:
                SupportCardSetup(cardMono);
                break;
            case CardType.CheatCard:
                CheatCardSetup(cardMono);
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
        CardIconSet cardIconSet = new CardIconSet();
        cardIcon.sprite = cardIconSet.ReturnCardIcon(cardMono.attribute);
        
    }

    public void SupportCardSetup(Card cardMono)
    {
        //Set Text UI
        nameText.text = "" + cardMono.cardName;
        atkText.text = "" ;
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
        cardIcon.sprite = null;

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
        cardIcon.sprite = null;
    }

    //======================UI==========================

    public void SetEnemyBack() //Dấu khỏi Enemy
    {
        isEnemyBack = true;
        cardBack.gameObject.SetActive(true);
    }

    public void UnsetEnemyBack() //Bỏ dấu khỏi Enemy
    {
        isEnemyBack = false;
        cardBack.gameObject.SetActive(false);
    }

    public void OnCardBorder() //Dấu khỏi Enemy
    {
        cardBorder.gameObject.SetActive(true);
    }

    public void OffCardBorder() //Bỏ dấu khỏi Enemy
    {
        cardBorder.gameObject.SetActive(false);
    }
}
