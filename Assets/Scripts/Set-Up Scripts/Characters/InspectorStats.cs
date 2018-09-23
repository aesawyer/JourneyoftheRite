using UnityEngine;

[System.Serializable]
public class BaseStats {

	[Range(1, 12)] public int strength;
	[Range(1, 12)] public int intelligence;
	[Range(1, 12)] public int dexterity;
	[Range(1, 12)] public int charisma;
    [Range(1, 12)] public int endurance;
    [Range(1, 12)] public int grace;

    [HideInInspector] public int speed;
    [HideInInspector] public int fight;
    [HideInInspector] public int stealth;
    [HideInInspector] public int dodge;
    [HideInInspector] public int will;
    [HideInInspector] public int perception;

    [HideInInspector] public int curHealth;
    [HideInInspector] public int maxHealth;
    [HideInInspector] public int curEnergy;
    [HideInInspector] public int maxEnergy;
    [HideInInspector] public int damage;

}

[System.Serializable]
public enum ModStats {
    none,
    intelligence,
    dexterity,
    charisma,
    endurance,
    maxHealth,
    speed,
    stealth,
    dodge,
}