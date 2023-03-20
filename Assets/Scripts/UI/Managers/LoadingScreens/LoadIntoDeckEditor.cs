using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadIntoDeckEditor : LoadingScreen
{
    private List<string> loadingCardId = new List<string>(); //Chứ mảng card tạm để kiểm tra việc load ảnh Card
    

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
            

        //if (progress < 0)
        //    return;

        //if (loadingCardId.Count <= 0)
        //{
        //    isLoadingDone = true;
        //    return;
        //}
            

        //foreach (var id in loadingCardId)
        //{
        //    if (PlayfabCardDB.Instance.playfabCardDB[id].cardSprite != null)
        //    {
        //        loadingCardId.Remove(id);
        //        UpdateProgress();
        //        return;
        //    }
        //}
        
  
    }

    ////Load Img của những lá bài
    //public void StartLoading_CardImg(List<string> tempInventoryCardId)
    //{   
    //    loadingCardId = new List<string>(tempInventoryCardId.ToArray());

    //    SetUpLoadingInfo(tempInventoryCardId.Count - 1);

    //}


}
