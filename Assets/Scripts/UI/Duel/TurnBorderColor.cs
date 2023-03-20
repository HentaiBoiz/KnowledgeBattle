using Photon.Pun;
using UnityEngine;

public class TurnBorderColor : MonoBehaviour
{
    SpriteRenderer border;
    public Color playerColor;
    public Color opponentColor;

    private void Start()
    {
        border = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Field_Manager_Id.Instance.isDuelStart == false)
            return;

        switch (TurnManager.Instance.currentTurn)
        {
            case 0:
                if (PhotonNetwork.IsMasterClient)
                {
                    border.color = playerColor;
                }
                else
                {
                    border.color = opponentColor;
                }
                break;
            case 1:
                if (PhotonNetwork.IsMasterClient)
                {
                    border.color = opponentColor;
                }
                else
                {
                    border.color = playerColor;
                }
                break;
        }
    }
}
