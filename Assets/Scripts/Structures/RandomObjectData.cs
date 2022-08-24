using System;
using UnityEngine;

[Serializable]
public class RandomObjectData
{
    /// <summary>
    /// Object in random pool
    /// </summary>
    public GameObject RandomObject;
    [Range(0, 100), Tooltip("Object spawn ratio. Max = 100")]
    public int ratio;
}