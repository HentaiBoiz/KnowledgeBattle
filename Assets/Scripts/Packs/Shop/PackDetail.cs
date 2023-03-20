using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PackDetail : MonoBehaviour
{
    public static PackDetail Instance;

    public Pack currPackSelect = null;

    //Animation
    [HideInInspector]
    public List<Card> lastCardsUnpack = new List<Card>(); //Dùng làm Animation
    public UnpackAnimation unpackAnimation;

    #region UI
    public TextMeshProUGUI packNameTxt;
    public TextMeshProUGUI packDesTxt;
    public TextMeshProUGUI packPriceTxt;
    public Image packImage;
    public Image packLayout; //Đổi color theo loại Pack
    public Button buyButton;
    public TextMeshProUGUI playerMoney;
    #endregion

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        unpackAnimation.gameObject.SetActive(false);
        GetVirtualCurrency();
    }


    public void SetupPackDetail(Pack pack)
    {
        currPackSelect = pack;

        buyButton.gameObject.SetActive(true);

        packNameTxt.text = pack.packName;
        packDesTxt.text = pack.description;
        packPriceTxt.text = "Price: " + pack.price.ToString() + " KC";

        packImage.sprite = pack.image;

        PackColor packColor = new PackColor();
        packLayout.color = packColor.ReturnPackColor(pack.packType.ToString());
    }

    public void BuyPack()
    {
        PurchaseItemRequest request = new PurchaseItemRequest
        {
            ItemId = currPackSelect.id,
            CatalogVersion = "Knowledge Card",
            VirtualCurrency = "KC",
            Price = currPackSelect.price
        };

        PlayFabClientAPI.PurchaseItem(request, OnPurchaseSuccess, LoginErr);


    }

    protected virtual void OnPurchaseSuccess(PurchaseItemResult result)
    {
        Debug.Log("Purchase Success");
        Debug.Log(result.Items.Count);

        lastCardsUnpack = new List<Card>();


        foreach (var item in result.Items)
        {
            Debug.Log(item.ItemId);
            if (!item.ItemId.Contains("pack"))
            {
                Debug.Log("Added " + item.ItemId);
                for (int i = 0; i < item.UsesIncrementedBy; i++)
                {
                    lastCardsUnpack.Add(PlayfabCardDB.Instance.FindCardById(item.ItemId));
                }
            }

        }

        GetVirtualCurrency();

        //Show Animation
        unpackAnimation.SetupPackToShow(packImage.sprite, packLayout.color);
        unpackAnimation.gameObject.SetActive(true);

    }

    protected virtual void LoginErr(PlayFabError error)
    {
        string textERR = error.GenerateErrorReport();
        Debug.LogWarning(textERR);
    }

    //Get Current Knowledge Coin
    public void GetVirtualCurrency()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), OnGetUserInventorySuccess, OnRequestError);
    }

    public void OnGetUserInventorySuccess(GetUserInventoryResult result)
    {
        int kcoin = result.VirtualCurrency["KC"];
        playerMoney.text = kcoin.ToString();

    }

    public void OnRequestError(PlayFabError error)
    {
        string textERR = error.GenerateErrorReport();
        Debug.LogWarning(textERR);
    }
}
