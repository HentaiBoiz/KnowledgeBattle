using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class RPS_Manager : MonoBehaviour
{
    public enum RPS
    {
        Null,
        Rock,
        Paper,
        Scissor
    }

    public bool isAllReady = false; //Bắt đầu bao búa kéo hay chưa ?

    public RPS hostChoose; //Lựa chọn của Host
    public RPS clientChoose; //Lựa chọn của Player

    public Transform hostClickButtons;
    public Transform clientClickButtons;
    public Transform chooseSideButtons; //Các Button mang lựa chọn đi trước hay đi sau

    //Show Result
    private Animator animator;
    private bool isShowing = false;

    public Sprite RockSprite;
    public Sprite PaperSprite;
    public Sprite ScissorSprite;

    public Image P1_ChooseImage;
    public Image P2_ChooseImage;

    private PhotonView photonView;

    //Tên phòng Duel
    public string SCENE_NAME;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        chooseSideButtons.gameObject.SetActive(false);

        EnableButtonForDuelist();
    }

    private void Update()
    {

        if (!PhotonNetwork.IsMasterClient)
            return;

        if (hostChoose == RPS.Null || clientChoose == RPS.Null)
            return;

        ShowResultAnimation();

    }

    public void OnSelectRock()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            RPC_HostChoose(RPS.Rock);
        }
        else
        {
            RPC_ClientChoose(RPS.Rock);
        }
    }

    public void OnSelectPaper()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            RPC_HostChoose(RPS.Paper);
        }
        else
        {
            RPC_ClientChoose(RPS.Paper);
        }
    }

    public void OnSelectScissor()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            RPC_HostChoose(RPS.Scissor);
        }
        else
        {
            RPC_ClientChoose(RPS.Scissor);
        }
    }

    public void OnSelectGoFirst()
    {
        PlayerInfoDDOL.Instance.playerProfile.goFirst = 1;
        photonView.RPC("LetsDuel", RpcTarget.All);
    }

    public void OnSelectGoSecond()
    {
        PlayerInfoDDOL.Instance.playerProfile.goFirst = 2;
        photonView.RPC("LetsDuel", RpcTarget.All);
    }

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if (stream.IsWriting)
    //    {
    //        stream.SendNext(hostChoose);
    //        stream.SendNext(clientChoose);
    //    }
    //    else if (stream.IsReading)
    //    {
    //        hostChoose = (RPS)stream.ReceiveNext();
    //        clientChoose = (RPS)stream.ReceiveNext();
    //    }
    //}

    #region RPC METHOD
    private void RPC_HostChoose(RPS choose)
    {
        photonView.RPC("SetHostChoose", RpcTarget.All, choose);
        hostClickButtons.gameObject.SetActive(false);
    }
    [PunRPC]
    private void SetHostChoose(RPS choose)
    {
        hostChoose = choose;
    }
    private void RPC_ClientChoose(RPS choose)
    {
        photonView.RPC("SetClientChoose", RpcTarget.All, choose);
        clientClickButtons.gameObject.SetActive(false);
    }
    [PunRPC]
    private void SetClientChoose(RPS choose)
    {
        clientChoose = choose;
    }
    private void RPC_AllShowButton()
    {
        photonView.RPC("SetAllShowButton", RpcTarget.All);
    }
    [PunRPC]
    private void SetAllShowButton()
    {
        EnableButtonForDuelist();
    }

    //Gắn image vào để show kết quả
    private void RPC_ShowResultImage() 
    {
        photonView.RPC("SetShowResultImage", RpcTarget.All);
    }
    [PunRPC]
    private void SetShowResultImage()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            switch (hostChoose)
            {
                case RPS.Rock:
                    P1_ChooseImage.sprite = RockSprite;
                    break;
                case RPS.Paper:
                    P1_ChooseImage.sprite = PaperSprite;
                    break;
                case RPS.Scissor:
                    P1_ChooseImage.sprite = ScissorSprite;
                    break;
            }

            switch (clientChoose)
            {
                case RPS.Rock:
                    P2_ChooseImage.sprite = RockSprite;
                    break;
                case RPS.Paper:
                    P2_ChooseImage.sprite = PaperSprite;
                    break;
                case RPS.Scissor:
                    P2_ChooseImage.sprite = ScissorSprite;
                    break;
            }

        }
        else
        {
            switch (clientChoose)
            {
                case RPS.Rock:
                    P1_ChooseImage.sprite = RockSprite;
                    break;
                case RPS.Paper:
                    P1_ChooseImage.sprite = PaperSprite;
                    break;
                case RPS.Scissor:
                    P1_ChooseImage.sprite = ScissorSprite;
                    break;
            }

            switch (hostChoose)
            {
                case RPS.Rock:
                    P2_ChooseImage.sprite = RockSprite;
                    break;
                case RPS.Paper:
                    P2_ChooseImage.sprite = PaperSprite;
                    break;
                case RPS.Scissor:
                    P2_ChooseImage.sprite = ScissorSprite;
                    break;
            }

        }
    }

    //Show Button chọn lượt đi cho Client
    [PunRPC]
    private void ShowTurnButtonClient()
    {
        chooseSideButtons.gameObject.SetActive(true);
    }

    //All Go To Duel Room
    [PunRPC]
    private void LetsDuel()
    {
        PhotonNetwork.LoadLevel(SCENE_NAME);
    }
    #endregion

    #region MASTERCLIENT METHOD

    public void EnableButtonForDuelist()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            hostClickButtons.gameObject.SetActive(true);

        }
        else
        {
            clientClickButtons.gameObject.SetActive(true);
        }
    }

    public void ShowResultAnimation()
    {
        if (isShowing)
            return;

        animator.SetBool("Show", true);
        isShowing = true;
        RPC_ShowResultImage();
    }
    public void EndShowResultAnimation()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        animator.SetBool("Show", false); //Set về lại False

        if (hostChoose == clientChoose) //Huề kết quả
        {
            hostChoose = clientChoose = RPS.Null;
            RPC_AllShowButton(); //Mở lại Button
            isShowing = false;
            return;
        }

        switch (hostChoose)
        {
            case RPS.Rock:
                if (clientChoose == RPS.Paper)
                    ClientWinRPS();
                if (clientChoose == RPS.Scissor)
                {
                    HostWinRPS();
                }
                break;
            case RPS.Paper:
                if (clientChoose == RPS.Scissor)
                    ClientWinRPS();
                if (clientChoose == RPS.Rock)
                {
                    HostWinRPS();
                }
                break;
            case RPS.Scissor:
                if (clientChoose == RPS.Rock)
                    ClientWinRPS();
                if (clientChoose == RPS.Paper)
                {
                    HostWinRPS();
                }
                break;
        }
        

    }

    public void HostWinRPS()
    {
        chooseSideButtons.gameObject.SetActive(true);
    }

    public void ClientWinRPS()
    {
        photonView.RPC("ShowTurnButtonClient", RpcTarget.Others);
    }
    #endregion
}
