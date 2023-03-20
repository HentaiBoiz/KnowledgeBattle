using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using Photon.Pun;

public class Pb_ResetView : View
{
    [SerializeField]
    private TMP_InputField inputEmail;
    [SerializeField]
    private Button resetpassBtn;
    [SerializeField]
    private Button logginBtn;
    [SerializeField]
    private TMP_Text message;


    public override void Initialize()
    {
        inputEmail.text = "trungnhancdps@gmail.com";
       
        //Player đăng nhập
        resetpassBtn.onClick.AddListener(() =>
        {
            if (inputEmail.text == "")
                return;


            ResetPassword();

        });

        logginBtn.onClick.AddListener(() =>
        {
            ViewManager.Instance.Show<Pb_LoginView>();

        });

        

        base.Initialize();
    }

    public override void Show(object args = null)
    {
        base.Show(args);
    }


    public virtual void ResetPassword()
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            // Password = this.password.text,
            Email = this.inputEmail.text,
            TitleId = "28339"
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, LoginSuccess, LoginErr);

    }

    void LoginSuccess(SendAccountRecoveryEmailResult result)
    {
        Debug.Log("check your email to reset your password!!");
        message.text = "Please check your email !!!";
       
    }
    protected virtual void LoginErr(PlayFabError error)
    {
        string textERR = error.GenerateErrorReport();
        Debug.LogWarning(textERR);
    }
}
