using Photon;
using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using Unity.Entities.UniversalDelegates;
using Unity.Scenes;
using UnityEngine;

public class DuelRuleManager : MonoBehaviourPunCallbacks
{
    public static DuelRuleManager Instance;
    PhotonView _photonView;
    //Score
    public static int AddScore;
    public static int SubScore;

    public static string resultWLose;

    [Header("RULE VARIABLES")]
    bool isCheck = false;
    public bool wasPush = false; //Mỗi lượt Player chỉ được đẩy 1 lá bài lên Battle Zone

    [Header("UI")]
    public Transform winMessage;
    public Transform loseMessage;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();

        winMessage.gameObject.SetActive(false);
        loseMessage.gameObject.SetActive(false);

        //Set Variable
        wasPush = false;
    }

    private void Update()
    {

        if (Field_Manager_Id.Instance.isDuelStart == false) //Game chưa bắt đầu thì không chạy
            return;

        if (!PhotonNetwork.InRoom)
            return;

        if (PhotonNetwork.CurrentRoom.PlayerCount != 2)
            return;

        if (isCheck)
            return;

        //Điểm gốc của bạn bằng 0 thì xử thua, đây là trường hợp khi cả 2 còn Connect đầy đủ
        if (Field_Manager_Id.Instance.zoneId[TurnManager.Instance.localSide].healthPoint <= 0)
        {
            YouLose(TurnManager.Instance.localSide);
        }

    }

    #region RPC METHOD
    public void YouLose(int loseSide)
    {
        _photonView.RPC("RPC_YouLose", RpcTarget.All, loseSide);
    }
    [PunRPC]
    public void RPC_YouLose(int loseSide)
    {
        if (TurnManager.Instance.localSide == loseSide) //Lose
        {
            Invoke("Lose", 2f);

        }
        else //Win
        {
            Invoke("Win", 2f);
        }

        isCheck = true;
    }
    #endregion

    #region LOCAL METHOD
    public void Win()
    {

        Debug.Log("You Win");
        resultWLose = "You Win";
        winMessage.gameObject.SetActive(true);

        PlayfabUserInfomation.Instance.SetPlayerPunish(0); //Đưa hình phạt về 0

        AddVC();
        
    }

    public void Lose()
    {
        Debug.Log("You Lose");
        resultWLose = "You Lose";
        loseMessage.gameObject.SetActive(true);

        PlayfabUserInfomation.Instance.SetPlayerPunish(0); //Đưa hình phạt về 0

        SubVC();
        
    }

    public void LoadScene()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("EndGame");
    }
    #endregion

    #region PLAYFAB METHOD

    public void AddVC()
    {

        int rand = Random.Range(10, 40);
        AddScore = rand;

        AddUserVirtualCurrencyRequest vcRequest = new AddUserVirtualCurrencyRequest
        {

            Amount = AddScore,
            VirtualCurrency = "RS",

        };
        PlayFabClientAPI.AddUserVirtualCurrency(vcRequest, OnAddVCComplete, OnPlayfabError);

    }

    public void OnAddVCComplete(ModifyUserVirtualCurrencyResult VCi)
    {
        Debug.Log("Complete" + VCi.BalanceChange);
        AddScore = VCi.BalanceChange;
        Debug.Log(AddScore.ToString());

        //Leave game
        LoadScene();
    }

    public void SubVC()
    {
        int rand = Random.Range(10, 40);

        SubScore = rand;

        SubtractUserVirtualCurrencyRequest virtualCurrencyRequest = new SubtractUserVirtualCurrencyRequest
        {
            Amount = SubScore,
            VirtualCurrency = "RS",

        };
        PlayFabClientAPI.SubtractUserVirtualCurrency(virtualCurrencyRequest, SubVCComplete, OnPlayfabError);

    }

    public void SubVCComplete(ModifyUserVirtualCurrencyResult VC)
    {
        Debug.Log("Complete" + VC.BalanceChange);
        SubScore = VC.BalanceChange;
        Debug.Log(SubScore.ToString());

        //Leave game
        LoadScene();
    }
    public void OnPlayfabError(PlayFabError error)
    {
        string textERR = error.GenerateErrorReport();

        ErrorsManager.Instance.PushError(textERR);
        Debug.LogWarning(textERR);

    }

    #endregion

    #region PHOTON METHOD
    //Nếu có người thoát trận thì người còn lại win
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        if (isCheck)
            return;

        Win();
    }
    #endregion
}
