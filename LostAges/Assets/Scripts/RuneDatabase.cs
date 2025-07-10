using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class RuneMapping
{
    public Sprite runeIcon;
    public string translation;
}

[CreateAssetMenu(fileName = "RuneDatabase", menuName = "Rune/Database")]
public class RuneDatabase : ScriptableObject
{
    public List<RuneMapping> runeMappings;
}