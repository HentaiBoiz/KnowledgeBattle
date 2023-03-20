using System.Collections.Generic;
using UnityEngine;

public class DropZone : MonoBehaviour
{
    public int dropSide; //0: Host, 1: Client

    public Transform topCard;
    GameObject cloneCard;

    public List<Transform> cardsInDrop;

    private void Start()
    {

        cardsInDrop = new List<Transform>();

        foreach (Transform child in this.transform)
        {
            cardsInDrop.Add(child);
        }

    }

    public void UpdateDropUI(int size)
    {
        int currDrop = Field_Manager_Id.Instance.zoneId[dropSide].dropZone.Count;

        //Top card
        if(currDrop > 0)
        {
            topCard.gameObject.SetActive(true);
            topCard.GetComponent<ThisCard>().SetupCard(Field_Manager_Id.Instance.zoneId[dropSide].dropZone[currDrop - 1]); //Lấy giá trị của Top card
        }
        else
        {
            topCard.GetComponent<ThisCard>().cardMono = null;
            topCard.gameObject.SetActive(false);
        }

        for (int i = 0; i < cardsInDrop.Count - 1; i++)
        {
            cardsInDrop[i].gameObject.SetActive(true);

            if (i < currDrop - 1)
            {
                Card temp = CardDatabase.Instance.FindCardWithId(Field_Manager_Id.Instance.zoneId[dropSide].dropZone[i]);

                //Lấy mã màu từ thẻ bài
                Color newColor;
                ColorUtility.TryParseHtmlString("#" + temp.templateColor, out newColor);
                cardsInDrop[i].GetComponentInChildren<SpriteRenderer>().color = newColor;

            }
            else
            {
                cardsInDrop[i].gameObject.SetActive(false);
            }
        }

    }
}
