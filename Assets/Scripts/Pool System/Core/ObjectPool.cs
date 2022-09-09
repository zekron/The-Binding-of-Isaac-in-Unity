using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPool
{
    public GameObject Prefab => prefab;
    public int Size => size;
    public int RuntimeSize => queue.Count;

    [SerializeField] private GameObject prefab;
    [SerializeField] private int size = 1;

    private Queue<GameObject> queue;

    private Transform poolParent;

    public ObjectPool(GameObject prefab, int size = 1)
    {
        this.prefab = prefab;
        this.size = size;
    }

    public void Initialize(Transform parent)
    {
        queue = new Queue<GameObject>();
        poolParent = parent;

        for (var i = 0; i < size; i++)
        {
            queue.Enqueue(InstantiateObject());
        }
    }

    private GameObject InstantiateObject()
    {
        var copy = Object.Instantiate(prefab, poolParent);

        copy.SetActive(false);

        return copy;
    }

    private GameObject GetAvailableObject()
    {
        GameObject availableObject;

        if (queue.Count > 0 && !queue.Peek().activeSelf)
        {
            availableObject = queue.Dequeue();
        }
        else
        {
            availableObject = InstantiateObject();
        }

        queue.Enqueue(availableObject);

        return availableObject;
    }

    public GameObject PreparedObject()
    {
        GameObject preparedObject = GetAvailableObject();

        preparedObject.SetActive(true);

        return preparedObject;
    }

    public GameObject PreparedObject(Vector3 position)
    {
        GameObject preparedObject = GetAvailableObject();

        preparedObject.transform.position = position;
        preparedObject.SetActive(true);

        return preparedObject;
    }

    public GameObject[] PreparedObject(Vector3[] positions)
    {
        GameObject[] preparedObjects = new GameObject[positions.Length];

        for (int i = 0; i < positions.Length; i++)
        {
            preparedObjects[i] = GetAvailableObject();
            preparedObjects[i].transform.position = positions[i];
            preparedObjects[i].SetActive(true);
        }

        return preparedObjects;
    }

    public GameObject PreparedObject(Vector3 position, Quaternion rotation)
    {
        GameObject preparedObject = GetAvailableObject();

        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;
        preparedObject.SetActive(true);

        return preparedObject;
    }

    public GameObject PreparedObject(Vector3 position, Quaternion rotation, Vector3 localScale)
    {
        GameObject preparedObject = GetAvailableObject();

        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;
        preparedObject.transform.localScale = localScale;
        preparedObject.SetActive(true);

        return preparedObject;
    }

    public GameObject PreparedObject(Vector3 position, Quaternion rotation, Transform parent)
    {
        GameObject preparedObject = GetAvailableObject();

        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;
        preparedObject.transform.parent = parent;
        preparedObject.SetActive(true);

        return preparedObject;
    }
}