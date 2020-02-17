using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateWeaponList
{
    [MenuItem("Assets/Lists/Weapon List")]
    public static WeaponList Create()
    {
        WeaponList asset = ScriptableObject.CreateInstance<WeaponList>();
        AssetDatabase.CreateAsset(asset, "Assets/Resources/WeaponList.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
}
