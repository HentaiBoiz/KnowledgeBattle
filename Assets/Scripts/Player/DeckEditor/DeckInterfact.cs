using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DeckJson
{
    public string deckName;
    public List<string> cardIds;

    public DeckJson()
    {

    }

    public DeckJson(string deckName, List<string> cardIds)
    {
        this.deckName = deckName;
        this.cardIds = cardIds;
    }

    public DeckJson(DeckJson deckJson)
    {
        this.deckName = deckJson.deckName;
        this.cardIds = deckJson.cardIds;
    }
}

public class DeckInterfact : MonoBehaviour
{

    public static DeckInterfact Instance;

    public List<DeckJson> deckJsons = new List<DeckJson>();

    [Header("UI")]
    public TMP_InputField deckNameInput;
    public DeckDropdownHandler slotDropDown;
    public Transform cardDeckHolder;
    public GameObject deckCard2DObj;
    public TMP_Text amountCard;

    //Limit Card in Deck
    public int maximumCard = 60;
    public int minimumCard = 40;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LoadDeckFromPlayfab();
    }
    private void Update()
    {
        CountCardInDeck(); // Can check lai hieu nang cua Game roi fix!
    }


    #region NORMAL METHOD
    public void AddCardToDeck(string id)
    {

        if (cardDeckHolder.transform.childCount >= maximumCard)
        {
            return;
        }
        deckJsons[PlayfabUserInfomation.Instance.playerData.currDeckSlot].cardIds.Add(id);
        DeckEditorCardDB.Instance.ModifyCardCountCalculate(-1, id);
        CreateADeckCard2D(id);

        Invoke("ReassignCardIndex", 0.1f);
        Invoke("CountCardInDeck", 0.1f);

        Debug.Log(id + "was added to Deck");

    }

    public void RemoveCardFromDeck(int index, string id)
    {
        DeckEditorCardDB.Instance.ModifyCardCountCalculate(1, id);
        deckJsons[PlayfabUserInfomation.Instance.playerData.currDeckSlot].cardIds.RemoveAt(index);
        Destroy(cardDeckHolder.GetChild(index).gameObject);
        Invoke("ReassignCardIndex", 0.1f);
        Invoke("CountCardInDeck", 0.1f);

        Debug.Log(id + "was removed from Deck");

    }

    public void ClearCurrentDeck()
    {
        //Làm sạch holder
        foreach (Transform card2D in cardDeckHolder)
        {
            DeckEditorCardDB.Instance.ModifyCardCountCalculate(1, card2D.GetComponent<DeckCard2D>().cardId);
            Destroy(card2D.gameObject);
        }

        deckJsons[PlayfabUserInfomation.Instance.playerData.currDeckSlot].cardIds = new List<string>();

    }

    public void CreateDeckCard2D(int deckIndex)
    {
        //Làm sạch holder
        foreach (Transform card2D in cardDeckHolder)
        {
            Destroy(card2D.gameObject);
        }

        foreach (var id in deckJsons[deckIndex].cardIds)
        {
            GameObject g = Instantiate(deckCard2DObj, cardDeckHolder.transform);
            g.transform.SetParent(cardDeckHolder);

            g.GetComponent<DeckCard2D>().SetupCardUI(PlayfabCardDB.Instance.playfabCardDB[id]);

        }

        Invoke("ReassignCardIndex", 0.1f);
        deckNameInput.text = deckJsons[deckIndex].deckName;
    }
    public void CreateADeckCard2D(string cardId)
    {
        GameObject g = Instantiate(deckCard2DObj, cardDeckHolder.transform);
        g.transform.SetParent(cardDeckHolder);

        g.GetComponent<DeckCard2D>().SetupCardUI(PlayfabCardDB.Instance.playfabCardDB[cardId]);

    }

    public void ChangeDeckSlot(int index)
    {
        PlayfabUserInfomation.Instance.playerData.currDeckSlot = index;

        PlayfabUserInfomation.Instance.UploadPlayerData();

        CreateDeckCard2D(PlayfabUserInfomation.Instance.playerData.currDeckSlot);
    }

    //Khi sort lại Deck thì gán index lại cho bài
    public void ReassignCardIndex()
    {
        int i = 0;

        foreach (Transform item in cardDeckHolder)
        {
            item.GetComponent<DeckCard2D>().cardIndex = i;
            i++;
        }
    }

    public void ChangeDeckName()
    {
        deckJsons[PlayfabUserInfomation.Instance.playerData.currDeckSlot].deckName = deckNameInput.text;
    }

    //Đếm số lượng Card trong Deck
    public void CountCardInDeck()
    {
        amountCard.text = cardDeckHolder.transform.childCount.ToString() + "/60";
    }
    #endregion


    #region PLAYFAB METHOD
    public void SaveDeckToPlayfab()
    {
        //Kiểm tra điều kiện
        if (cardDeckHolder.transform.childCount < minimumCard)
        {
            Debug.Log("Deck much have " + minimumCard + " card to save");
            return ;
        }

        //Bắt đầu đưa dữ liệu Deck lên Playfab
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { "Decks", JsonConvert.SerializeObject(deckJsons) }
            },

            Permission = UserDataPermission.Public

        };
        PlayFabClientAPI.UpdateUserData(request, result =>
        {

            Debug.Log("UPLOAD DECK SUCCESS");
        },
        error =>
        {
            Debug.Log(error.GenerateErrorReport());
            ErrorsManager.Instance.PushError(error.GenerateErrorReport());
        });
    }

    public void LoadDeckFromPlayfab()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
        {
            if (result.Data != null && result.Data.ContainsKey("Decks"))
            {
                deckJsons = JsonConvert.DeserializeObject<List<DeckJson>>(result.Data["Decks"].Value);
                Debug.Log("LOAD DECK SUCCESS");

                //Tính toán lượng card bị lấy đi
                DeckEditorCardDB.Instance.GetCardCountCalculate();
                for (int i = 0; i < deckJsons.Count; i++)
                {
                    slotDropDown.AddDeckOptions(i);
                }

            }
            else
            {
                deckNameInput.text = "New Deck";
                deckJsons.Add(new DeckJson(deckNameInput.text, new List<string>()));

                Debug.Log("NO DECK WAS FOUNDED ! CREATE A NEW DECK");
            }

            slotDropDown.SetStartOption(PlayfabUserInfomation.Instance.playerData.currDeckSlot);

        },
        error =>
        {
            Debug.Log(error.GenerateErrorReport());
            ErrorsManager.Instance.PushError(error.GenerateErrorReport());
        });
    }
    #endregion
}
