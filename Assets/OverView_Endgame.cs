using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OverView_Endgame : View
{

    [SerializeField]
    public TMP_Text Score;
    [SerializeField]
    public TMP_Text Scorenew;
    [SerializeField]
    public TMP_Text KC;
    [SerializeField]
    public TMP_Text rate;
    [SerializeField]
    public Button DetailsViewBtn;
    [SerializeField]
    public Image ReImg;

    int AddKC;


    string win = "WinLose/win", lose = "WinLose/lose";

    public override void Initialize()
    {
        DetailsViewBtn.onClick.AddListener(() =>
        {
            ViewManager.Instance.Show<DetailsView_Endgame>();

        });
        getAddKC();


        base.Initialize();
    }


    public override void Show(object args = null)
    {
        base.Show(args);
    }
    
    public void getAddKC()
    {
        int r = Random.Range(100, 300);
        AddKC = r;
        AddUserVirtualCurrencyRequest vcRequest = new AddUserVirtualCurrencyRequest
        {

            Amount = AddKC,
            VirtualCurrency = "KC",

        };
        PlayFabClientAPI.AddUserVirtualCurrency(vcRequest, OnAddVCComplete, OnRequestsError);
    }

    private void OnRequestsError(PlayFab.PlayFabError obj)
    {
        throw new System.NotImplementedException();
        Debug.Log("Failed");
    }

    public void OnAddVCComplete(ModifyUserVirtualCurrencyResult VCi)
    {
        Debug.Log("Complete" + VCi.BalanceChange);
        AddKC = VCi.BalanceChange;
        string a = DuelRuleManager.resultWLose.ToString();
        Debug.Log(a);

        if (a == "You Win")
        {
            ReImg.sprite = Resources.Load<Sprite>(win);
            Score.text = $"<color=green>+{DuelRuleManager.AddScore.ToString()}</color>";
            KC.text = AddKC.ToString();
            Debug.Log(Score);
        }
        else
        {
            ReImg.sprite = Resources.Load<Sprite>(lose);
            Score.text = (DuelRuleManager.SubScore.ToString());
            Score.text = ( DuelRuleManager.SubScore.ToString());
            
            Score.text = $"<color=red>{DuelRuleManager.SubScore.ToString()}</color>";
            
            KC.text = (AddKC / 2).ToString();
            Debug.Log(Score);
        }


    }
   
    





    



}
