using System;
using UnityEditor;
using UnityEngine;
using System.IO;

public class CreateAssetBundles
{
    [MenuItem("AssetBundles/Create Asset Bundles")]
    private static void BuildAllAssetBundles()
    {
        string assetBundleDirectoryPath = Application.dataPath + "/../AssetBundles";

        try
        {
            BuildPipeline.BuildAssetBundles(assetBundleDirectoryPath, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
        }
        catch(Exception e)
        {
            Debug.LogWarning(e);
        }
    }

    [MenuItem("AssetBundles/Set Asset Bundle From File Name")]
    private static void SetAssetBundlesFromFileNames()
    {
        if (Selection.assetGUIDs.Length > 0)
        {
            Debug.Log($"bundles started with length of {Selection.assetGUIDs.Length}");
            Debug.Log($"bundles objects length is {Selection.objects.Length}");
            foreach (UnityEngine.Object asset in Selection.objects)
            {
                //get the asset path
                string assetPath = AssetDatabase.GetAssetPath(asset);
                Debug.Log($"path is {assetPath}");
                AssetImporter assetImporter = AssetImporter.GetAtPath(assetPath);

                //get name of folder of asset and set that to the name of the bundle
                string folderName = Path.GetFileName(Path.GetDirectoryName(assetPath));
                Debug.Log($"directory name is {Path.GetDirectoryName(assetPath)}");
                Debug.Log($"Asset bundle name is {folderName}");
                assetImporter.assetBundleName = folderName;
                Debug.Log(Selection.assetGUIDs.Length + " Asset Bundles Assigned");
            }
        }
        else
        {
            Debug.Log("No Assets Selected");
        }
    }
}
