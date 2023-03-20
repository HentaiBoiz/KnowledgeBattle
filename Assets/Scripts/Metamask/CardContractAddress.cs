using Newtonsoft.Json;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardContractAddress
{
    #region Convert Variables
    public string contractAddress;
    public string[] cardIds; 
    #endregion


    public CardContractAddress()
    {

    }

    //Chuyển đổi Catalog Item sang Card Contract Address
    public CardContractAddress(CatalogItem catalogContract)
    {
        //Gán dữ liệu
        var customData = JsonConvert.DeserializeObject<Dictionary<string, string>>(catalogContract.CustomData.ToString());

        //Setup
        contractAddress = catalogContract.ItemId;
        cardIds = customData.Keys.ToArray(); //Chỉ lấy List các Keys (nghĩa là Card Id)
    }
}
