using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GoalItem
{
    public Sprite item;
    public FruitType type;
    public int amount;
}

[CreateAssetMenu(menuName ="RequirementLevel")]
public class LevelSO : ScriptableObject
{
    public int step;
    public List<GoalItem> items= new List<GoalItem>();
    
}
