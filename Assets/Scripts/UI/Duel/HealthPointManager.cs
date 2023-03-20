using UnityEngine;
using TMPro;
using Photon.Pun;

public class HealthPointManager : MonoBehaviour
{
    public int localSide; //0: Là Player, 1: Là Opponent
    const float ChangeHpSpeed = 1f;

    private TextMeshProUGUI hpTxt;

    private bool isSetup = false;
    private bool isCalculateDamage = false;
    private bool isRecover; //True: Nếu là Recover, False: Nếu là Taking

    //Taking Damage Animation
    private float currentHp = -1;
    private float targetHp = -1; //Đây là lượng HP hướng tới sau khi đã nhận Damage

    private void Awake()
    {
        hpTxt = GetComponent<TextMeshProUGUI>();
    }

    public void Update()
    {
        if (!Field_Manager_Id.Instance.isDuelStart)
            return;

        //Kiểm tra HP có thay đổi không
        CheckHpIfChange();

        if (isCalculateDamage == false)
            return;

        CalculateDamageAnimation();
    }

    public void SetupHP()
    {
        if (isSetup)
            return;

        //Nếu vẫn còn Player chưa được SetHP để bắt đầu đấu thì chưa cập nhật
        foreach (var item in Field_Manager_Id.Instance.zoneId)
        {
            if (item.healthPoint <= 0)
                return;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            if(localSide == 0)
            {
                SetHpText(Field_Manager_Id.Instance.zoneId[0].healthPoint);
            }
            else
            {
                SetHpText(Field_Manager_Id.Instance.zoneId[1].healthPoint);
            }
        }
        else
        {
            if (localSide == 0)
            {
                SetHpText(Field_Manager_Id.Instance.zoneId[1].healthPoint);
            }
            else
            {
                SetHpText(Field_Manager_Id.Instance.zoneId[0].healthPoint);
            }
        }

        isSetup = true;
    }

    public void SetHpText(int hp)
    {
        hpTxt.text = hp.ToString();
    }

    public void StartCalculateDamage(float targetHp)
    {

        currentHp = float.Parse(hpTxt.text);
        this.targetHp = targetHp;

        //Xem thử là trừ hay cộng
        if(currentHp <= targetHp)
        {
            isRecover = true;
        }
        else
        {
            isRecover = false;
        }

        isCalculateDamage = true;
    }

    public void CalculateDamageAnimation()
    {
        if (isRecover)
        {
            if (currentHp < targetHp)
            {
                currentHp += ChangeHpSpeed;
                SetHpText((int)currentHp);
            }
            else
            {
                isCalculateDamage = false;
                SetHpText((int)targetHp);
            }
        }
        else
        {
            if (currentHp > targetHp)
            {
                currentHp -= ChangeHpSpeed;
                SetHpText((int)currentHp);
            }
            else
            {
                isCalculateDamage = false;
                SetHpText((int)targetHp);
            }
        }
    }

    //Kiểm tra HP có thay đổi không
    public void CheckHpIfChange()
    {
        if (isCalculateDamage == false)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (localSide == 0)
                {
                    if (int.Parse(hpTxt.text) != Field_Manager_Id.Instance.zoneId[0].healthPoint)
                        StartCalculateDamage(Field_Manager_Id.Instance.zoneId[0].healthPoint);
                }
                else
                {
                    if (int.Parse(hpTxt.text) != Field_Manager_Id.Instance.zoneId[1].healthPoint)
                        StartCalculateDamage(Field_Manager_Id.Instance.zoneId[1].healthPoint);
                }
            }
            else
            {
                if (localSide == 0)
                {
                    if (int.Parse(hpTxt.text) != Field_Manager_Id.Instance.zoneId[1].healthPoint)
                        StartCalculateDamage(Field_Manager_Id.Instance.zoneId[1].healthPoint);
                }
                else
                {
                    if (int.Parse(hpTxt.text) != Field_Manager_Id.Instance.zoneId[0].healthPoint)
                        StartCalculateDamage(Field_Manager_Id.Instance.zoneId[0].healthPoint);
                }
            }
        }

    }
}
