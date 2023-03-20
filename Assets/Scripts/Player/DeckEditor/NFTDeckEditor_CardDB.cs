using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using Photon.Pun;

public class NFTDeckEditor_CardDB : MonoBehaviour
{
    public static NFTDeckEditor_CardDB Instance;

    //Network
    private List<CardContractAddress> cardContractAddresses = new List<CardContractAddress>();
    private Dictionary<string, Card> inventoryCards = new Dictionary<string, Card>();
    //Quản lí số lượng
    public Dictionary<string, int> cardCountCalculate = new Dictionary<string, int>(); //Biến này sẽ lưu số lượng các lá bài đã có trong deck kèm ID, sau đó tính toán để trừ đi amount trong inventory

    [Header("UI")]
    public Transform inventoryHolder;
    public GameObject inventoryCardObj;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        GetPlayerNFTInventory();
    }

    #region NFT Methods
    private void GetPlayerNFTInventory()
    {
        //Convert Catalog Item sang Card Contract Address
        foreach (var catalogContract in PlayfabCardDB.Instance.contractCatalogItemsDB)
        {
            cardContractAddresses.Add(new CardContractAddress(catalogContract.Value));
        }

        //Tìm kiếm trong kho đồ NFT của Player có lá bài nào
        foreach (var cardContractAddress in cardContractAddresses)
        {
            FindNFTInventory(cardContractAddress);
        }

    }

    //Tìm kiếm trong kho đồ NFT của Player có lá bài nào
    async void FindNFTInventory(CardContractAddress cardContractAddress)
    {
        string chain = "ethereum";
        string network = "goerli";
        string contract = cardContractAddress.contractAddress;
        string account = PlayerPrefs.GetString("Account");
        //string tokenId = "0x01559ae4021aee70424836ca173b6a4e647287d15cee8ac42d8c2d8d128927e5";

        foreach (var cardId in cardContractAddress.cardIds)
        {
            BigInteger balanceOf = await ERC1155.BalanceOf(chain, network, contract, account, cardId);

            //Nếu tìm thấy Card Id này trong kho đồ NFT của Player 
            if (balanceOf > 0)
            {
                Card tempCard = PlayfabCardDB.Instance.FindCardById(cardId);

                inventoryCards.Add(cardId, tempCard);

                CreateInventoryCardUI(tempCard, balanceOf.ToString());
            }

        }

        if (DeckInterfact.Instance.deckJsons.Count > 0)
        {
            DeckInterfact.Instance.CreateDeckCard2D(PlayfabUserInfomation.Instance.playerData.currDeckSlot);
        }
    }
    #endregion


    #region Normal Methods
    //Tạo ra Card UI trong Inventory
    public void CreateInventoryCardUI(Card card, string amount)
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
}
