using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NormalBattleView : View
{
    public Button RankedModeBtn;
    public Button CustomModeBtn;

    public override void Initialize()
    {
        RankedModeBtn.onClick.AddListener(() =>
        {
            ViewManager.Instance.Show<RankBattleView>();
        });

        CustomModeBtn.onClick.AddListener(() =>
        {
            ViewManager.Instance.Show<CustomBattleView>();
        });

        base.Initialize();
    }

}
