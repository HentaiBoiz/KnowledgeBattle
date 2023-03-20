using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInCheat : MonoBehaviour
{
    public int zoneId;

    public bool isActivateEffect = false;


    #region LOCAL VFX
   
    #endregion

    private void OnEnable()
    {
        isActivateEffect = false;
        GetComponent<ThisCard>().SetEnemyBack(); //Khi vừa set thì lá Cheat này sẽ bị ẩn đi
    }

    private void Update()
    {
        //Check Life Time
        if (GetComponent<ThisCard>().cardMono == null)
            return;

        if (GetComponent<ThisCard>().cardMono.type == Card.CardType.CheatCard)
        {
            if (GetComponent<ThisCard>().cardMono.timeLife <= 0 && isActivateEffect)
            {
                CheckCancelEffect(EffectManager.Instance);

                Field_Manager_Id.Instance.AddCardToDropZone(zoneId, GetComponent<ThisCard>().cardMono.id);
                Field_Manager_Id.Instance.zoneId[zoneId].cheatZone = "";

                GetComponent<ThisCard>().cardId = "";
                Field_Manager_Id.Instance.Invoke("OnUpdateCheat", 0.2f);
                Field_Manager_Id.Instance.Invoke("OnUpdateDrop", 0.2f);
                this.gameObject.SetActive(false);

            }
        }

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


        if (GetComponent<ThisCard>().cardMono.type != Card.CardType.CheatCard)
            return;

        if (isActivateEffect == true)
            return;

        //Show Cheat Popup
        if (Input.GetMouseButtonUp(0))
        {

            CancelCheatPopup();
            //Card Detail
            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.transform.gameObject == this.gameObject)
                {

                    if (zoneId == TurnManager.Instance.localSide)
                    {
                        CardDetail.Instance.ShowCardDetail(this.GetComponent<ThisCard>().cardMono.id);

                        //Nếu đang trong State cho phép kích hoạt bẫy và thỏa hết điều kiện thì kích hoạt
                        if(StateManager.Instance.DuelState == StateManager.ActionState.counterState && CanBeActivate())
                            Invoke("ShowCheatPopup", 0.1f);
                    }
                    else
                    {
                        CardDetail.Instance.HideCardDetail();
                    }    

                    GetComponent<ThisCard>().OnCardBorder();
                }
                else
                {
                    GetComponent<ThisCard>().OffCardBorder();
                }
            }
        }


    }

    public void CheckCheatEffect(EffectManager effectManager)
    {
        if (isActivateEffect == true)
            return;

        CheatEventManager.Instance.ActivatedCheat(zoneId);

        foreach (var effect in GetComponent<ThisCard>().cardMono.mainEffects)
        {
            effect.ExecuteEffect(effectManager);
        }

        //Reset Action Side
        EffectManager.Instance.SetActionSide(-1);
    }

    public void CheckCancelEffect(EffectManager effectManager)
    {

        foreach (var effect in GetComponent<ThisCard>().cardMono.mainEffects)
        {
            effect.CancelEffect(effectManager);
        }

    }

    //Popup
    public void ShowCheatPopup()
    {
        SelectionManager.Instance.OpenCheatPanel(this, zoneId);
    }

    public void CancelCheatPopup()
    {
        SelectionManager.Instance.HideCheatPanel();
    }

    //Checking Method
    public bool CanBeActivate()
    {
        foreach (var effect in GetComponent<ThisCard>().cardMono.mainEffects)
        {
            if (!effect.canBeActivate())
                return false;
        }

        return true;
    }
}
