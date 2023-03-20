using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldQuestionManager : MonoBehaviour
{
    public List<ThisQuestion> thisQuestions;

    private void Update()
    {
        if (Question_Manager_Id.Instance.isUpdateQuestion == true)
        {
            foreach (var thisQuestion in thisQuestions)
            {
                thisQuestion.UpdateUIQuestion();
            }

            Question_Manager_Id.Instance.isUpdateQuestion = false;
        }
    }
}
