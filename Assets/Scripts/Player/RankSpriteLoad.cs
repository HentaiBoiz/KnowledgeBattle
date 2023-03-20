using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankSpriteLoad
{
    // Sprite RankImage;
    string Freshman1 = "RankIm/FM1_1", Freshman2 = "RankIm/FM2", Freshman3 = "RankIm/FM3", 
        Sopho1 = "RankIm/SP1", Sopho2 = "RankIm/SP2", Sopho3 = "RankIm/SP13", 
        Junior1 = "RankIm/JU1_1", Junior2 = "RankIm/JU1_2", Junior3 = "RankIm/JU1_3", 
        Senior1 = "RankIm/SE1", Senior2 = "RankIm/SE2", Senior3 = "RankIm/SE3",
        Master1 = "RankIm/MA1_1", Master2 = "RankIm/MA1_2", Master3 = "RankIm/MA1_3", 
        Doctor1 = "RankIm/DO1", Doctor2 = "RankIm/DO2", Doctor3 = "RankIm/DO3", 
        PD1 = "RankIm/PD1_1", PD2 = "RankIm/PD1_2", PD3 = "RankIm/PD1_3";

    public RankSpriteLoad()
    {

    }

    public void SetRank(int rscore, TMP_Text tmpText, Image rankImg)
    {
        //Get Rank
        if (rscore >= 0 && rscore < 100)
        {
            tmpText.text = "Freshman I";
            rankImg.sprite = Resources.Load<Sprite>(Freshman1);
        }
        else if (rscore >= 100 && rscore < 200)
        {
            tmpText.text = "Freshman II";
            rankImg.sprite = Resources.Load<Sprite>(Freshman2);

        }
        else if (rscore >= 200 && rscore < 300)
        {
            tmpText.text = "Freshman III";
            rankImg.sprite = Resources.Load<Sprite>(Freshman3);

        }
        else if (rscore >= 300 && rscore < 400)
        {
            tmpText.text = "Sophomore I";
            rankImg.sprite = Resources.Load<Sprite>(Sopho1);

        }
        else if (rscore >= 400 && rscore < 500)
        {
            tmpText.text = "Sophomore II";
            rankImg.sprite = Resources.Load<Sprite>(Sopho2);

        }
        else if (rscore >= 500 && rscore < 600)
        {
            tmpText.text = "Sophomore III";
            rankImg.sprite = Resources.Load<Sprite>(Sopho3);

        }
        else if (rscore >= 600 && rscore < 700)
        {
            tmpText.text = "Junior I";
            rankImg.sprite = Resources.Load<Sprite>(Junior1);

        }
        else if (rscore >= 700 && rscore < 800)
        {
            tmpText.text = "Junior II";
            rankImg.sprite = Resources.Load<Sprite>(Junior2);


        }
        else if (rscore >= 800 && rscore < 900)
        {
            tmpText.text = "Junior III";
            rankImg.sprite = Resources.Load<Sprite>(Junior3);


        }
        else if (rscore >= 1000 && rscore < 1100)
        {
            tmpText.text = "Senior I";
            rankImg.sprite = Resources.Load<Sprite>(Senior1);


        }
        else if (rscore >= 1100 && rscore < 1200)
        {
            tmpText.text = "Senior II";
            rankImg.sprite = Resources.Load<Sprite>(Senior2);

        }
        else if (rscore >= 1200 && rscore < 1300)
        {
            tmpText.text = "Senior III";
            rankImg.sprite = Resources.Load<Sprite>(Senior3);

        }
        else if (rscore >= 1300 && rscore < 1400)
        {
            tmpText.text = "Master I";
            rankImg.sprite = Resources.Load<Sprite>(Master1);

        }
        else if (rscore >= 1400 && rscore < 1500)
        {
            tmpText.text = "Master II";
            rankImg.sprite = Resources.Load<Sprite>(Master2);

        }
        else if (rscore >= 1500 && rscore < 1600)
        {
            tmpText.text = "Master III";
            rankImg.sprite = Resources.Load<Sprite>(Master3);

        }
        else if (rscore >= 1600 && rscore < 1700)
        {
            tmpText.text = "Doctor I";
            rankImg.sprite = Resources.Load<Sprite>(Doctor1);

        }
        else if (rscore >= 1700 && rscore < 1800)
        {
            tmpText.text = "Doctor II";
            rankImg.sprite = Resources.Load<Sprite>(Doctor2);

        }
        else if (rscore >= 1800 && rscore < 1900)
        {
            tmpText.text = "Doctor III";
            rankImg.sprite = Resources.Load<Sprite>(Doctor3);

        }
        else if (rscore >= 1900 && rscore < 2000)
        {
            tmpText.text = "Post-doctoral I";
            rankImg.sprite = Resources.Load<Sprite>(PD1);

        }
        else if (rscore >= 2000 && rscore < 2100)
        {
            tmpText.text = "Post-doctoral II";
            rankImg.sprite = Resources.Load<Sprite>(PD2);

        }
        else if (rscore >= 2100 && rscore < 2200)
        {
            tmpText.text = "Post-doctoral III";
            rankImg.sprite = Resources.Load<Sprite>(PD3);

        }

    }
}
