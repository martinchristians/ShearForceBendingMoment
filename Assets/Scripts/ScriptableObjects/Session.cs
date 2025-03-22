using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Session", menuName = "Exercise/Session")]
public class Session : ScriptableObject
{
    public List<Section> sections = new();
}