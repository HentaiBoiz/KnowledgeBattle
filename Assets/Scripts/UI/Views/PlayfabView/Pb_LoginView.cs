using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using static Rank;

public class Pb_LoginView : View
{
    
    //Variable
    private string userName;
   

    //UI
    [SerializeField]
    private TMP_InputField inputEmail;
    [SerializeField]
    private TMP_InputField inputPassword;

    [SerializeField]
    private Button logginBtn;
    [SerializeField]
    private Button registerBtn;
    [SerializeField]
    private Button resetpassBtn;
    [SerializeField]
    private TMP_Text message;

   
  


    public override void Initialize()
    {
        //inputEmail.text = "crongbi@gmail.com";
        //inputPassword.text = "crong123";
        //inputEmail.text = "trungnhancdps@gmail.com";
        //inputPassword.text = "boycdpscu";
        inputEmail.text = "vodangkhanh1611@gmail.com";
        inputPassword.text = "battlinboxer1611";

        //Player đăng nhập
        logginBtn.onClick.AddListener(() =>
        {
            if (inputEmail.text == "")
                return;

            if (inputPassword.text == "")
                return;

            message.text = null;

            Login();

        });

        registerBtn.onClick.AddListener(() =>
        {
            message.text = null;

            ViewManager.Instance.Show<Pb_RegisterView>();

        });

        resetpassBtn.onClick.AddListener(() =>
        {
            message.text = null;

            ViewManager.Instance.Show<Pb_ResetView>();
        });


        base.Initialize();
    }

    public override void Show(object args = null)
    {
        base.Show(args);
    }

    public void Login()
    {

        string email = inputEmail.text;
        string password = inputPassword.text;

        LoginWithEmailAddressRequest loginRequest = new LoginWithEmailAddressRequest
        {

            Password = password,
            Email = email,

        };
        PlayFabClientAPI.LoginWithEmailAddress(loginRequest, this.LoginSuccess, this.RequestError);

    }



    protected virtual void LoginSuccess(LoginResult result)
    {

        Debug.Log("PlayfabID: " + result.PlayFabId);
        Debug.Log("SessionTicket:" + result.SessionTicket);

        PlayfabUserInfomation.Instance.SetUserData(result.PlayFabId, result.SessionTicket);
        PlayerInfoDDOL.Instance.SetAccountInfo(result.PlayFabId, "", result.SessionTicket); //Cần check lại chỗ này
    }


    #region PLAYFAB METHOD


    private void RequestError(PlayFabError error)
    {
        string textERR = error.GenerateErrorReport();
        message.text = textERR;
    }


    #endregion



    
}
