using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailsView_Endgame : View
{
    [SerializeField]
    public TMP_Text QuestionSolved;
    [SerializeField]
    public TMP_Text TotalDamage;
    [SerializeField]
    public TMP_Text Reward;
    [SerializeField]
    public Button OverviewBtn;

    public override void Initialize()
    {
        OverviewBtn.onClick.AddListener(() =>
        {
            ViewManager.Instance.Show<OverView_Endgame>();

        });


        base.Initialize();
    }


}
