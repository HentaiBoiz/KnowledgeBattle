using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StateManager;

public class CardInQueue : MonoBehaviour
{
    public int index;

    public List<Card> attachingCards = new List<Card>(); //List những lá bài đang Attach vào vùng Queue này

    public bool isActivateEffect = false;

    Queue parentQueue;

    #region LOCAL VFX
    public GameObject attachVFX; //Khi Player lựa bài để attach thì sẽ bật VFX này lên
    #endregion

    private void Start()
    {
        parentQueue = GetComponentInParent<Queue>();
        isActivateEffect = false;
        attachVFX.gameObject.SetActive(false);
    }

    private void Update()
    {
        //Check Life Time
        if (GetComponent<ThisCard>().cardMono == null)
            return;

        if(GetComponent<ThisCard>().cardMono.type == Card.CardType.SupportCard)
        {
            if(GetComponent<ThisCard>().cardMono.timeLife <= 0)
            {
                CheckCancelEffect(EffectManager.Instance);

                Field_Manager_Id.Instance.AddCardToDropZone(parentQueue.zoneSide, GetComponent<ThisCard>().cardMono.id);
                Field_Manager_Id.Instance.zoneId[parentQueue.zoneSide].queueZone[index] = "";

                GetComponent<ThisCard>().cardId = "";
                Field_Manager_Id.Instance.Invoke("OnUpdateQueue", 0.2f);
                Field_Manager_Id.Instance.Invoke("OnUpdateDrop", 0.2f);
                this.gameObject.SetActive(false);

            }
        }


        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Card Detail
        if (Physics.Raycast(ray, out hit, 100f))
        {
            if (hit.transform.gameObject == this.gameObject)
            {
                //Debug.DrawLine(ray.origin, hit.point, Color.red, 2f);
                if (Input.GetMouseButtonDown(0))
                {
                    if (!GetComponent<ThisCard>().isEnemyBack)
                        CardDetail.Instance.ShowCardDetail(this.GetComponent<ThisCard>().cardMono.id);
                    else
                        CardDetail.Instance.HideCardDetail();
                }

                GetComponent<ThisCard>().OnCardBorder();
            }
            else
            {
                GetComponent<ThisCard>().OffCardBorder();
            }
        }

        if (StateManager.Instance.DuelState == ActionState.normalState) //Trạng thái bình thường
        {
            attachVFX.gameObject.SetActive(false);
            return;
        }
            

        if (parentQueue.zoneSide != TurnManager.Instance.localSide)
            return;

        if (PhotonNetwork.IsMasterClient)
        {
            if (!Question_Manager_Id.Instance.isCanAttachMore(0, index))
                return;
        }
        else
        {
            if (!Question_Manager_Id.Instance.isCanAttachMore(1, index))
                return;
        }

        if (StateManager.Instance.DuelState == ActionState.attachState) //Trạng thái đang chọn để Attach
        {
            attachVFX.gameObject.SetActive(true);

            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.transform.gameObject == this.gameObject)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        Debug.Log("CHOOSED");
                        SelectionManager.Instance.ShowResourcesSelection(index); //Hiện bảng cho chọn Resources
                    }

                }

            }
            return;
        }
         
    }

    public void CheckQueueEffect(EffectManager effectManager)
    {
        if (TurnManager.Instance.localSide == EffectManager.Instance.actionSide)
        {

            foreach (var effect in GetComponent<ThisCard>().cardMono.queueEffects)
            {
                effect.ExecuteEffect(effectManager);
            }
        }

        //Reset Action Side
        EffectManager.Instance.SetActionSide(-1);
    }

    public void CheckCancelEffect(EffectManager effectManager)
    {
        foreach (var effect in GetComponent<ThisCard>().cardMono.queueEffects)
        {
            effect.CancelEffect(effectManager);
        }

    }
}
