using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldCheatManager : MonoBehaviour
{
    public CardInCheat[] duelistCheats;

    private void Start()
    {
        DisableAllCheatCards();
    }

    private void Update()
    {
        if (Field_Manager_Id.Instance.isUpdateCheat)
        {
            foreach (CardInCheat duelistCheat in duelistCheats)
            {
                UpdateCheatZoneUI(duelistCheat, duelistCheat.zoneId);
            }

            Field_Manager_Id.Instance.isUpdateCheat = false;
        }
    }

    public void UpdateCheatZoneUI(CardInCheat cheatCard, int side)
    {
        if (Field_Manager_Id.Instance.zoneId[side].cheatZone != "") //Nghĩa là ở vị trí queue đó có Id của card chứ không rỗng
        {
            cheatCard.gameObject.SetActive(true);

            cheatCard.GetComponent<ThisCard>().SetupCard(Field_Manager_Id.Instance.zoneId[side].cheatZone);

            ////Chỉ có đúng side thì mới Activate Effect được
            //if (cheatCard.GetComponent<CardInCheat>().isActivateEffect == false)
            //{
            //    cheatCard.GetComponent<CardInCheat>().isActivateEffect = true;
            //    cheatCard.GetComponent<CardInCheat>().CheckCheatEffect(EffectManager.Instance);
            //}

        }
        else
        {
            cheatCard.GetComponent<CardInCheat>().isActivateEffect = false;
            cheatCard.GetComponent<ThisCard>().cardMono = null;
            cheatCard.gameObject.SetActive(false);
        }
    }

    public void DisableAllCheatCards()
    {
        foreach (var cheatCard in duelistCheats)
        {
            cheatCard.gameObject.SetActive(false);
        }
    }
}
