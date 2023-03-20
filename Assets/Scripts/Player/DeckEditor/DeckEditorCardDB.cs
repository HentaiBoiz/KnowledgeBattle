using Newtonsoft.Json;
using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;
using static Card;

//Script quản lí các lá bài trong Deck Editor
public class DeckEditorCardDB : MonoBehaviour
{
    public static DeckEditorCardDB Instance;

    //Network
    private Dictionary<string, ItemInstance> inventoryPlayfab = new Dictionary<string, ItemInstance>();
    public Dictionary<string, int> cardCountCalculate = new Dictionary<string, int>(); //Biến này sẽ lưu số lượng các lá bài đã có trong deck kèm ID, sau đó tính toán để trừ đi amount trong inventory

    //Local
    public Dictionary<string, Card> inventoryCards = new Dictionary<string, Card>();

    [Header("UI")]
    public Transform inventoryHolder;
    public GameObject inventoryCardObj;
    public LoadIntoDeckEditor loadingScreen;

    //Bool
    private bool isLoadingCard = false;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //loadingScreen.gameObject.SetActive(true);
        GetPlayerInventory();
    }

    //private void Update()
    //{

    //    if (isLoadingCard == false)
    //        return;

    //    if (tempInventoryCardId.Count <= 0)
    //        return;

    //    //foreach (var id in tempInventoryCardId)
    //    //{
    //    //    if(inventoryCards[id].cardSprite != null)
    //    //    {
    //    //        CreateInventoryCardUI(inventoryCards[id], inventoryPlayfab[id].RemainingUses.ToString());
    //    //        tempInventoryCardId.Remove(id);
    //    //        break;
    //    //    }
    //    //}

        
        
    //}

    #region NORMAL METHOD
    public void CreateInventoryCardUI(Card card ,string amount)
    {
        GameObject g = Instantiate(inventoryCardObj, inventoryHolder.transform);
        g.transform.SetParent(inventoryHolder);

        g.GetComponent<InventoryCard>().SetupCardUI(card);

        g.GetComponent<InventoryCard>().cardCount = int.Parse(amount);
        g.GetComponent<InventoryCard>().cardCountTxt.text = amount;
    }

    //Lúc load JsonDeck xong thì thực hiện hàm này
    public void GetCardCountCalculate()
    {
        foreach (var deck in DeckInterfact.Instance.deckJsons)
        {
            foreach (var cardId in deck.cardIds)
            {
                //Đếm trong Deck, có bài thì trừ đi 1 trong Inventory
                ModifyCardCountCalculate(-1, cardId);
            }
        }
    }

    public void ModifyCardCountCalculate(int amount, string cardId)
    {
        if (cardCountCalculate.ContainsKey(cardId))
        {
            cardCountCalculate[cardId] += amount;
        }
        else
        {
            cardCountCalculate.Add(cardId, amount);
        }
    }

    public void ExitDeckEditor()
    {
        PhotonNetwork.LoadLevel("1_OfflineScene");
    }
    #endregion

    #region PLAYFAB METHOD
    public void GetPlayerInventory()
    {
        //Setup trạng thái
        isLoadingCard = false;

        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
        result => {

            foreach (ItemInstance item in result.Inventory)
            {
                inventoryPlayfab.Add(item.ItemId, item); //Add vào DB inventory của User - ItemInstance

                inventoryCards.Add(item.ItemId,PlayfabCardDB.Instance.FindCardById(item.ItemId)); //Add vào DB inventory của User - Card
            }

            //tempInventoryCardId = new List<string>();

            //foreach (var item in inventoryCards)
            //{
            //    tempInventoryCardId.Add(item.Key);
            //}

            //loadingScreen.StartLoading_CardImg(tempInventoryCardId);

            //Tạo Card UI trong Deck và Inventory
            foreach (var card in inventoryCards)
            {
                CreateInventoryCardUI(inventoryCards[card.Key], inventoryPlayfab[card.Key].RemainingUses.ToString());
            }

            if (DeckInterfact.Instance.deckJsons.Count > 0)
            {
                DeckInterfact.Instance.CreateDeckCard2D(PlayfabUserInfomation.Instance.playerData.currDeckSlot);
            }

            

        },
        error => {
            Debug.Log(error.GenerateErrorReport());
        });
    }

   

    #endregion


    
}
