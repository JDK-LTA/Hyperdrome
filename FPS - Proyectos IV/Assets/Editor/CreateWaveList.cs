using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
public class CreateWaveList
{
    [MenuItem("Tools/Create Wave List")]
    public static WaveList Create()
    {
        WaveList asset = ScriptableObject.CreateInstance<WaveList>();
        AssetDatabase.CreateAsset(asset, "Assets/Resources/WaveList.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
}
#endif
