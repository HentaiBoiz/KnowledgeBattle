using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField]
    protected Slider slider;
    [SerializeField]
    protected int progress = -1;
    [SerializeField]
    protected bool isLoadingDone = false;


    public void SetUpLoadingInfo(int max)
    {
        progress = 0;
        slider.maxValue = max;
    }

    public void UpdateProgress()
    {
        progress++;
        slider.value = progress;
    }

    //====================================

    protected virtual void OnLoadingDone()
    {
        this.gameObject.SetActive(false);
    }

}
