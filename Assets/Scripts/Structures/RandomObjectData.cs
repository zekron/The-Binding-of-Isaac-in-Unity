using System;
using UnityEngine;

[Serializable]
public class RamdomObjectData
{
    public GameObject RandomObject;
    [Range(0, 100), Tooltip("Max = 100")]
    public int ratio;
}