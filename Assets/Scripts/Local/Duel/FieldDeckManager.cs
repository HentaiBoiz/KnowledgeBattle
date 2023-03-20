using UnityEngine;

public class FieldDeckManager : MonoBehaviour
{
    public DuelistDeck[] duelistDecks;

    private void Update()
    {
        if (Field_Manager_Id.Instance.isUpdateDeck)
        {
            foreach (DuelistDeck duelistDeck in duelistDecks)
            {
                duelistDeck.UpdateDeckUI(Field_Manager_Id.Instance.zoneId[duelistDeck.deckSide].deckZone.Count);
            }

            Field_Manager_Id.Instance.isUpdateDeck = false;
        }
    }

    
}
