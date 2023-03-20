using UnityEngine;

public class FieldDropManager : MonoBehaviour
{
    public DropZone[] dropZones;

    private void Update()
    {
        if (Field_Manager_Id.Instance.isUpdateDrop)
        {
            foreach (DropZone dropZone in dropZones)
            {
                dropZone.UpdateDropUI(Field_Manager_Id.Instance.zoneId[dropZone.dropSide].dropZone.Count);
            }

            Field_Manager_Id.Instance.isUpdateDrop = false;
        }
    }
}
