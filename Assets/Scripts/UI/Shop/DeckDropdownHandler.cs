using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeckDropdownHandler : MonoBehaviour
{
    TMP_Dropdown _dropDown;

    private void Awake()
    {
        _dropDown = GetComponent<TMP_Dropdown>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
        _dropDown.ClearOptions();

        _dropDown.onValueChanged.AddListener(delegate { DropDownItemSelected(_dropDown); });
    }

    public void AddDeckOptions(int index)
    {
        _dropDown.AddOptions(new List<string>() { "Slot " + (index + 1).ToString() });
    }

    void DropDownItemSelected(TMP_Dropdown dropdown)
    {
        int index = dropdown.value;
        string slotIndex = dropdown.options[index].text.Remove(0, 5);

        DeckInterfact.Instance.ChangeDeckSlot(int.Parse(slotIndex) - 1);
    }

    public void SetStartOption(int index)
    {
        _dropDown.value = index;
    }
}
