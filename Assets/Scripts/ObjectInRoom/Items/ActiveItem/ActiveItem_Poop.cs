using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveItem_Poop : ActiveItem
{
    [Header("Specific Item")]
    [SerializeField] private GameObject poopPrefab;

    protected override void SpecificSkill()
    {
        ObjectPoolManager.Release(poopPrefab, gamePlayer.transform.position);
    }
}
