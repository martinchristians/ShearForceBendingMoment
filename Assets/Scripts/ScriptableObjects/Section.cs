using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Section", menuName = "Exercise/Section")]
public class Section : ScriptableObject
{
    public List<Task> tasks = new();
}