
using UnityEngine;
using static Card;

public class CardIconSet 
{
    #region PATH
    string iconChemistry = "CardIcon/IconChemistry";
    string iconAlgebra = "CardIcon/IconAlgebra";
    string iconBiology = "CardIcon/IconBiology";
    string iconGeometry = "CardIcon/IconGeometry";
    string iconEnglish = "CardIcon/IconEnglish";
    string iconHistory = "CardIcon/IconHistory";
    #endregion

    public Sprite ReturnCardIcon(CardAttribute cardAttribute)
    {
        Sprite temp = null;

        switch (cardAttribute)
        {
            case CardAttribute.Chemistry:
                temp = Resources.Load<Sprite>(iconChemistry);
                break;
            case CardAttribute.Algebra:
                temp = Resources.Load<Sprite>(iconAlgebra);
                break;
            case CardAttribute.Biology:
                temp = Resources.Load<Sprite>(iconBiology);
                break;
            case CardAttribute.Geometry:
                temp = Resources.Load<Sprite>(iconGeometry);
                break;
            case CardAttribute.English:
                temp = Resources.Load<Sprite>(iconEnglish);
                break;

        }

        return temp;
    }
}
