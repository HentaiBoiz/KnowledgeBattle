using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserMenuView : View
{
    public CinemachineVirtualCamera cinemachineVirtualCamera; //Disable khi mở menu lên
    PlayerInput playerInput;
    Transform playerPos;

    [Header("UI")]
    public Button deckEditorBtn;
    public Button quickMatchBtn;
    public Button playerProfileBtn;
    public Button logoutBtn;
    public Button exitBtn;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EnablePlayerController();

            ViewManager.Instance.Show<PlayerStatusView>();
        }
    }

    public override void Initialize()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;

        quickMatchBtn.onClick.AddListener(() =>
        {
            PlayerInfoDDOL.Instance.SetPlayerLastPos(playerPos.position);

            PhotonNetwork.LoadLevel("1_DuelRoomScene");
        });


        deckEditorBtn.onClick.AddListener(() =>
        {
            PlayerInfoDDOL.Instance.SetPlayerLastPos(playerPos.position);

            PhotonNetwork.LoadLevel("4_DeckEditor");
        });

        playerProfileBtn.onClick.AddListener(() =>
        {
            PlayerInfoDDOL.Instance.SetPlayerLastPos(playerPos.position);

            PhotonNetwork.LoadLevel("1_PlayerProfile");
        });

        playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();


        //Player đăng xuất
        logoutBtn.onClick.AddListener(() =>
        {
            PlayfabUserInfomation.Instance.LogoutPlayerLoginTime();
            PhotonNetwork.Disconnect();

        });

        exitBtn.onClick.AddListener(() =>
        {
            Application.Quit();
        });

        base.Initialize();
    }

    public override void Show(object args = null)
    {
        DisablePlayerController();

        base.Show(args);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        SceneManager.LoadScene("0_LoginScenePlayfab");
    }

    public void EnablePlayerController()
    {
        try
        {
            cinemachineVirtualCamera.enabled = true;
            playerInput.SwitchCurrentActionMap("Player");
        }
        catch (System.Exception e)
        {
            ErrorsManager.Instance.PushError(e.ToString());
        }
        
    }

    public void DisablePlayerController()
    {
        try
        {
            cinemachineVirtualCamera.enabled = false;
            playerInput.SwitchCurrentActionMap("Menu");
        }
        catch (System.Exception e)
        {
            ErrorsManager.Instance.PushError(e.ToString());
        }
       
    }


}
