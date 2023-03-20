using static Card;
using UnityEngine;
using System.Collections.Generic;

//Effect này tăng ATK của  tất cả lá bài theo Attribute
public class IncreaseAtkAttribute : CardEffect
{
    public int amount; //Tăng bao nhiêu ?
    public CardAttribute attribute; //Hệ nào ?

    int tempSide = -1;

    public override void ExecuteEffect(EffectManager effectManager)
    {
        tempSide = EffectManager.Instance.actionSide;

        string tempId = Time.deltaTime.ToString();

        affectedIds.Add(tempId);

        effectManager.StartIncATKAttribute(tempSide, amount, attribute, tempId);
    }


    public override void CancelEffect(EffectManager effectManager)
    {
        foreach (var id in affectedIds)
        {
            effectManager.EndIncATKAttribute(tempSide, amount, attribute, id);
        }

        affectedIds = new List<string>();
    }

    public override bool canBeActivate()
    {
        return true;
    }
}
