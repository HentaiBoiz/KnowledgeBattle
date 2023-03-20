using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadIntoDuel : LoadingScreen
{
    [SerializeField]
    private List<string> loadingCardId = new List<string>(); //Chứ mảng card tạm để kiểm tra việc load ảnh Card

    private void Start()
    {
        isLoadingDone = false;
        progress = -1;

        PhotonDuelistStats.Instance.LoadRoomDuelists(); //Load Stats của Player
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

        if (PhotonDuelistStats.Instance.AllIsReady())
        {
            isLoadingDone = true;
            return;
        }

        //if (loadingCardId.Count <= 0)
        //{
        //    PhotonDuelistStats.Instance.SetPlayerReady(TurnManager.Instance.localSide);
        //    return;
        //}


        //foreach (var id in loadingCardId)
        //{
        //    if (PlayfabCardDB.Instance.playfabCardDB[id].cardSprite != null)
        //    {
        //        //AssignSprite(id);
        //        loadingCardId.Remove(id);
        //        UpdateProgress();
        //        return;
        //    }
        //}


    }

    //Load Img của những lá bài
    public void StartLoading_CardImg(List<string> tempInventoryCardId)
    {
        loadingCardId = new List<string>(tempInventoryCardId.ToArray());

        SetUpLoadingInfo(tempInventoryCardId.Count - 1);

    }

    protected override void OnLoadingDone()
    {
        Field_Manager_Id.Instance.SetDeckReady();

        Field_Manager_Id.Instance.StartDuel();

        base.OnLoadingDone();

    }

    ////Double Check cho việc load ảnh
    //private void AssignSprite(string id)
    //{
    //    foreach (var card in CardDatabase.Instance.cardsDatabase)
    //    {
    //        if(card.id == id)
    //            card.cardSprite = PlayfabCardDB.Instance.playfabCardDB[id].cardSprite;
    //    }
    //}
}
