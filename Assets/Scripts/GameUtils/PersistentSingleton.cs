using UnityEngine;

public class PersistentSingleton<T> : MonoBehaviour where T : Component
{
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                DontDestroyOnLoad(instance.gameObject);
            }
            if (instance == null)
            {
                var go = new GameObject(string.Format("PersistentSingleton_{0}", typeof(T).Name), typeof(T));
                instance = go.GetComponent<T>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }
    private static T instance;

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
}