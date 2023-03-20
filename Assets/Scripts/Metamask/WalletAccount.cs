using UnityEngine;
using TMPro;

public class WalletAccount : MonoBehaviour
{
    public TextMeshProUGUI myAccount;

    // Start is called before the first frame update
    void Start()
    {
        myAccount.text = PlayerPrefs.GetString("Account");
    }


}