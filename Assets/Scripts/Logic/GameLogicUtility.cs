using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogicUtility
{

    public static Vector3 LocalPointToWorldPoint(Transform parent, Vector3 localPoint)
    {
        return localPoint + parent.position;
    }
}
