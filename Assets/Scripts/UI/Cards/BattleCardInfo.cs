using TMPro;
using UnityEngine;

public class BattleCardInfo : MonoBehaviour
{
    public TextMeshPro atkTxt;
    public TextMeshPro lifeTxt;


    public void SetupBattleInfo(int atk, int life, int defaultAtk, int defaultLife)
    {
        //ATK COLOR
        if(atk > defaultAtk)
        {
            atkTxt.text = $"<color=green>ATK: {atk.ToString()}</color>";
        } 
        else if(atk < defaultAtk)
        {
            atkTxt.text = $"<color=red>ATK: {atk.ToString()}</color>";
        }
        else
        {
            atkTxt.text = $"<color=white>ATK: {atk.ToString()}</color>";
        }

        //LIFE COLOR
        if (life > defaultLife)
        {
            lifeTxt.text = $"<color=green>LIFE: {life.ToString()}</color>";
        }
        else if (life < defaultLife)
        {
            lifeTxt.text = $"<color=red>LIFE: {life.ToString()}</color>";
        }
        else
        {
            lifeTxt.text = $"<color=white>LIFE: {life.ToString()}</color>";
        }

    }
}
