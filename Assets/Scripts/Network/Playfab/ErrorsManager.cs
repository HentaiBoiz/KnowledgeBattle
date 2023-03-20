using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

[Serializable]
public class ErrorMessage
{
    public string error;
    public string timeStamp;

    public ErrorMessage() { }
    public ErrorMessage(string error, string timeStamp)
    {
        this.error = error;
        this.timeStamp = timeStamp;
    }
    public ErrorMessage(ErrorMessage errorMessage)
    {
        this.error = errorMessage.error;
        this.timeStamp = errorMessage.timeStamp;
    }
}

public class ErrorsManager : MonoBehaviour
{
    public static ErrorsManager Instance;

    private List<ErrorMessage> listErrors;


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        Instance = this;
    }

    public void PushError(string _error)
    {

        listErrors.Add(new ErrorMessage(_error, DateTime.Now.ToString("ddd MMM %d, yyyy hh:mm:ss tt")));

        SaveErrorToPlayfab();
    }

    public void SaveErrorToPlayfab()
    {

        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { "Errors", JsonConvert.SerializeObject(listErrors) }
            },

            Permission = UserDataPermission.Public

        };
        PlayFabClientAPI.UpdateUserData(request, result =>
        {

            Debug.Log("SEND MESSAGE SUCCESS");
        },
        error =>
        {
            Debug.Log(error.GenerateErrorReport());
        });
    }

    public void LoadListErrorFromPlayfab()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
        {
            if (result.Data != null && result.Data.ContainsKey("Errors"))
            {
                listErrors = JsonConvert.DeserializeObject<List<ErrorMessage>>(result.Data["Errors"].Value);
                Debug.Log("LOAD SUCCESS");

            }
            else
            {
                listErrors = new List<ErrorMessage>();
                Debug.Log("NO ERROR WAS FOUNDED ! SEND A NEW ERROR");
            }
        },
        error =>
        {
            Debug.Log(error.GenerateErrorReport());
        });
    }

}