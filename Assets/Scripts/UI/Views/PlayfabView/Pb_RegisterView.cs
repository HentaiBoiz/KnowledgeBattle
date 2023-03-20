using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Pb_RegisterView : View
{
    [SerializeField]
    private TMP_InputField inputEmail;
    [SerializeField]
    private TMP_InputField inputName;
    [SerializeField]
    private TMP_InputField inputPassword;

    [SerializeField]
    private Button logginBtn;
    [SerializeField]
    private Button registerBtn;

    public override void Initialize()
    {
        inputEmail.text = "trungnhancdps@gmail.com";
        inputName.text = "yukey123456";

        //Player đăng nhập
        registerBtn.onClick.AddListener(() =>
        {
            if (inputEmail.text == "")
                return;

            if (inputName.text == "")
                return;

            if (inputPassword.text == "")
                return;

            Register();

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

    public void Register()
    {

        string email = inputEmail.text;
        string userName = inputName.text;
        string password = inputPassword.text;

        RegisterPlayFabUserRequest registerRequest = new RegisterPlayFabUserRequest
        {
            Email = email,
            Password = password,
            Username = userName,
            DisplayName = userName,
        };
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, this.RegisterSuccess, this.RegisterErr);
       

        //PhotonNetwork.LocalPlayer.NickName = userName;
        //PhotonNetwork.ConnectUsingSettings();
    }


    protected virtual void RegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("Register Success");
        ViewManager.Instance.Show<Pb_LoginView>();
    }
    protected virtual void RegisterErr(PlayFabError error)
    {
        string textERR = error.GenerateErrorReport();
        Debug.LogWarning(textERR);
    }


}
