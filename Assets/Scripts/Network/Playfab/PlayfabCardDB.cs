using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class PlayfabCardDB : MonoBehaviour
{

    public static PlayfabCardDB Instance;

    //Catalog Item
    public Dictionary<string, CatalogItem>  cardCatalogItemsDB = new Dictionary<string, CatalogItem>();
    public Dictionary<string, CatalogItem> contractCatalogItemsDB = new Dictionary<string, CatalogItem>(); //NFT Contract Address
    public Dictionary<string, CatalogItem> packCatalogItemsDB = new Dictionary<string, CatalogItem>();


    //Card Database
    public Dictionary<string, Card> playfabCardDB = new Dictionary<string, Card>();


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        Instance = this;
    }

    #region NETWORK METHOD
    //Khi Player đăng nhập xong thì Load hết Card và Pack Info từ Server về
    public void LoadAllCardFromPlayfab()
    {
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest
        {
            CatalogVersion = "Knowledge Card"
        },
        result => {
            foreach (CatalogItem item in result.Catalog)
            {
                if (item.ItemId.Contains("pack"))
                {
                    packCatalogItemsDB.Add(item.ItemId.ToString(), item);
                }
                else
                {
                    cardCatalogItemsDB.Add(item.ItemId.ToString(), item);
                }
            }
            Debug.Log("LOAD SUCCESS");
        },
        error => {
            Debug.Log(error.GenerateErrorReport());
            ErrorsManager.Instance.PushError(error.GenerateErrorReport());
        });
    }

    public void LoadAllContractFromPlayfab()
    {
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest
        {
            CatalogVersion = "NFT Address"
        },
        result => {
            foreach (CatalogItem contract in result.Catalog)
            {
                contractCatalogItemsDB.Add(contract.ItemId.ToString(), contract);
                
            }
            Debug.Log("LOAD CONTRACT ADDRESS SUCCESS");
        },
        error => {
            Debug.Log(error.GenerateErrorReport());
            ErrorsManager.Instance.PushError(error.GenerateErrorReport());
        });
    }

    public Card FindCardById(string id) //Kiểm tra thử trong DB, img của lá bài này đã được load chưa, chưa thì phải load
    {
        if (!cardCatalogItemsDB.ContainsKey(id))
        {
            Debug.LogError("Can't find card with Id: " + id);
            return null;
        } //Kiểm tra trong Catalog DB trước

        //Kiểm tra và Load ảnh Card
        if (playfabCardDB.ContainsKey(id)) //Chưa load card này về
        {
            //if (playfabCardDB[id].cardSprite == null)
            //{
            //    //string imgUrl = cardCatalogItemsDB[id].ItemImageUrl.ToString();
            //    //StartCoroutine(GetImageFromUrlAndAddCardToDb(imgUrl, playfabCardDB[id]));
            //}

            return playfabCardDB[id];
        }
        else
        {
            Card newCard = new Card(cardCatalogItemsDB[id]);

            //Lệnh này sẽ bị chạy chậm hơn mấy lệnh khác nhưng chắc ko sao
            //string imgUrl = cardCatalogItemsDB[id].ItemImageUrl.ToString();
            //StartCoroutine(GetImageFromUrlAndAddCardToDb(imgUrl, newCard));

            playfabCardDB.Add(newCard.id, newCard);

            return playfabCardDB[newCard.id];
        }

    }

    public int ReturnCardLvById(string id)
    {
        if (!cardCatalogItemsDB.ContainsKey(id))
        {
            Debug.LogError("Can't find card with Id: " + id);
            return -1;
        } //Kiểm tra trong Catalog DB trước

        //Gán dữ liệu
        var customData = JsonConvert.DeserializeObject<Dictionary<string, string>>(cardCatalogItemsDB[id].CustomData.ToString());

        int level = int.Parse(customData["Level"].ToString());

        return level;
    }

   

    //IEnumerator GetImageFromUrlAndAddCardToDb(string url, Card loadCard)
    //{
    //    //WWW www = new WWW(url);
    //    //yield return www;

    //    UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
    //    yield return request.SendWebRequest();

    //    if (request.result != UnityWebRequest.Result.Success)
    //    {
    //        Debug.Log(request.error);
    //        ErrorsManager.Instance.PushError(request.error);
    //    }
    //    else
    //    {

    //        Texture2D cardTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;

    //        Sprite sprite = Sprite.Create(cardTexture, new Rect(0, 0, cardTexture.width, cardTexture.height), Vector2.zero);
    //        loadCard.cardSprite = sprite;

    //    }

    //}
    #endregion
}
