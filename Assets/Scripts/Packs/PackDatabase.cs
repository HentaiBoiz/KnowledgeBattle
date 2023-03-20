using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static Pack;

public class PackDatabase : MonoBehaviour
{
    [Serializable]
    public struct PackTypeList
    {
        public PackType packType;
        public List<Pack> listPack;
        
    }

    public static PackDatabase Instance { get; private set; }

    //public List<SOPack> soPacksDatabase = new List<SOPack>(); //List chứ mảng các lá bài dạng Scriptable Object

    //public List<Pack> packsDatabase = new List<Pack>();
    public List<PackTypeList> packTypeLists = new List<PackTypeList>();

    //Loading Screen
    public LoadIntoStore loadIntoStore;

    private void Awake()
    {

        GetStoreItems();
        
        Instance = this;
    }

    private void Start()
    {
        loadIntoStore.gameObject.SetActive(true);
    }

    #region NORMAL METHOD
    //Nếu đã có loại pack này trong list rồi thì không cần tạo lại
    public bool CheckIfHaveType(PackType packType)
    {
        foreach (var item in packTypeLists)
        {
            if(item.packType == packType)
            {
                return true;
            }
        }

        return false;
    }

    public void AddPackToDatabase(Pack pack)
    {
        foreach (var item in packTypeLists)
        {
            if (item.packType == pack.packType)
            {
                item.listPack.Add(pack);
            }
        }

    }

    //Trả về List có chứa các loại bài đó
    public List<Pack> ReturnPackType(string type)
    {
        PackType temp = Enum.Parse<PackType>(type);

        foreach (var item in packTypeLists)
        {
            if (item.packType == temp)
            {
                return item.listPack;
            }
        }

        return null;
    }

    public Pack FindPackWithIdOnly(string id)
    {

        foreach (var packType in packTypeLists)
        {
            foreach (var pack in packType.listPack)
            {
                if (pack.id == id)
                    return pack;
            }
        }

        Debug.LogError("Can't find pack with Id: " + id);
        return null;
    }

    public Pack FindPackWithIdnType(string id, string type)
    {
        PackType temp = Enum.Parse<PackType>(type);

        foreach (var pack in ReturnPackType(type))
        {
            if (pack.id == id)
                return pack;
        }

        Debug.LogError("Can't find pack with Id: " + id + " and have Type: " + type);
        return null;
    }

    //public void SetupLocal()
    //{
    //    foreach (SOPack soPack in soPacksDatabase)
    //    {
    //        if (CheckIfHaveType(soPack.packType))
    //        {
    //            AddPackToDatabase(new Pack(soPack));
    //        }
    //        else
    //        {
    //            packTypeLists.Add(new PackTypeList() { packType = soPack.packType, listPack = new List<Pack>() }); //Thêm list mới với loại pack này

    //            AddPackToDatabase(new Pack(soPack));
    //        }

    //    }
    //}

    #endregion


    #region PLAYFAB METHOD

    //Lấy tất cả Pack trong Store về
    public void GetStoreItems()
    {
        PlayFabClientAPI.GetStoreItems(new GetStoreItemsRequest
        {
            CatalogVersion = "Knowledge Card",
            StoreId = "001_vending_machine"
         
        },
        result =>
        {
            foreach (var item in result.Store)
            {

                var packData = JsonConvert.DeserializeObject<Dictionary<string, string>>(item.CustomData.ToString());

                PackType type = Enum.Parse<PackType>(packData["PackType"].ToString());

                #region CREATE NEW PACK
                Pack tempPack = new Pack();

                tempPack.id = item.ItemId;
                tempPack.packName = packData["DisplayName"].ToString();

                tempPack.image = null;
                string imgUrl = PlayfabCardDB.Instance.packCatalogItemsDB[item.ItemId].ItemImageUrl.ToString();
                StartCoroutine(GetPackImageFromUrl(imgUrl, tempPack));

                tempPack.description = packData["Description"].ToString();
                tempPack.packType = type;
                tempPack.price =int.Parse(item.VirtualCurrencyPrices["KC"].ToString());
                #endregion

                loadIntoStore.AddToTempIdPack(item.ItemId);

                //Phân loại Pack
                if (CheckIfHaveType(tempPack.packType))
                {
                    AddPackToDatabase(tempPack);
                }
                else
                {
                    packTypeLists.Add(new PackTypeList() { packType = tempPack.packType, listPack = new List<Pack>() }); //Thêm list mới với loại pack này

                    AddPackToDatabase(tempPack);
                }

            }

            loadIntoStore.StartLoading_PackImg();
            
        },
        error =>
        {
            Debug.Log(error.GenerateErrorReport());
        });
    }


    #endregion

    IEnumerator GetPackImageFromUrl(string url, Pack loadPack)
    {
        //WWW www = new WWW(url);
        //yield return www;

        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
            ErrorsManager.Instance.PushError(request.error);
        }
        else
        {

            Texture2D packTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;

            Sprite sprite = Sprite.Create(packTexture, new Rect(0, 0, packTexture.width, packTexture.height), Vector2.zero);
            loadPack.image = sprite;

        }

    }
}
