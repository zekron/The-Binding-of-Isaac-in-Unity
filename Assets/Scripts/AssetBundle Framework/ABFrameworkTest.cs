using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssetBundleFramework;

public class ABFrameworkTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var prefab = AssetBundleManager.Instance.LoadAsset<GameObject>("Square.prefab");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
