using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Section", menuName = "Exercise/Section")]
public class Section : ScriptableObject
{
    [SerializeField] private string title;

    public List<Task> tasks = new();
}