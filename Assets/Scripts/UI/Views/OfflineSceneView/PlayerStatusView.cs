using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;

public class PlayerStatusView : View
{
    //UI
    [SerializeField]
    private Image playerAvatar;
    [SerializeField]
    private TextMeshProUGUI playerName;
    [SerializeField]
    private TextMeshProUGUI playerMoney;
    [SerializeField]
    private GameObject messageTxt;
    [SerializeField]
    private Button btnOpenQuestBoard;
    //Count
    float messageTime = 5f; //Thời gian Message hiện ra
    float countTime;

    private void Update()
    {
        if(messageTxt.activeSelf == true)
            countTime -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Escape))
        {

            ViewManager.Instance.Show<UserMenuView>();

        }

        if(countTime <= 0 && messageTxt != null)
        {
            HideMessage();
        }
    }

    public override void Initialize()
    {

        playerName.text = PhotonNetwork.NickName;
        GetVirtualCurrency();

        //Nếu có Punish
        if (PlayfabUserInfomation.Instance.playerData.punish > 0)
        {
            PlayfabUserInfomation.Instance.SubVC(PlayfabUserInfomation.Instance.playerData.punish);

            //ShowMessage("ABANDONED GAME PENATLY: You received a Rank Score penalty for leaving a recent game! We expect player to try their hardest in each game they play !");
        }
        btnOpenQuestBoard.onClick.AddListener(() =>
        {
            ViewManager.Instance.Show<QuestDetailsView>();
        });

        base.Initialize();
    }

    public override void Show(object args = null)
    {

        base.Show(args);
    }

    public void GetVirtualCurrency()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), OnGetUserInventorySuccess, OnRequestError);
    }

    public void OnGetUserInventorySuccess(GetUserInventoryResult result)
    {
        int kcoin = result.VirtualCurrency["KC"];
        playerMoney.text = kcoin.ToString();

    }

    public void OnRequestError(PlayFabError error)
    {
        string textERR = error.GenerateErrorReport();
        Debug.LogWarning(textERR);
    }

    //======================
    public void ShowMessage(string txt)
    {
        try
        {
            countTime = messageTime;

            messageTxt.GetComponent<TextMeshProUGUI>().text = txt;
            messageTxt.gameObject.SetActive(true);
        }
        catch (System.Exception e)
        {
            ErrorsManager.Instance.PushError(e.ToString());
        }
        
    }

    public void HideMessage()
    {
        messageTxt.gameObject.SetActive(false);
    }
}
