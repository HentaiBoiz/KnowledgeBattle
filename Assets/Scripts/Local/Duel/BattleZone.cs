using UnityEngine;

public class BattleZone : MonoBehaviour
{
    public int zoneSide; //0: Host, 1: Client

    public ThisCard[] battleCards = new ThisCard[3];


    private void Start()
    {
        //Set this to Battle Card
        foreach (var battleCard in battleCards)
        {
            battleCard.GetComponent<CardInBattleZone>()._battleZone = this;
        }

        //Disable all queue cards
        DisableAllBattleCards();
    }

    public void UpdateBattleZoneUI(int size)
    {

        for (int i = 0; i < battleCards.Length; i++)
        {
            if (Field_Manager_Id.Instance.zoneId[zoneSide].battleZone[i] != "") //Nghĩa là ở vị trí battle đó có Id của card chứ không rỗng
            {
                battleCards[i].gameObject.SetActive(true);

                battleCards[i].GetComponent<ThisCard>().SetupCard(Field_Manager_Id.Instance.zoneId[zoneSide].battleZone[i]);

            }
            else
            {
                battleCards[i].GetComponent<ThisCard>().cardMono = null;
                battleCards[i].gameObject.SetActive(false);
            }

        }

    }

    public void DisableAllBattleCards()
    {
        foreach (var battleCard in battleCards)
        {
            battleCard.gameObject.SetActive(false);
        }
    }
}
