using UnityEngine;
using static StateManager;

public class FieldQueueManager : MonoBehaviour
{
    public Queue[] duelistQueues;

    private void Update()
    {
        if (Field_Manager_Id.Instance.isUpdateQueue)
        {
            foreach (Queue duelistQueue in duelistQueues)
            {
                duelistQueue.UpdateQueueZoneUI(duelistQueue.zoneSide);
            }

            Field_Manager_Id.Instance.isUpdateQueue = false;
        }

    }

    #region NORMAL METHOD
    public void QueueToAttachState() //Nên cân nhắc có nên đưa vào RPC hay ko
    {
        StateManager.Instance.DuelState = ActionState.attachState;
    }

    public void AttachToNormalState()
    {
        StateManager.Instance.DuelState = ActionState.normalState;
    }
    #endregion
}
