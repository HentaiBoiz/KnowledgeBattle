using System.Collections.Generic;
using UnityEngine;

public abstract class CardEffect : MonoBehaviour
{
    protected List<string> affectedIds = new List<string>(); //Những effect này ảnh hưởng tới lá bài nào ?

    // You can give that the signature (parameters and return type) you want/need of course
    // Every type inherited from this HAS TO implement this method
    public abstract void ExecuteEffect(EffectManager effectManager);

    public abstract void CancelEffect(EffectManager effectManager);

    public abstract bool canBeActivate();
}
