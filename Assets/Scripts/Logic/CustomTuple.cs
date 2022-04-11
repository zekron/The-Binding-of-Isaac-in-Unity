using System;
using UnityEngine;

public class CustomTuple<T1, T2>
{
    public T1 value1;
    public T2 value2;
    public CustomTuple(T1 item1, T2 item2)
    {
        value1 = item1;
        value2 = item2;
    }
}

//要可视化操作必须使用具体类型
[Serializable]
public class TupleWithGameObjectInt : CustomTuple<GameObject, int>
{
    public TupleWithGameObjectInt(GameObject value1, int value2) : base(value1, value2) { }
}

[Serializable]
public class TupleWithGameObjectVector2 : CustomTuple<GameObject, Vector2>
{
    public TupleWithGameObjectVector2(GameObject value1, Vector2 value2) : base(value1, value2) { }
}
