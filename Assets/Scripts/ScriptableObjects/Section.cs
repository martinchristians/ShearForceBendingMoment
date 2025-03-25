using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Section", menuName = "Exercise/Section")]
public class Section : ScriptableObject
{
    public string title;
    public int sectionIndex = -1;
    public List<Task> tasks = new();
}