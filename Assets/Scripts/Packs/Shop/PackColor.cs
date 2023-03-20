using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Pack;

public class PackColor
{
    #region PACK TYPE COLOR STRING
    private string _IronHex = "DBDBDB";
    private string _BronzeHex = "CF9F7C";
    private string _GoldHex = "C1BB4C";
    private string _PlatiumHex = "6CCBE7";
    private string _DiamonHex = "BE84E3";

    public string IronHex { get => _IronHex; }
    public string BronzeHex { get => _BronzeHex; }
    public string GoldHex { get => _GoldHex; }
    public string PlatiumHex { get => _PlatiumHex;}
    public string DiamonHex { get => _DiamonHex; }

    #endregion

    public Color ReturnPackColor(string packType)
    {
        PackType type = Enum.Parse<PackType>(packType);

        PackColor packColor = new PackColor();

        switch (type)
        {
            case PackType.IronPack:
                return getColorByHex(packColor.IronHex);
            case PackType.BronzePack:
                return getColorByHex(packColor.BronzeHex);
            case PackType.GoldPack:
                return getColorByHex(packColor.GoldHex);
            case PackType.PlatiumPack:
                return getColorByHex(packColor.PlatiumHex);
            case PackType.DiamondPack:
                return getColorByHex(packColor.DiamonHex);

        }

        return Color.white;
    }

    //Lấy màu thông qua mã
    public Color getColorByHex(string hex)
    {

        Color newColor;
        ColorUtility.TryParseHtmlString("#" + hex, out newColor);

        return newColor;
    }
}
