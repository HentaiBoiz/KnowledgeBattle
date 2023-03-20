using UnityEngine;
using Photon.Pun;

public abstract class View : MonoBehaviourPunCallbacks
{
    public bool IsInitialized { get; private set; }

    public virtual void Initialize()
    {
        IsInitialized = true;
    }

    public virtual void Show(object args = null)
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}
