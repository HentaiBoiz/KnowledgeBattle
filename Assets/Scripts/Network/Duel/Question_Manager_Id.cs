using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Question_Manager_Id : MonoBehaviour
{
    public static Question_Manager_Id Instance;

    public QuestionsArray[] questionsArrays = new QuestionsArray[2];

    [Header("UI")]
    #region UI
    public Color haveQuestionColor;
    public Color emptyQuestionColor;
    public Sprite onMouseOutSprite;
    public Sprite onMouseOverSprite;
    public Color solvedQuestionColor;
    public Sprite qLoading;
    #endregion

    PhotonView _photonView;

    #region UPDATE TRIGGER FOR LOCAL
    float updateTime = 15f; //Sau 15s thì Update 1 lần
    float countTime = 0f;
    [HideInInspector]
    public bool isUpdateQuestion = false;
    #endregion

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        countTime -= Time.deltaTime;
        if (countTime > 0f)
            return;

        countTime = updateTime;
        OnUpdateQuestion();
    }

    #region CHECKING METHOD
    public bool isAnswerSlotFull(int side)
    {
        for (int i = 0; i < 3; i++)
        {
            if (isCanAttachMore(side, i))
                return false;
        }

        return true;
    }
    //Kiểm tra thử Question của index đó là full slot attach resources hay chưa
    public bool isCanAttachMore(int side, int questionIndex)
    {
        if (questionsArrays[side].fieldQuestion[questionIndex].questionId != "")
            if (questionsArrays[side].fieldQuestion[questionIndex].currentCardAttach.Count < questionsArrays[side].fieldQuestion[questionIndex].realAnswers.Length)
            {
                return true;
            }


        return false;
    }

    public bool isAnswerRight(Question question)
    {
        for (int i = 0; i < question.realAnswers.Length; i++)
        {
            if (question.realAnswers[i] != question.currentAnswer[i])
                return false;
        }

        return true;
    }
    #endregion

    #region RPC METHOD
    //Set Question
    public void SetQuestion(int side, int zoneIndex, int handIndex)
    {
        Card cardMono = CardDatabase.Instance.FindCardWithId(Field_Manager_Id.Instance.zoneId[side].handZone[handIndex]); //Tìm card đó trong db

        if(cardMono == null)
        {
            Debug.LogError("Cannot find card");
            return;
        }

        //Tìm câu hỏi Random
        Question question = QuestionDatabase.Instance.RandomQuestionWithConditions(cardMono.level, cardMono.attribute.ToString());


        Debug.Log("Id: " + question.questionId);

        _photonView.RPC("RPC_SetQuestion", RpcTarget.All, side, zoneIndex, question.questionId, question.questionAttribute.ToString());
        
    }
    [PunRPC]
    public void RPC_SetQuestion(int side, int zoneIndex, string questionId, string attribute)
    {
        questionsArrays[side].fieldQuestion[zoneIndex] = QuestionDatabase.Instance.FindQuestionWithId(questionId, attribute);

        StartCoroutine(GetQuestionImgFromUrl(QuestionDatabase.Instance.questionCatalogItemsDB[questionId].ItemImageUrl, questionsArrays[side].fieldQuestion[zoneIndex]));

        Invoke("OnUpdateQuestion", 0.2f);
    }
    //Load Question Img
    IEnumerator GetQuestionImgFromUrl(string url, Question question)
    {
        question.mainQuestion = qLoading;

        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
            ErrorsManager.Instance.PushError(request.error);
        }
        else
        {
            Debug.Log("Load Question Img Succes");

            Texture2D cardTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;

            Sprite sprite = Sprite.Create(cardTexture, new Rect(0, 0, cardTexture.width, cardTexture.height), Vector2.zero);
            question.mainQuestion = sprite;

        }

    }

    //Solve Question
    public void SolveQuestion(int side, int qIndex)
    {
        Question question = questionsArrays[side].fieldQuestion[qIndex];

        //Kiểm tra xem Answer này đúng hay sai ?
        if (isAnswerRight(question))
        {
            DuelRuleManager.Instance.wasPush = true; //Đánh dấu là đã Push 1 Battle Card trong lượt này rồi
            _photonView.RPC("RPC_SolveQuestion", RpcTarget.All, side, qIndex);
            _photonView.RPC("DiscardAllAttachCard", RpcTarget.All, side, qIndex);
            
            
        }
        else
        {
            _photonView.RPC("RPC_CancelQuestion", RpcTarget.All, side, qIndex);
            _photonView.RPC("DiscardAllAttachCard", RpcTarget.All, side, qIndex);

        }

    }
    [PunRPC]
    public void RPC_SolveQuestion(int side, int qIndex)
    {
        int battleZoneId = Field_Manager_Id.Instance.CheckBattleZoneEmpty(side);
        
        Field_Manager_Id.Instance.zoneId[side].battleZone[battleZoneId] = Field_Manager_Id.Instance.zoneId[side].queueZone[qIndex].ToString();

        //Push VFX
        Duel_VFX_Manager.Instance.BattleCardAppearVFX(Field_Manager_Id.Instance.zoneId[side].battleZone[battleZoneId]);

        //Cheat Event
        CheatEventManager.Instance.SetLastPushCard(side, battleZoneId);

        //DiscardAllAttachCard(side, qIndex);

        Field_Manager_Id.Instance.zoneId[side].queueZone[qIndex] = "";
        questionsArrays[side].fieldQuestion[qIndex].questionId = "";

        //Chuyển State thành Counter State để chờ đối thủ kích hoạt Cheat
        CheatEventManager.Instance.StartCounterConfirmState(side);


        //Update UI
        Field_Manager_Id.Instance.Invoke("OnUpdateQueue", 0.2f);
        Field_Manager_Id.Instance.Invoke("OnUpdateBattle", 0.2f);
        Invoke("OnUpdateQuestion", 0.2f);
    }
    [PunRPC]
    public void RPC_CancelQuestion(int side, int qIndex)
    {
        Field_Manager_Id.Instance.zoneId[side].handZone.Add(Field_Manager_Id.Instance.zoneId[side].queueZone[qIndex].ToString());

        Field_Manager_Id.Instance.zoneId[side].queueZone[qIndex] = "";
        questionsArrays[side].fieldQuestion[qIndex].questionId = "";

        //Update UI
        Field_Manager_Id.Instance.Invoke("OnUpdateQueue", 0.2f);
        Field_Manager_Id.Instance.Invoke("OnUpdateBattle", 0.2f);
        Invoke("OnUpdateQuestion", 0.2f);
    }
    [PunRPC]//Bỏ hết bài đã dùng để attach xuống mộ
    public void DiscardAllAttachCard(int side, int qIndex)
    {
        for (int i = 0; i < questionsArrays[side].fieldQuestion[qIndex].currentCardAttach.Count; i++)
        {
            Field_Manager_Id.Instance.AddCardToDropZone(side, questionsArrays[side].fieldQuestion[qIndex].currentCardAttach[i].id);
            questionsArrays[side].fieldQuestion[qIndex].currentCardAttach.RemoveAt(i);
        }

    }

    //Attach Card
    public void AttachACard(int side, int questionIndex, int handIndex, string hint)
    {
        _photonView.RPC("RPC_AttachACard", RpcTarget.All, side, questionIndex, handIndex, hint);
    }
    [PunRPC]
    public void RPC_AttachACard(int side, int questionIndex, int handIndex, string hint)
    {
        try
        {
            int answerIndex = questionsArrays[side].fieldQuestion[questionIndex].currentCardAttach.Count;

            //Lấy lá bài trên tay để Attach
            Card temp = CardDatabase.Instance.FindCardWithId(Field_Manager_Id.Instance.zoneId[side].handZone[handIndex]);

            questionsArrays[side].fieldQuestion[questionIndex].currentAnswer[answerIndex] = hint;

            questionsArrays[side].fieldQuestion[questionIndex].currentCardAttach.Add(temp);

            Field_Manager_Id.Instance.zoneId[side].handZone.RemoveAt(handIndex);

            SelectionManager.Instance.HideResourcesSelection();

            //Update
            Invoke("OnUpdateQuestion", 0.2f);
            Field_Manager_Id.Instance.Invoke("OnUpdateHand", 0.2f);
        }
        catch (System.Exception e)
        {
            ErrorsManager.Instance.PushError(e.Message);
        }
        
    }


    #endregion

    #region RAISED EVENT METHOD

    public void OnUpdateQuestion()
    {
        object[] datas = new object[] { }; //Đóng gói

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };

        PhotonNetwork.RaiseEvent(
            ((byte)EventCode.onUpdateQuestion),
            datas,
            raiseEventOptions,
            SendOptions.SendReliable);

    }
    #endregion

   
}
