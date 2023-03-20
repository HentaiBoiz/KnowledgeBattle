using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;

public class JoinLobbyView : View
{
    [SerializeField]
    private Button joinLobbyBtn;
    [SerializeField]
    private Button logoutBtn;

    public override void Initialize()
    {
        //Player join lobby
        joinLobbyBtn.onClick.AddListener(() =>
        {
            JoinLobby();

        });

        //Player đăng xuất
        logoutBtn.onClick.AddListener(() =>
        {
            Logout();

            ViewManager.Instance.Show<LoginView>();
        });


        base.Initialize();
    }

    public override void Show(object args = null)
    {
        base.Show(args);
    }

    public override void OnJoinedLobby()
    {
        //Setup Player Profile On Local PC
        //PlayerProfile user = PlayerInfoDDOL.Instance.playerProfile;

        //user.userId = PhotonNetwork.LocalPlayer.UserId.ToString();
        //user.nickName = PhotonNetwork.LocalPlayer.NickName.ToString();

        ViewManager.Instance.Show<RoomView>();
    }

    //==================================================
    public void JoinLobby()
    {
        PhotonNetwork.JoinLobby();

    }

    public void Logout()
    {
        PhotonNetwork.Disconnect();
    }
}
