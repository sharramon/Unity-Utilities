using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using System.IO;

[CustomEditor(typeof(CreateAddressables))]
public class CreateAddressableGUI : Editor
{
    public override void OnInspectorGUI()
    {
        CreateAddressables _createAddressables = target as CreateAddressables;

        base.OnInspectorGUI();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Addressable Group Name");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        _createAddressables._groupName =  EditorGUILayout.TextField(_createAddressables._groupName);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if(GUILayout.Button("Create Addressable Group"))
        {
            //Debug.Log("I was clicked");
            CreateAddressableGroup(_createAddressables._groupName);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Addressable File Name");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        _createAddressables._fileName = EditorGUILayout.TextField(_createAddressables._fileName);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Create Addressables"))
        {
            //Debug.Log("I was clicked");
            AddAddressables(_createAddressables._fileName, _createAddressables._groupName);
        }
        EditorGUILayout.EndHorizontal();
    }

    public void CreateAddressableGroup(string groupName)
    {
        if (groupName == "")
        {
            Debug.LogError("Please Input a groupName");
            return;
        }

        AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
        settings.CreateGroup(groupName, false, false, true, null);
    }
    public void AddAddressables(string fileName, string groupName)
    {
        string[] files = Directory.GetFiles(fileName, "*.prefab", SearchOption.AllDirectories);

        //foreach (UnityEngine.Object asset in Selection.objects)
        //{
        //    //get the asset path
        //    string assetPath = AssetDatabase.GetAssetPath(asset);
        //    AddAssetToAddressableGroup(_groupName, assetPath, asset.name);
        //}

        foreach (string assetPath in files)
        {
            //get the asset path
            Debug.Log($"asset path is {assetPath}");
            string name = assetPath.Substring(assetPath.LastIndexOf("/") + 1);
            Debug.Log($"asset name is {name}");
            AddAssetToAddressableGroup(groupName, assetPath, name);
        }
    }
    public void AddAssetToAddressableGroup(string groupName, string assetPath, string address)
    {
        AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;

        if (settings.FindGroup(groupName) == null)
        {
            Debug.LogError($"Group of name {groupName} does not exist");
            return;
        }

        if (AssetDatabase.AssetPathToGUID(assetPath) == null)
        {
            Debug.LogError($"The asset path {assetPath} does not exist");
        }

        AddressableAssetGroup group = settings.FindGroup(groupName);
        AddressableAssetEntry entry = settings.CreateOrMoveEntry(AssetDatabase.AssetPathToGUID(assetPath), group);
        entry.address = address;

        Debug.Log($"Addressable set for {assetPath}, in group {groupName}, with address {address}");
    }
}
