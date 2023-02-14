using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DownloadAssetBundles : MonoBehaviour
{
    [SerializeField] private Image _myImageLoad;
    [SerializeField] private AudioSource _myAudioSourceLoad;
    private enum TypeWanted
    {
        UNKNOWN,
        GAMEOBJECT,
        TEXTURE2D,
        AUDIOCLIP
    }
    private void Start()
    {
        LoadAssetBundle(ActionWantedToAssetBundleLoad, "myPath", "myAsset");
    }

    private IEnumerator DownloadAssetBundleFromServer(Callback<dynamic, TypeWanted> callbackFunction, string assetBundlePath = "", string assetBundleName = "")
    {
        dynamic assetBundleLoad = null;
        TypeWanted typeReceieved = TypeWanted.UNKNOWN;

        string url = assetBundlePath + "/" + assetBundleName;

        using (UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(url))
        {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogWarning($"Error on the get request at : {url} {www.error}");
            }
            else
            {
                AssetBundle assetBundle = DownloadHandlerAssetBundle.GetContent(www);
                assetBundleLoad = assetBundle.LoadAsset(assetBundle.GetAllAssetNames()[0]); // get the first one. This is just a placeholder for actual functionality
                assetBundle.Unload(false); //just for memory
                yield return new WaitForEndOfFrame();
            }
            www.Dispose();
        }
        Debug.Log($"The asset bundle unloaded is a type of : {assetBundleLoad}");
        typeReceieved = (TypeWanted)CheckAssetBundleLoadType(assetBundleLoad);
        callbackFunction(assetBundleLoad, typeReceieved);
    }

    private int CheckAssetBundleLoadType(dynamic assetBundleLoad)
    {
        TypeWanted typeWanted = TypeWanted.UNKNOWN;
        if(assetBundleLoad is GameObject)
        {
            typeWanted = TypeWanted.GAMEOBJECT;
        }
        else if(assetBundleLoad is Texture2D)
        {
            typeWanted = TypeWanted.TEXTURE2D;
        }
        else if(assetBundleLoad is AudioClip)
        {
            typeWanted = TypeWanted.AUDIOCLIP;
        }
        return (int)typeWanted;
    }
    private void LoadAssetBundle(Callback<dynamic, TypeWanted> callbackFunction, string assetBundlePath = "", string assetBundleName = "")
    {
        StartCoroutine(DownloadAssetBundleFromServer(callbackFunction, assetBundlePath, assetBundleName));
    }
    private void ActionWantedToAssetBundleLoad(dynamic assetDownload, TypeWanted typeWanted)
    {
        switch(typeWanted)
        {
            case TypeWanted.UNKNOWN:
                break;
            case TypeWanted.GAMEOBJECT:
                InstantiateGameObjectFromAssetBundle(assetDownload as GameObject);
                break;
            case TypeWanted.TEXTURE2D:
                ApplyImportedSpriteFromAssetBundle(ConvertTexture2DToSprite(assetDownload as Texture2D));
                break;
            case TypeWanted.AUDIOCLIP:
                PlayImportedAudioClipFromAssetBundle(assetDownload as AudioClip);
                break;
            default:
                break;
        }
    }
    private void InstantiateGameObjectFromAssetBundle(GameObject go)
    {
        GameObject instanceGo = Instantiate(go);
        instanceGo.transform.position = Vector3.zero;
        instanceGo.transform.rotation = Quaternion.identity;
    }
    private void ApplyImportedSpriteFromAssetBundle(Sprite s)
    {
        _myImageLoad.sprite = s;
        _myImageLoad.type = Image.Type.Simple;
        _myImageLoad.preserveAspect = true;
    }
    private void PlayImportedAudioClipFromAssetBundle(AudioClip ac)
    {
        _myAudioSourceLoad.clip = ac;
        _myAudioSourceLoad.Play();
    }

    private Sprite ConvertTexture2DToSprite(Texture2D texture2D)
    {
        return Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero);
    }
}

public delegate void Callback();
public delegate void Callback<T>(T arg1);
public delegate void Callback<T, U>(T arg1, U arg2);
public delegate void Callback<T, U, W>(T arg1, U arg2, W arg3);
