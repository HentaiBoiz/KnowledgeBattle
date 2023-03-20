using Newtonsoft.Json;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Card
{
    #region Enum
    public enum CardType
    {
        BattleCard,
        SupportCard,
        CheatCard
    }
    public enum CardAttribute
    {
        Algebra, //Đại số
        Geometry, //Hình học
        Chemistry, //Hóa học
        English, //Tiếng anh
        Physics, //Vật lý
        Biology, //Sinh học
    }
    #endregion

    #region Convert Variables
    //Scriptable Card sẽ được Convert qua class CardMono để dễ dàng sử dụng
    public string id;
    public string cardName;
    public int level;
    public int atk;
    public int life;
    public int timeLife; //Dành cho Support Card
    public string cardDescription;
    public string[] cardHints;

    public CardType type;
    public CardAttribute attribute;

    public Sprite cardSprite;
    public string templateColor;

    public CardEffect[] queueEffects; //Effect kích hoạt khi vào hàng đợi

    public CardEffect[] mainEffects; //Effect khi được summon, kích hoạt

    public CardEffect[] continuousEffect; //Effect kích hoạt mỗi lượt

    public CardEffect[] dropEffect; //Effect kích hoạt khi bị drop
    #endregion


    public Card()
    {

    }

    public Card(string id, string cardName, int level, int atk, int life, int timeLife, string cardDescription, string[] cardHints, CardType type, CardAttribute attribute, Sprite cardSprite, string templateColor, CardEffect[] queueEffects, CardEffect[] mainEffects, CardEffect[] continuousEffect, CardEffect[] dropEffect)
    {
        this.id = id;
        this.cardName = cardName;
        this.level = level;
        this.atk = atk;
        this.life = life;
        this.timeLife = timeLife;
        this.cardDescription = cardDescription;
        this.cardHints = cardHints;
        this.type = type;
        this.attribute = attribute;
        this.cardSprite = cardSprite;
        this.templateColor = templateColor;
        this.queueEffects = queueEffects;
        this.mainEffects = mainEffects;
        this.continuousEffect = continuousEffect;
        this.dropEffect = dropEffect;
    }

    public Card(Card card)
    {
        this.id = card.id;
        this.cardName = card.cardName;
        this.level = card.level;
        this.atk = card.atk;
        this.life = card.life;
        this.timeLife = card.timeLife;
        this.cardDescription = card.cardDescription;
        this.cardHints = card.cardHints;
        this.type = card.type;
        this.attribute = card.attribute;
        this.cardSprite = card.cardSprite;
        this.templateColor = card.templateColor;
        this.queueEffects = card.queueEffects;
        this.mainEffects = card.mainEffects;
        this.continuousEffect = card.continuousEffect;
        this.dropEffect = card.dropEffect;
    }

    public Card(SOCard soCard)
    {
        this.id = soCard.id;
        this.cardName = soCard.cardName;
        this.level = soCard.level;
        this.atk = soCard.atk;
        this.life = soCard.life;
        this.timeLife = soCard.timeLife;
        this.cardDescription = soCard.cardDescription;
        this.cardHints = soCard.cardHints;
        this.type = soCard.type;
        this.attribute = soCard.attribute;
        this.cardSprite = soCard.cardSprite;
        this.templateColor = soCard.templateColor;
        this.queueEffects = soCard.queueEffects;
        this.mainEffects = soCard.mainEffects;
        this.continuousEffect = soCard.continuousEffect;
        this.dropEffect = soCard.dropEffect;
    }

    public Card(Card card,SOCard soCard)
    {
        this.id = card.id;
        this.cardName = card.cardName;
        this.level = card.level;
        this.atk = card.atk;
        this.life = card.life;
        this.timeLife = card.timeLife;
        this.cardDescription = card.cardDescription;
        this.cardHints = card.cardHints;
        this.type = card.type;
        this.attribute = card.attribute;
        this.templateColor = card.templateColor;
        this.cardSprite = soCard.cardSprite;
        this.queueEffects = soCard.queueEffects;
        this.mainEffects = soCard.mainEffects;
        this.continuousEffect = soCard.continuousEffect;
        this.dropEffect = soCard.dropEffect;
    }

    public Card(CatalogItem cCard)
    {
        //Gán dữ liệu
        var customData = JsonConvert.DeserializeObject<Dictionary<string, string>>(cCard.CustomData.ToString());

        //Setup

        CardType type = Enum.Parse<CardType>(customData["Type"].ToString());
        CardAttribute attribute = Enum.Parse<CardAttribute>(customData["Attribute"].ToString());

        this.id = cCard.ItemId;
        this.cardName = cCard.DisplayName.ToString();
        this.level = int.Parse(customData["Level"].ToString());
        this.atk = int.Parse(customData["Atk"].ToString());
        this.life = int.Parse(customData["Life"].ToString());
        this.timeLife = int.Parse(customData["TimeLife"].ToString());
        this.cardDescription = cCard.Description.ToString();
        this.cardHints = new string[4] { customData["Hint1"].ToString(), customData["Hint2"].ToString(), customData["Hint3"].ToString(), customData["Hint4"].ToString() };
        this.type = type;
        this.cardSprite = null; //Set null trước sau này mới lấy Image về
        this.attribute = attribute;
        this.templateColor = customData["TemplateColor"].ToString();
    }

}
