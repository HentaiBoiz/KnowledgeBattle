using UnityEngine;
using static Card;

[CreateAssetMenu(fileName = "Card", menuName = "Scriptale Object/Card")]
public class SOCard : ScriptableObject
{

    public string id;
    public string cardName;
    public int level;
    public int atk;
    public int life;
    public int timeLife; //Dành cho Support Card
    [TextArea(0,3)]
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

}
