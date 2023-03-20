using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using Unity.Scenes;
using UnityEngine;

public class ADDVC : MonoBehaviour
{

    public int AddScore;
    public int SubScore;
    // Start is called before the first frame update
    void Start()
    {
        Login();
       
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Login()
    {

        string email = "crongbi@gmail.com";
        string password = "crong123";

        LoginWithEmailAddressRequest loginRequest = new LoginWithEmailAddressRequest
        {

            Password = password,
            Email = email,

        };
        PlayFabClientAPI.LoginWithEmailAddress(loginRequest, this.LoginSuccess, this.OnPlayfabError);

    }



    protected virtual void LoginSuccess(LoginResult result)
    {

        Debug.Log("PlayfabID: " + result.PlayFabId);
        Debug.Log("SessionTicket:" + result.SessionTicket);

// AddVC(AddScore);
        SubVC(SubScore);

       // PlayfabUserInfomation.Instance.SetUserData(result.PlayFabId, result.SessionTicket);
       //PlayerInfoDDOL.Instance.SetAccountInfo(result.PlayFabId, "", result.SessionTicket); //Cần check lại chỗ này
    }


    #region PLAYFAB METHOD


    
    public void OnPlayfabError(PlayFabError error)
    {
        string textERR = error.GenerateErrorReport();
        Debug.LogWarning(textERR);

    }
    public void AddVC(int addrand)
    {

         addrand = Random.Range(20, 40);


        AddUserVirtualCurrencyRequest vcRequest = new AddUserVirtualCurrencyRequest
        {


            VirtualCurrency = "RS",
            Amount = addrand,
            
      
            

        };
        PlayFabClientAPI.AddUserVirtualCurrency(vcRequest, OnAddVCComplete, OnPlayfabError);

    }

    public void OnAddVCComplete(ModifyUserVirtualCurrencyResult VCi)
    {
        Debug.Log("Complete" + VCi.BalanceChange);


    }

    public void SubVC(int subrand)
    {
        subrand = Random.Range(20, 40);

        

        SubtractUserVirtualCurrencyRequest vc = new SubtractUserVirtualCurrencyRequest
        {
            VirtualCurrency = "RS",
              Amount = subrand,
           

        };
        PlayFabClientAPI.SubtractUserVirtualCurrency(vc, SubVCComplete, OnPlayfabError);

    }

    public void SubVCComplete(ModifyUserVirtualCurrencyResult VC)
    {
        Debug.Log("Complete" + VC.BalanceChange);
    }
    #endregion

}
