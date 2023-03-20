using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawEffect : CardEffect
{
    // Adjust how many cards are drawn
    public int amount;


    public override void ExecuteEffect(EffectManager effectManager)
    {
        int temp = EffectManager.Instance.actionSide;

        effectManager.StartDrawACard(temp);
        
    }


    public override void CancelEffect(EffectManager effectManager)
    {
        //Do Nothing
    }

    public override bool canBeActivate()
    {
        return true;
    }
}
