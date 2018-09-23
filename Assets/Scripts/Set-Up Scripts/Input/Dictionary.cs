using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "TextAdventure/Dictionary")]
public class Dictionary : ScriptableObject {
    public string keyString;
    public Category[] categories;

}

[System.Serializable]
public class Category
{
    public string name;
    public string[] verbs;
}