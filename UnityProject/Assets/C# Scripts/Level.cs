using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "Level Data", order = 1)]
public class Level : ScriptableObject
{
    public new string name;
    public Sprite icon;
}
