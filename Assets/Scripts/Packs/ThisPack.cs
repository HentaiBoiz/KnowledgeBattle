using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Pack;
using UnityEngine.EventSystems;

public class ThisPack : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    

    public string packId;
    public Pack packMono;

    [Header("Pack UI")]
    #region CARD UI
    public Image packImage;
    public Image packLayout; //Đổi color theo loại Pack
    public Transform packBorder;
    #endregion

    public void OnPointerEnter(PointerEventData eventData)
    {
        packBorder.gameObject.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PackDetail.Instance.SetupPackDetail(packMono);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        packBorder.gameObject.SetActive(false);
    }


    public void SetupPack(string packId)
    {
        packMono = PackDatabase.Instance.FindPackWithIdOnly(packId);

        if (packMono == null)
        {
            Debug.LogError("Can't find Pack with Id: " + packId);
            return;
        }


        //Pack Type
        packImage.sprite = packMono.image;

        PackColor packColor = new PackColor();
        packLayout.color = packColor.ReturnPackColor(packMono.packType.ToString());

    }

   

   
}