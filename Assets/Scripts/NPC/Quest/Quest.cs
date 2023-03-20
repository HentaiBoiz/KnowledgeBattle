using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest 
{
    public enum QuestProgress
    {
        NOT_AVAILABLE,
        AVAILABLE,
        ACCEPTED,
        COMPLETE,
        DONE
    }

    public string title, des, hint, congratulation, sumary, questObjective;
    public int id, kcReward, nextQuest, questObjectiveCount, questObjectiveRequirement;

    public QuestProgress progress;
    public Quest() { }

    public Quest(string title, string des, string hint, string congratulation, string sumary, string questObjective, int questObjectiveCount, int questObjectiveRequirement, int id, int kcReward, int nextQuest)
    {
        this.Title = title;
        this.Des = des;
        this.Hint = hint;
        this.Congratulation = congratulation;
        this.Sumary = sumary;
        this.QuestObjective = questObjective;
        this.QuestObjectiveCount = questObjectiveCount;
        this.QuestObjectiveRequirement = questObjectiveRequirement;
        this.Id = id;
        this.KcReward = kcReward;
        this.NextQuest = nextQuest;
    }
    public string Title { get => title; set => title = value; }
    public string Des { get => des; set => des = value; }
    public string Hint { get => hint; set => hint = value; }
    public string Congratulation { get => congratulation; set => congratulation = value; }
    public string Sumary { get => sumary; set => sumary = value; }
    public string QuestObjective { get => questObjective; set => questObjective = value; }
    public int QuestObjectiveCount { get => questObjectiveCount; set => questObjectiveCount = value; }
    public int QuestObjectiveRequirement { get => questObjectiveRequirement; set => questObjectiveRequirement = value; }
    public int Id { get => id; set => id = value; }
    public int KcReward { get => kcReward; set => kcReward = value; }
    public int NextQuest { get => nextQuest; set => nextQuest = value; }
}
