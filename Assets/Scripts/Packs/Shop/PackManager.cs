using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Pack;

public class PackManager : MonoBehaviour
{

    [Serializable]
    public struct PackObjectList
    {
        public PackType packType;
        public List<Transform> listPack;

    }

    public static PackManager Instance;

    public List<PackObjectList> packObjectLists = new List<PackObjectList>();


    private void Awake()
    {
        Instance = this;
    }

    public void AddToObjectList(PackType packType, Transform transform)
    {
        foreach (var item in packObjectLists)
        {
            if(item.packType == packType)
            {
                item.listPack.Add(transform);
                return;
            }
        }

        //Chưa tìm thấy thì tạo mới list
        PackObjectList temp = new PackObjectList() { packType = packType, listPack = new List<Transform>() };

        temp.listPack.Add(transform);

        packObjectLists.Add(temp) ;


    }

    public void DisableAllPack()
    {
        foreach (var type in packObjectLists)
        {
            foreach (var item in type.listPack)
            {
                item.gameObject.SetActive(false);
            }
        }
    }

    public void ShowPack(PackType packType)
    {
        DisableAllPack();

        foreach (var type in packObjectLists)
        {
            if(type.packType == packType)
            {
                foreach (var item in type.listPack)
                {
                    item.gameObject.SetActive(true);
                }

                return;
            }
        }
    }

    #region BUTTON METHOD
    public void ShowIronPack()
    {
        ShowPack(PackType.IronPack);
    }
    public void ShowBronzePack()
    {
        ShowPack(PackType.BronzePack);
    }
    public void ShowGoldPack()
    {
        ShowPack(PackType.GoldPack);
    }
    public void ShowPlatinumPack()
    {
        ShowPack(PackType.PlatiumPack);
    }
    public void ShowDiamondPack()
    {
        ShowPack(PackType.DiamondPack);
    }

    public void ExitVendingMachine()
    {
        PhotonNetwork.LoadLevel("1_OfflineScene");
    }
    #endregion
}
