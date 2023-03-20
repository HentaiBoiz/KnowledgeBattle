using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static Card;

public class TestCardManager : MonoBehaviour
{
    public Button findBtn;
    public TMP_InputField idInput;

    public ThisCard testCard;

    public CardType cardType;

    // Start is called before the first frame update
    void Start()
    {
        idInput.interactable = false;
        findBtn.interactable = false;

        Login();
    }

    public void Login()
    {

        string email = "trungnhancdps@gmail.com";
        string password = "boycdpscu";

        LoginWithEmailAddressRequest loginRequest = new LoginWithEmailAddressRequest
        {

            Password = password,
            Email = email,

        };
        PlayFabClientAPI.LoginWithEmailAddress(loginRequest, result => {
            
            Debug.Log("LOGIN COMPLETED !!!");
            PlayfabCardDB.Instance.LoadAllCardFromPlayfab();
            idInput.interactable = true;
            findBtn.interactable = true;
        },
        error => {
            Debug.Log(error.GenerateErrorReport());
            //ErrorsManager.Instance.PushError(error.GenerateErrorReport());
        });

    }

    public void FindCard()
    {

        string cardId = idInput.text;

        testCard.SetupCardPlayfabDB(cardId);

        testCard.cardImage.sprite =  Resources.Load<Sprite>($"CardImg/{cardType.ToString().Replace(" ", "")}/{cardId}");

    }

    //IEnumerator GetImageFromUrlAndAddCardToDb(string url, ThisCard loadCard)
    //{
    //    Debug.Log("START LOAD IMG");
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
    //        loadCard.cardImage.sprite = sprite;

    //        Debug.Log("LOAD IMG SUCCESS");

    //    }

    //}
}
