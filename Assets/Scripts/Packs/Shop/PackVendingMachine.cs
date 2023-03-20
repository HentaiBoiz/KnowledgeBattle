using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackVendingMachine : MonoBehaviour
{
    public static PackVendingMachine Instance;

    public GameObject packPrefab;

    private void Awake()
    {
        Instance = this;
    }


    //public void SetupPackLocal()
    //{
    //    foreach (var item in PackDatabase.Instance.soPacksDatabase)
    //    {
    //        GameObject g = Instantiate(packPrefab, this.transform);
    //        g.transform.SetParent(this.transform);
    //        g.GetComponent<ThisPack>().SetupPack(item.id);

    //        PackManager.Instance.AddToObjectList(g.GetComponent<ThisPack>().packMono.packType, g.transform);
    //    }
    //}

    public void SetupPackPlayfab()
    {
        foreach (var typeList in PackDatabase.Instance.packTypeLists)
        {
            foreach (var item in typeList.listPack)
            {
                GameObject g = Instantiate(packPrefab, this.transform);
                g.transform.SetParent(this.transform);
                g.GetComponent<ThisPack>().SetupPack(item.id);
                PackManager.Instance.AddToObjectList(g.GetComponent<ThisPack>().packMono.packType, g.transform);
            }
            
        }

        PackManager.Instance.ShowIronPack();
    }
}
