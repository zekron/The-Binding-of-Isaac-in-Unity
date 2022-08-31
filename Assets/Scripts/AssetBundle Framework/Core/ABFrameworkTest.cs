using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssetBundleFramework;

public class ABFrameworkTest : MonoBehaviour
{
    GameObject circlePrefab;
    GameObject squarePrefab;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Unload material");
            AssetBundleManager.Instance.ReleaseResources("Mat_Black.mat");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Unload all materials");
            AssetBundleManager.Instance.ReleaseAllResources();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("Unload material after unload prefab");
            AssetBundleManager.Instance.ReleaseResources("Square.prefab");
            AssetBundleManager.Instance.ReleaseResources("Mat_Black.mat");
        }


        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Debug.Log("Load Square.prefab");
            AssetBundleManager.Instance.LoadAsset<GameObject>("Square.prefab");
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            Debug.Log("Instantiate Square.prefab");
            squarePrefab = AssetBundleManager.Instance.LoadAsset<GameObject>("Square.prefab");
            Instantiate(squarePrefab);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Debug.Log("Load Circle.prefab");
            AssetBundleManager.Instance.LoadAsset<GameObject>("Circle.prefab");
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            Debug.Log("Instantiate Square.prefab");
            circlePrefab = AssetBundleManager.Instance.LoadAsset<GameObject>("Circle.prefab");
            Instantiate(circlePrefab);
        }
    }
}
