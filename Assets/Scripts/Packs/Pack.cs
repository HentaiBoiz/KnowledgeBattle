using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Pack
{
    public enum PackType
    {
        IronPack,
        BronzePack,
        GoldPack,
        PlatiumPack,
        DiamondPack
    }

    public string id;
    public string packName;
    public Sprite image = null;
    public string description;
    public PackType packType;
    public int price;

    public Pack() { }

    public Pack(string id, string name, Sprite image, string description, PackType packType, int price)
    {
        this.id = id;
        this.packName = name;
        this.image = image;
        this.description = description;
        this.packType = packType;
        this.price = price;
    }

    public Pack(Pack pack)
    {
        this.id = pack.id;
        this.packName = pack.packName;
        this.image = pack.image;
        this.description = pack.description;
        this.packType = pack.packType;
        this.price = pack.price;
    }

    public Pack(SOPack pack)
    {
        this.id = pack.id;
        this.packName = pack.packName;
        this.image = pack.image;
        this.description = pack.description;
        this.packType = pack.packType;
        this.price = pack.price;
    }
}
