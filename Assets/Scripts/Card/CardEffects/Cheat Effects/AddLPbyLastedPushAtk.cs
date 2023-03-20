using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Effect này cộng điểm gốc của Player bằng với sức tấn công của quái thú đối thủ vừa triệu hồi
public class AddLPbyLastedPushAtk : CardEffect
{

    public override bool canBeActivate()
    {
        //Nếu có lá vừa được summon thì cho phép kích hoạt
        if (CheatEventManager.Instance.GetLastPushCard() != null)
            return true;

        return false;
    }

    public override void CancelEffect(EffectManager effectManager)
    {
        
    }

    public override void ExecuteEffect(EffectManager effectManager)
    {
        effectManager.IncreaseLPByLastedPushATK();

        CheatEventManager.Instance.Invoke("EndCounterState", 0.3f);
    }
}
