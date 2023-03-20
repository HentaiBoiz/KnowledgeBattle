using static Card;
using UnityEngine;

[CreateAssetMenu(fileName = "Question", menuName = "Scriptale Object/Question")]
public class SOQuestion : ScriptableObject
{
    public string questionId;
    public CardAttribute questionAttribute;
    public int questionLevel; //Cấp độ của câu hỏi
    public Sprite mainQuestion; //Câu hỏi chính
    public string[] realAnswers; //Câu trả lời đúng

}
