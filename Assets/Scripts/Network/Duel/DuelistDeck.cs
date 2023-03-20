using AutoLayout3D;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DuelistDeck : MonoBehaviour
{
    public int deckSide; //0: Host, 1: Client

    public List<Transform> cardsInDeck;

    public TextMeshPro deckCountTxt;

    private void Start()
    {
        cardsInDeck = new List<Transform>();

        foreach (Transform child in this.transform)
        {
            cardsInDeck.Add(child);
        }
    }

    public void UpdateDeckUI(int size)
    {
        for (int i = 0; i < cardsInDeck.Count; i++)
        {
            cardsInDeck[i].gameObject.SetActive(true);
            if (i > size)
            {
                cardsInDeck[i].gameObject.SetActive(false);
            }
        }

        deckCountTxt.text = size.ToString();
    }
}
