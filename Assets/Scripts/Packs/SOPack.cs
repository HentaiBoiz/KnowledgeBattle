using UnityEngine;
using UnityEngine.UI;
using static Pack;

[CreateAssetMenu(fileName = "Pack", menuName = "Scriptale Object/Pack")]
public class SOPack : ScriptableObject
{
    public string id;
    public string packName;
    public Sprite image;
    [TextArea(0, 3)]
    public string description;
    public PackType packType;
    public int price;
}
