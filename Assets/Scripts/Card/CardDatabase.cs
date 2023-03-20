using System.Collections.Generic;
using UnityEngine;

public class CardDatabase : MonoBehaviour
{
    public static CardDatabase Instance { get; private set; }

    public List<SOCard> soCardsDatabase = new List<SOCard>(); //List chứ mảng các lá bài dạng Scriptable Object

    public List<Card> cardsDatabase = new List<Card>();

    //Kiểm tra và thực hiện việc Load Card
    #region CHECK LOAD CARD
    private List<string> tempCardIds = null;
    public LoadIntoDuel loadIntoDuel;
    private bool isStartSetupCard = false; //Biến này để ngăn không cho load card nhiều lần
    #endregion

    private void Awake()
    {
        Instance = this;
        loadIntoDuel.gameObject.SetActive(true);
    }

    public Card FindCardWithId(string id)
    {
        foreach (var card in cardsDatabase)
        {
            if (card.id == id)
                return new Card(card);
        }

        return null;
    }


    public void StartSetupCard()
    {
        if (isStartSetupCard)
            return;

        isStartSetupCard = true;

        tempCardIds = new List<string>();
        

        foreach (var duelist in PhotonDuelistStats.Instance.duelists)
        {
            foreach (var id in duelist.deckCardIds)
            {
                if(!tempCardIds.Contains(id))
                    tempCardIds.Add(id);
                
            }
        }

        //Load bộ câu hỏi dựa trên level trong Deck của Player
        QuestionDatabase.Instance.StartLoadQuestionCatalog(tempCardIds);

        loadIntoDuel.StartLoading_CardImg(tempCardIds);

        foreach (var id in tempCardIds)
        {
           // Card newCard = new Card(PlayfabCardDB.Instance.FindCardById(id),Resources.Load<SOCard>($"SOCards/{id}"));

            cardsDatabase.Add(new Card(PlayfabCardDB.Instance.FindCardById(id), Resources.Load<SOCard>($"SOCards/{id}")));
        }
    }

}
