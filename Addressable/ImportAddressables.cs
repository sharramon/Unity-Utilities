using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ImportAddressables : MonoBehaviour
{
    public string assetAddress = "myPrefab";
    public GameObject myPrefab;
    // Start is called before the first frame update
    void Start()
    {
        LoadAsset(assetAddress, OnAssetLoaded);
    }

    public void LoadAsset(string address, System.Action<GameObject> onLoaded)
    {
        Addressables.LoadAssetAsync<GameObject>(address).Completed += op =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                onLoaded(op.Result);
            }
        };
    }
    void OnAssetLoaded(GameObject asset)
    {
        myPrefab = asset;
    }
}
