using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Tear SO",menuName = "Scriptable Object/Tear")]
public class TearSO : ScriptableObject
{
    [SerializeField] private Sprite tearUISprite;
    [SerializeField] private GameObject tearPrefab;
    [SerializeField] private int tearType = 0b0;
}
