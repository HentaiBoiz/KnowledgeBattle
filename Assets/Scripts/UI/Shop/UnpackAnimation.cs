using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnpackAnimation : MonoBehaviour
{
    //Unpack Card
    public Transform cardHolder;
    public GameObject unpackCard; //Prefab

    //Pack
    public Image packImg;
    public Image packTemplate;

    private void OnEnable()
    {
        cardHolder.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        HideUnpackCard();
    }

    public void SetupPackToShow(Sprite packSprite, Color packColor)
    {
        packImg.sprite = packSprite;
        packTemplate.color = packColor;

    }

    public void ShowUnpackCard()
    {
        StartCoroutine(CreateUnpackCard());
    }

    IEnumerator CreateUnpackCard()
    {
        foreach (var item in PackDetail.Instance.lastCardsUnpack)
        {
            Sprite temp = item.cardSprite;
            yield return temp;

            GameObject g = Instantiate(unpackCard, cardHolder);
            g.transform.SetParent(cardHolder);

            g.transform.localPosition = Vector3.one;
            g.transform.localScale = Vector3.one;
            g.transform.localRotation = Quaternion.identity;

            g.GetComponent<UnpackCard>().SetupCardUI(item);
        }

        cardHolder.gameObject.SetActive(true);
    }

    public void HideUnpackCard()
    {
        foreach(Transform child in cardHolder)
        {
            Destroy(child.gameObject);
        }
    }

}
