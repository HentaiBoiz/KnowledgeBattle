using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public int handSide; //0: Host, 1: Client

    public List<Transform> cardsInHand;

    GameObject prefabCard;

    private void Start()
    {
        prefabCard = transform.GetChild(0).gameObject;
        for (int i = 0; i < 10; i++)
        {
            Instantiate(prefabCard, this.transform);
        }

        cardsInHand = new List<Transform>();

        int j = 0;

        foreach (Transform child in this.transform)
        {
            cardsInHand.Add(child);
            child.GetComponent<CardInHand>().index = j;
            j++;
        }
    }


    public void UpdateHandUI(int size)
    {
        int currHand = Field_Manager_Id.Instance.zoneId[handSide].handZone.Count;

        for (int i = 0; i < cardsInHand.Count; i++)
        {
            cardsInHand[i].GetComponent<ThisCard>().cardMono = null;
            cardsInHand[i].gameObject.SetActive(true);

            if(i < currHand)
            { 
                cardsInHand[i].GetComponent<ThisCard>().SetupCard(Field_Manager_Id.Instance.zoneId[handSide].handZone[i]);
                CheckBack(cardsInHand[i].GetComponent<ThisCard>());
            }
            else
            {
                cardsInHand[i].gameObject.SetActive(false);
            }
        }

    }

    public void CheckBack(ThisCard thisCard)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if(handSide == 0)
            {
                thisCard.UnsetEnemyBack();
            }
            else
            {
                thisCard.SetEnemyBack();
            }

        }
        else
        {
            if (handSide == 1)
            {
                thisCard.UnsetEnemyBack();
            }
            else
            {
                thisCard.SetEnemyBack();
            }
        }
    }
}
