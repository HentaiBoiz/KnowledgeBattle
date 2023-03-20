using UnityEngine;

public class FieldHandManager : MonoBehaviour
{
    public Hand[] duelistHands;

    private void Update()
    {
        if (Field_Manager_Id.Instance.isUpdateHand)
        {
            foreach (Hand duelistHand in duelistHands)
            {
                duelistHand.UpdateHandUI(duelistHand.handSide);
            }


            Field_Manager_Id.Instance.isUpdateHand = false;
        }

    }
}
