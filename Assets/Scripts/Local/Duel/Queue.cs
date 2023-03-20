using UnityEngine;

public class Queue : MonoBehaviour
{
    public int zoneSide; //0: Host, 1: Client

    public ThisCard[] queueCards = new ThisCard[3];


    private void Start()
    {
        //Disable all queue cards
        DisableAllQueueCards();
    }

    public void UpdateQueueZoneUI(int size)
    {

        for (int i = 0; i < queueCards.Length; i++)
        {
            if(Field_Manager_Id.Instance.zoneId[zoneSide].queueZone[i] != "") //Nghĩa là ở vị trí queue đó có Id của card chứ không rỗng
            {
                queueCards[i].gameObject.SetActive(true);

                queueCards[i].GetComponent<ThisCard>().SetupCard(Field_Manager_Id.Instance.zoneId[zoneSide].queueZone[i]);

                //Chỉ có đúng side thì mới Activate Effect được
                if (queueCards[i].GetComponent<CardInQueue>().isActivateEffect == false)
                {
                    queueCards[i].GetComponent<CardInQueue>().isActivateEffect = true;
                    queueCards[i].GetComponent<CardInQueue>().CheckQueueEffect(EffectManager.Instance);
                }
                    
            }
            else
            {
                queueCards[i].GetComponent<CardInQueue>().isActivateEffect = false;
                queueCards[i].GetComponent<ThisCard>().cardMono = null;
                queueCards[i].gameObject.SetActive(false);
            }

        }

    }

    public void DisableAllQueueCards()
    {
        foreach (var queueCard in queueCards)
        {
            queueCard.gameObject.SetActive(false);
        }
    }
}
