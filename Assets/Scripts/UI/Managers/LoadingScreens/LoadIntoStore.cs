using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadIntoStore : LoadingScreen
{
    private List<string> loadingPackId = new List<string>(); //Chứa mảng pack tạm để kiểm tra việc load ảnh Pack

    private void Start()
    {
        isLoadingDone = false;
        progress = -1;
    }

    private void Update()
    {
        if (isLoadingDone)
        {
            OnLoadingDone();
            return;
        }


        if (progress < 0)
            return;

        if (loadingPackId.Count <= 0)
        {
            isLoadingDone = true;
            return;
        }


        foreach (var id in loadingPackId)
        {
            if (PackDatabase.Instance.FindPackWithIdOnly(id).image != null)
            {
                loadingPackId.Remove(id);
                UpdateProgress();
                return;
            }
        }

    }

    //Load Img của các Pack
    public void StartLoading_PackImg()
    {

        SetUpLoadingInfo(loadingPackId.Count - 1);

    }

    protected override void OnLoadingDone()
    {
        PackVendingMachine.Instance.SetupPackPlayfab();

        base.OnLoadingDone();

    }

    //Add Id thêm vào Temp Pack Id
    public void AddToTempIdPack(string id)
    {
        loadingPackId.Add(id);
    }

}
