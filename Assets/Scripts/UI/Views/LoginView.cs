using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class LoginView : View
{
    [SerializeField]
    private TMP_InputField inputName;
    [SerializeField]
    private Button logginBtn;


    public override void Initialize()
    {
        PhotonNetwork.GameVersion = "0.0.1";

        //Player đăng nhập
        logginBtn.onClick.AddListener(() =>
        {
            if (inputName.text == "")
                return;

            Login();

        });


        base.Initialize();
    }

    public override void Show(object args = null)
    {
        base.Show(args);
    }

    public override void OnConnectedToMaster()
    {
        ViewManager.Instance.Show<JoinLobbyView>();
    }

    //=====================================================
    public void Login()
    {

        string userName = inputName.text;

        PhotonNetwork.LocalPlayer.NickName = userName;
        PhotonNetwork.ConnectUsingSettings();
    }

}
