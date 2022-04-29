using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameLogicUtility
{

    public static Vector3 LocalPointToWorldPoint(Transform parent, Vector3 localPoint)
    {
        return localPoint + parent.position;
    }

    /// <summary>
    /// Ï´ÅÆ
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static IList<T> Shuffle<T>(this IList<T> list)
    {
        int num = list.Count;
        int ran;
        while (num > 0)
        {
            ran = Random.Range(0, num);
            T temp = list[ran];
            list[ran] = list[num - 1];
            list[num - 1] = temp;
            //list.Insert(list.Count - 1, list[ran]);
            num--;
        }
        return list;
    }
}
