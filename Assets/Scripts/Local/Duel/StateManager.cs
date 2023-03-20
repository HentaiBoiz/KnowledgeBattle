using UnityEngine;

public class StateManager : MonoBehaviour
{
    public static StateManager Instance;

    public enum ActionState
    {
        normalState, //Không có gì xảy ra
        attachState, //Attach 1 lá bài vào Queue để giải Question
        fightingState,
        counterState, //Đây là State dành cho đối thủ kích hoạt Cheat Card
        confirmCounterState, //Đây là State dành cho đối thủ confirm có kích hoạt Cheat Card hay không
    }

    public ActionState DuelState;

    private void Awake()
    {
        Instance = this;
    }


}
