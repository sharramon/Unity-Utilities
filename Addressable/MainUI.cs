using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


public class MainUI : MonoBehaviour
{
    [SerializeField] Button _redButton;
    [SerializeField] Button _blueButton;

    bool hasCreated = false;

    AsyncOperationHandle<GameObject> opHandle;
    GameObject _previousObject;

    [SerializeField] List<string> _addressableKeys = new List<string>();

    private void Start()
    {
        _redButton.onClick.AddListener(OnRedButtonPressed);
        _blueButton.onClick.AddListener(OnBlueButtonPressed);
    }

    private void OnRedButtonPressed()
    {
        StartCoroutine(Instantiate(_addressableKeys[1]));
    }

    private void OnBlueButtonPressed()
    {
        StartCoroutine(Instantiate(_addressableKeys[0]));

    }

    private IEnumerator Instantiate(string key)
    {
        if(hasCreated) //check for first instance where nothing is loaded yet
            Addressables.ReleaseInstance(opHandle); //first destroy previous

        hasCreated = true;

        //opHandle = Addressables.LoadAssetAsync<GameObject>(key);
        opHandle = Addressables.InstantiateAsync(key);
        yield return opHandle;

        //if (opHandle.Status == AsyncOperationStatus.Succeeded)
        //{
        //    GameObject obj = opHandle.Result;
        //    _previousObject = Instantiate(obj, transform);
        //}
    }

    private void OnDestroy()
    {
        //Destroy(_previousObject);
        Addressables.ReleaseInstance(opHandle);
    }
}
