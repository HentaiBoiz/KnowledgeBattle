using UnityEngine;
using Photon.Pun;

public class FieldBattleManager : MonoBehaviour
{

    public static FieldBattleManager Instance;

    public BattleZone[] duelistBattles;

    PhotonView _photonView;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();
    }

    private void Update()
    {

        if (Field_Manager_Id.Instance.isUpdateBattle)
        {
            foreach (BattleZone duelistBattle in duelistBattles)
            {
                duelistBattle.UpdateBattleZoneUI(duelistBattle.zoneSide);
            }

            Field_Manager_Id.Instance.isUpdateBattle = false;
        }

    }

    #region RPC METHOD
    public void SetCardAtk(int side, int index, int addatk)
    {
        _photonView.RPC("RPC_SetCardAtk", RpcTarget.All, side, index, addatk);
    }
    [PunRPC]
    public void RPC_SetCardAtk(int side, int index, int addatk)
    {
        duelistBattles[side].battleCards[index].cardMono.atk += addatk;
    }

    public void SetCardLife(int side, int index, int addlife)
    {
        _photonView.RPC("RPC_SetCardLife", RpcTarget.All, side, index, addlife);
    }
    [PunRPC]
    public void RPC_SetCardLife(int side, int index, int addlife)
    {
        duelistBattles[side].battleCards[index].cardMono.life += addlife;
    }


    //Nếu máu về 0 thì lên dĩa
    public void CheckLife(int side, int index)
    {
        if(duelistBattles[side].battleCards[index].cardMono.life <= 0)
        {
            _photonView.RPC("DestroyBattleCard", RpcTarget.All, side, index);
        }

    }

    [PunRPC]
    public void DestroyBattleCard(int side, int index)
    {
        Field_Manager_Id.Instance.AddCardToDropZone(side, duelistBattles[side].battleCards[index].cardMono.id);
        Field_Manager_Id.Instance.zoneId[side].battleZone[index] = "";

        duelistBattles[side].battleCards[index].cardId = "";
        duelistBattles[side].battleCards[index].gameObject.SetActive(false);

        Field_Manager_Id.Instance.Invoke("OnUpdateBattle", 0.2f);
    }
    #endregion
}
