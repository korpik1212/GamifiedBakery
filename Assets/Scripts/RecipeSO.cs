using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "Recipe", menuName = "ScriptableObjects/RecipeSO", order = 1)]
public class RecipeSO : ScriptableObject
{
    public List<Sequence> sequences = new List<Sequence>();
}


[System.Serializable]
public class Sequence
{
    public enum SequenceType
    {
        Action,
        Weight
    }
    public string seqName;
    public SequenceType sequenceType;
    public float weight;
    public float duration;
    public string description;
    public Sprite icon;


}