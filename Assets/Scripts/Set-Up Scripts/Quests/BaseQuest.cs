using UnityEngine;

[CreateAssetMenu(menuName = "TextAdventure/Quest")]
public class BaseQuest : ScriptableObject {
    public string questName;
    public Rewards rewards;
    [TextArea]
    public string questDescription;
    public bool completed;
    public BaseQuest nextQuest;
    public QuestTypes type;
    public BaseCharacter fetchgoal;
    public BaseObject fetchTarget;
}

[System.Serializable]
public class Rewards {
    public int gold;
    public BaseObject[] loot;
}

[System.Serializable]
public enum QuestTypes {
    fetch,
    kill,
    explore,
    gather,
    defend
}
