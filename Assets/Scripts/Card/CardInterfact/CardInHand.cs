using AutoLayout3D;
using UnityEngine;
using static StateManager;
using static TurnManager;

public class CardInHand : MonoBehaviour
{
    LayoutElement3D layoutElement3D;
    public int index; //Index trên hand

    private void Start()
    {
        layoutElement3D = GetComponent<LayoutElement3D>();
    }

    private void Update()
    {
        if (StateManager.Instance.DuelState != ActionState.normalState)
            return;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        if (Input.GetMouseButtonUp(0))
        {
            UnzoomHandCard();

            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.transform.gameObject == this.gameObject)
                {
                    ZoomHandCard();

                    if (!GetComponent<ThisCard>().isEnemyBack)
                    {
                        Invoke("InvokeOpenHandPanel", 0.1f);
                        CardDetail.Instance.ShowCardDetail(this.GetComponent<ThisCard>().cardMono.id);
                    }
                    else
                    {
                        CardDetail.Instance.HideCardDetail();
                    }

                }
            }

        }

        if (SelectionManager.Instance.handPopup.isActiveAndEnabled)
            return;

        if (Physics.Raycast(ray, out hit, 100f))
        {

            if (hit.transform.gameObject == this.gameObject)
            {
                ZoomHandCard();
            }
            else
            {
                UnzoomHandCard();
            }
        }
        else
        {
            UnzoomHandCard();
        }
    }

    public void ZoomHandCard()
    {
        layoutElement3D.center.z = -1.4f;
        this.transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);
        GetComponent<ThisCard>().OnCardBorder();
        
    }

    public void UnzoomHandCard()
    {
        layoutElement3D.center.z = 0;
        this.transform.localScale = new Vector3(1f, 1f, 1f);
        GetComponent<ThisCard>().OffCardBorder();

        SelectionManager.Instance.HideHandPanel();
    }

    public void InvokeOpenHandPanel() //Để cho các lá bài kia hide trước, rồi mới gọi hàm Show Hand, nếu ko sẽ bị lỗi
    {
        SelectionManager.Instance.OpenHandPanel(GetComponent<ThisCard>().cardMono, index);
    }
}
