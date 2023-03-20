using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.UI;

public class QuestDetailsView : View
{
    public CinemachineVirtualCamera cinemachineVirtualCamera; //Disable khi mở menu lên
    PlayerInput playerInput;

    //public Quest quest;

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
        base.Initialize();
    }

    public override void Show(object args = null)
    {

        DisablePlayerController();
        base.Show(args);
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
