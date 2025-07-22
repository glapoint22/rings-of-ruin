using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Rings of Ruin/Level Data")]
public class LevelData : ScriptableObject
{
    public List<Ring> rings = new List<Ring>();
    public bool hasRuneFlares = false;
    public float minRuneFlaresSpawnInterval = 0;
    public float maxRuneFlaresSpawnInterval = 0;
}