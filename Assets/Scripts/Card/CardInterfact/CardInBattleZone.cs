using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using static StateManager;
using static TurnManager;

public class CardInBattleZone : MonoBehaviour
{
    public int index;

    public BattleCardInfo battleCardInfo;

    [HideInInspector]
    public BattleZone _battleZone;
    public List<string> affectingId = new List<string>(); //Đây là nơi lưu trữ id của những lá bài effect đang tác động lên lá bài này

    private void OnEnable()
    {
        if (_battleZone == null)
            return;

        //Xoay Card Battle Info
        if (PhotonNetwork.IsMasterClient) //Nếu là Host
        {
            if (_battleZone.zoneSide == 0)
            {
                battleCardInfo.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                battleCardInfo.transform.localScale = new Vector3(-1, -1, 1);
            }
        }
        else 
        {
            if (_battleZone.zoneSide == 1)
            {
                battleCardInfo.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                battleCardInfo.transform.localScale = new Vector3(-1, -1, 1);
            }
        }
        

    }

    private void Update()
    {
        if (GetComponent<ThisCard>().cardMono == null)
            return;

        battleCardInfo.SetupBattleInfo(GetComponent<ThisCard>().cardMono.atk, GetComponent<ThisCard>().cardMono.life, GetComponent<ThisCard>().defaultAtk, GetComponent<ThisCard>().defaultLife);

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Card Border 
        if (Physics.Raycast(ray, out hit, 100f))
        {
            if (hit.transform.gameObject == this.gameObject)
            {
                GetComponent<ThisCard>().OnCardBorder();
            }
            else
            {
                GetComponent<ThisCard>().OffCardBorder();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            CancelBattlePopup();
            //Card Detail
            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.transform.gameObject == this.gameObject)
                {

                    if (!GetComponent<ThisCard>().isEnemyBack)
                        CardDetail.Instance.ShowCardDetail(this.GetComponent<ThisCard>().cardMono.id);
                    else
                        CardDetail.Instance.HideCardDetail();

                    //Để cho các lá bài kia hide trước, rồi mới gọi hàm Show Hand, nếu ko sẽ bị lỗi
                    Invoke("ShowBattlePopup", 0.1f);

                    GetComponent<ThisCard>().OnCardBorder();
                }
                else
                {
                    GetComponent<ThisCard>().OffCardBorder();
                }
            }
        }
        

        //Khi phía bên kia đã phát động tấn công, thì đây là lệnh chọn lá bài để tấn công bên side còn lại
        if (StateManager.Instance.DuelState == ActionState.fightingState)
        {
            //Player lúc này chỉ được chọn bài của đối thủ để tấn cônng
            if (Input.GetMouseButtonDown(0) && TurnManager.Instance.localSide != _battleZone.zoneSide)
            {
                if (Physics.Raycast(ray, out hit, 100f))
                {
                    if (hit.transform.gameObject == this.gameObject)
                    {
                        //Phát động tấn công
                        DeclareAttack(SelectionManager.Instance.battlePopup.side, SelectionManager.Instance.battlePopup.battleZoneIndex, SelectionManager.Instance.battlePopup.battleCard, _battleZone.zoneSide, index, GetComponent<ThisCard>());

                        StateManager.Instance.DuelState = ActionState.normalState;
                    }
                }
                
            }

        }

    }

    public void ShowBattlePopup()
    {
        //Khi đã tấn công rồi thì ko cho tấn công lại nữa
        if (Field_Manager_Id.Instance.zoneId[_battleZone.zoneSide].battleZoneAttacked[index] == true)
            return;

        if (StateManager.Instance.DuelState == ActionState.fightingState)
            return;

        //Nếu thỏa đk thì hiện lên Battle Popup khi click vào
        if (TurnManager.Instance.currentTurn == _battleZone.zoneSide && TurnManager.Instance.currentStep == TurnStep.BattleStep)
        {

            SelectionManager.Instance.battlePopup.SetupBattle(_battleZone.zoneSide, index, GetComponent<ThisCard>()); //Lấy index và side của người phát động Attack

            SelectionManager.Instance.OpenBattlePanel(_battleZone.zoneSide, index); //Hiện lên Panel
        }
    }

    public void CancelBattlePopup()
    {
        if (TurnManager.Instance.currentStep != TurnStep.BattleStep)
            return;

        //Nếu đã đang trong fighting State rồi thì ko cho hủy hành động này
        if (StateManager.Instance.DuelState == ActionState.fightingState)
            return;

        SelectionManager.Instance.HideBattlePanel();

        Debug.Log("Cancel " + Time.deltaTime);
    }

    public void DeclareAttack(int attackerSide, int attackerIndex, ThisCard attackerCard, int beAttackedSide, int beAttackedIndex, ThisCard beAttackedCard)
    {
        //Đã tấn công rồi
        Field_Manager_Id.Instance.SetAttackedBool(attackerSide, attackerIndex, true);

        Duel_VFX_Manager.Instance.StartAttackVFX(Field_Manager_Id.Instance.zoneId[attackerSide].battleZone[attackerIndex], Field_Manager_Id.Instance.zoneId[beAttackedSide].battleZone[beAttackedIndex], attackerSide);


        int damage = attackerCard.cardMono.atk - beAttackedCard.cardMono.atk;

        //Tính Damage
        if (damage > 0) //ATK Attacker > ATK beAttacked
        {
            //Tính HP
            int currHp = Field_Manager_Id.Instance.zoneId[beAttackedSide].healthPoint; 
            Field_Manager_Id.Instance.SetDuelistHP(beAttackedSide, currHp - damage);

            //Trừ Life
            FieldBattleManager.Instance.SetCardLife(beAttackedSide, beAttackedIndex, -1);
        }
        else if (damage < 0) //ATK Attacker < ATK beAttacked
        {
            //Tính HP
            int currHp = Field_Manager_Id.Instance.zoneId[attackerSide].healthPoint; 
            Field_Manager_Id.Instance.SetDuelistHP(attackerSide, currHp + damage);

            FieldBattleManager.Instance.SetCardLife(attackerSide, attackerIndex, -1);
        }
        else //Sức ATK bằng nhau
        {
            FieldBattleManager.Instance.SetCardLife(attackerSide, attackerIndex, -1);

            FieldBattleManager.Instance.SetCardLife(beAttackedSide, beAttackedIndex, -1);
        }

        //Kiểm tra nếu LIFE của lá bài nào về 0 thì bị drop
        FieldBattleManager.Instance.CheckLife(attackerSide, attackerIndex);
        FieldBattleManager.Instance.CheckLife(beAttackedSide, beAttackedIndex);

        Field_Manager_Id.Instance.Invoke("OnUpdateBattle", 0.3f);
    }


}
