using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu (menuName = "TextAdventure/Base Object")]
public class BaseObject : ScriptableObject {

	public string noun = "Type In Lowercase";
    public int value;
    //public ObjectStats statModifiers;
    public bool equippable;
    [HideInInspector] public bool equipped;
    public int damageBonus;
    public string roomLocation;
    [TextArea]
	public string examineDescription;
    public SpecialInput[] specialInput;
    public UseFunction useFunction;
    
}
/*
[System.Serializable]
public class ObjectStats {
    public bool equippable;
    public int damageBonus;
    [HideInInspector] public bool equipped;
    public BoostedStats[] boostedStats;
    public ReducedStats[] reducedStats;
}

[System.Serializable]
public class BoostedStats {
    public ModStats boostedStat;
    [Range(-3, 3)] public int boost;
}

[System.Serializable]
public class ReducedStats {
    public ModStats reducedStat;
    [Range(-3, 3)] public int reduce;
}
*/
[System.Serializable]
public class UseFunction {
    public BaseObject passInObject;
    public string methodName;
    [TextArea]
    public string actionDescription;
}

[System.Serializable]
public class SpecialInput {
    public Dictionary dictionary;
    public string methodName;
    [TextArea]
    public string actionDescription;
}