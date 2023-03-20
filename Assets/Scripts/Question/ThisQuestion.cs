using static Card;
using UnityEngine;


public class ThisQuestion : MonoBehaviour
{
    [Header("Question Info")]
    public int thisSide;
    public int thisIndex;

    [Header("Question UI")]
    #region QUESTION UI
    SpriteRenderer spriteRenderer;

    #endregion

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {


        //Ko có Question nào thì ko cho click
        if (Question_Manager_Id.Instance.questionsArrays[thisSide].fieldQuestion[thisIndex].questionId == "")
            return;


        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100f))
        {

            if (hit.transform.gameObject == this.gameObject)
            {
                spriteRenderer.sprite = Question_Manager_Id.Instance.onMouseOverSprite;

                if (Input.GetMouseButtonDown(0))
                {
                    SelectionManager.Instance.OpenQuestionPanelUI(thisSide, thisIndex);
                }
            }
            else
            {
                spriteRenderer.sprite = Question_Manager_Id.Instance.onMouseOutSprite;
            }
        }

    }

    public void UpdateUIQuestion()
    {
        Question question = Question_Manager_Id.Instance.questionsArrays[thisSide].fieldQuestion[thisIndex];

        if (question.questionId != "")
        {
            if (question.realAnswers.Length == question.currentCardAttach.Count)
                spriteRenderer.color = Question_Manager_Id.Instance.solvedQuestionColor;
            else
                spriteRenderer.color = Question_Manager_Id.Instance.haveQuestionColor;
        }
        else
        {
            spriteRenderer.color = Question_Manager_Id.Instance.emptyQuestionColor;
        }

    }


}
