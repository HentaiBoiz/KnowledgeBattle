using Photon.Pun;
using UnityEngine;
using static StateManager;

public class BattlePopup : MonoBehaviour
{
    public Transform attackBtn;

    public ThisCard battleCard = null; //Khi chọn 1 Card trên Battle Zone, ThisCard của card đó trên tay sẽ được truyền vào
    public int battleZoneIndex = -1; //Khi chọn 1 Card trên Battle Zone, index của card đó trên tay sẽ được truyền vào
    public int side = -1; //0: Host, 1: Client

    private void Update()
    {
        //Khi không còn ở trong Battle Step thì tắt Panel này đi
        if(TurnManager.Instance.currentStep != TurnManager.TurnStep.BattleStep)
        {
            this.side = -1;
            battleZoneIndex = -1;
            this.gameObject.SetActive(false);
        }
    }

    public void SetupBattle(int side, int battleIndex, ThisCard attackerCard)
    {
        this.side = side;
        battleZoneIndex = battleIndex;
        battleCard = attackerCard;
    }

    //Khi bấm Button Attack thì chuyển sang Fighting State
    public void ChangeToFightingState()
    {

        StateManager.Instance.DuelState = ActionState.fightingState;
 
        //Không có Quái thú ở Battle Zone thì tấn công trực diện
        if (PhotonNetwork.IsMasterClient)
        {
            if (Field_Manager_Id.Instance.CheckOppBattleIsEmpty(1))
            {
                int currHp = Field_Manager_Id.Instance.zoneId[1].healthPoint;

                Duel_VFX_Manager.Instance.StartAttackDirectlyVFX(battleCard.cardMono.id, 0);

                Field_Manager_Id.Instance.SetDuelistHP(1, currHp - battleCard.cardMono.atk);

                Field_Manager_Id.Instance.SetAttackedBool(0, battleZoneIndex, true);

                StateManager.Instance.DuelState = ActionState.normalState;
            }
        }
        else
        {
            if (Field_Manager_Id.Instance.CheckOppBattleIsEmpty(0))
            {
                if (Field_Manager_Id.Instance.CheckOppBattleIsEmpty(0))
                {
                    int currHp = Field_Manager_Id.Instance.zoneId[0].healthPoint;

                    Duel_VFX_Manager.Instance.StartAttackDirectlyVFX(battleCard.cardMono.id, 1);

                    Field_Manager_Id.Instance.SetDuelistHP(0, currHp - battleCard.cardMono.atk);

                    Field_Manager_Id.Instance.SetAttackedBool(1, battleZoneIndex, true);

                    StateManager.Instance.DuelState = ActionState.normalState;
                }
            }
        }

        this.gameObject.SetActive(false);
    }
  
}
