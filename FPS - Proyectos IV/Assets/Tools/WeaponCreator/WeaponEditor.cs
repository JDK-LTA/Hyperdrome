using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//TODO: Creación y aplicación de cambios de prefabs (una vez cree el arma básica)
public class WeaponEditor : EditorWindow
{
    WeaponList weaponListRef;
    private int viewIndex = 1;

    [MenuItem("Tools/Weapon Creator")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(WeaponEditor));
    }

    private void OnEnable()
    {
        weaponListRef = AssetDatabase.LoadAssetAtPath("Assets/Resources/WeaponList.asset", typeof(WeaponList)) as WeaponList;
    }

    private void OnGUI()
    {
        GUILayout.Label("Weapon Creator", EditorStyles.boldLabel);
        GUILayout.Space(10);
        if (weaponListRef != null)
        {
            PrintTopMenu();
        }
    }

    private void PrintTopMenu()
    {
        GUILayout.BeginHorizontal();

        GUILayout.Space(10);
        if (GUILayout.Button("<- Prev", GUILayout.ExpandWidth(false)))
        {
            if (viewIndex > 1)
            {
                viewIndex--;
            }
            else
            {
                viewIndex = weaponListRef.weaponList.Count;
            }
        }

        GUILayout.Space(5);
        if (GUILayout.Button("Next ->", GUILayout.ExpandWidth(false)))
        {
            if (viewIndex < weaponListRef.weaponList.Count)
            {
                viewIndex++;
            }
            else
            {
                viewIndex = 1;
            }
        }

        GUILayout.Space(10);
        if (GUILayout.Button("+ Add Weapon to list", GUILayout.ExpandWidth(false)))
        {
            AddWeaponToList();
        }

        GUILayout.Space(5);
        if (GUILayout.Button("- Delete Weapon from list", GUILayout.ExpandWidth(false)))
        {
            DeleteFromList(viewIndex - 1);
        }

        GUILayout.EndHorizontal();

        if (weaponListRef.weaponList.Count > 0)
        {
            WeaponListConfigurator();
        }
        else
        {
            GUILayout.Space(10);
            GUILayout.Label("This bitch empty");
        }
    }

    void AddWeaponToList()
    {
        WeaponInfo newWeapon = new WeaponInfo();
        weaponListRef.weaponList.Add(newWeapon);
        viewIndex = weaponListRef.weaponList.Count;
    }

    void DeleteFromList(int i)
    {
        weaponListRef.weaponList.RemoveAt(i);
    }

    void WeaponListConfigurator()
    {
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();

        viewIndex = Mathf.Clamp(EditorGUILayout.IntField("Current Weapon", viewIndex, GUILayout.ExpandWidth(false)), 1, weaponListRef.weaponList.Count);
        EditorGUILayout.LabelField("of  " + weaponListRef.weaponList.Count.ToString() + "  weapons", "", GUILayout.ExpandWidth(false));

        GUILayout.EndHorizontal();

        string[] _choices = new string[weaponListRef.weaponList.Count];
        for (int i = 0; i < weaponListRef.weaponList.Count; i++)
        {
            _choices[i] = weaponListRef.weaponList[i].Name;
        }

        int _choiceIndex = viewIndex - 1;
        viewIndex = EditorGUILayout.Popup(_choiceIndex, _choices) + 1;

        GUILayout.Space(10);
        weaponListRef.weaponList[viewIndex - 1].Name = EditorGUILayout.TextField(new GUIContent("Name"), weaponListRef.weaponList[viewIndex - 1].Name as string);
        weaponListRef.weaponList[viewIndex - 1].ShootingType = (ShootingType)EditorGUILayout.EnumPopup("Shooting type", weaponListRef.weaponList[viewIndex - 1].ShootingType);
        weaponListRef.weaponList[viewIndex - 1].WeaponType = (WeaponType)EditorGUILayout.EnumPopup("Weapon type", weaponListRef.weaponList[viewIndex - 1].WeaponType);
        weaponListRef.weaponList[viewIndex - 1].Changer = (Changer)EditorGUILayout.EnumPopup("Changer", weaponListRef.weaponList[viewIndex - 1].Changer);

        GUILayout.Space(5);
        weaponListRef.weaponList[viewIndex - 1].Mesh = (Mesh)EditorGUILayout.ObjectField("Weapon Model", weaponListRef.weaponList[viewIndex - 1].Mesh, typeof(Mesh), false);
        weaponListRef.weaponList[viewIndex - 1].Bullet = (GameObject)EditorGUILayout.ObjectField("Bullet prefab", weaponListRef.weaponList[viewIndex - 1].Bullet, typeof(GameObject), false);
        weaponListRef.weaponList[viewIndex - 1].CrosshairTexture = (Texture2D)EditorGUILayout.ObjectField("Crosshair texture", weaponListRef.weaponList[viewIndex - 1].CrosshairTexture, typeof(Texture2D), false);
        weaponListRef.weaponList[viewIndex - 1].FireSound = (AudioClip)EditorGUILayout.ObjectField("Fire sound", weaponListRef.weaponList[viewIndex - 1].FireSound, typeof(AudioClip), false);

        GUILayout.Space(5);
        weaponListRef.weaponList[viewIndex - 1].LevelRequired = EditorGUILayout.IntField("Level required", weaponListRef.weaponList[viewIndex - 1].LevelRequired);

        GUILayout.Space(5);
        weaponListRef.weaponList[viewIndex - 1].Ammo = EditorGUILayout.IntField("Ammo", weaponListRef.weaponList[viewIndex - 1].Ammo);
        weaponListRef.weaponList[viewIndex - 1].Weight = EditorGUILayout.FloatField("Weight", weaponListRef.weaponList[viewIndex - 1].Weight);
        weaponListRef.weaponList[viewIndex - 1].DamagePerHit = EditorGUILayout.FloatField("Damage per hit", weaponListRef.weaponList[viewIndex - 1].DamagePerHit);
        weaponListRef.weaponList[viewIndex - 1].ForceToApply = EditorGUILayout.FloatField("Force to apply", weaponListRef.weaponList[viewIndex - 1].ForceToApply);
        weaponListRef.weaponList[viewIndex - 1].Range = EditorGUILayout.FloatField("Range", weaponListRef.weaponList[viewIndex - 1].Range);
        weaponListRef.weaponList[viewIndex - 1].CdBetweenShots = EditorGUILayout.FloatField("Cooldown between shots", weaponListRef.weaponList[viewIndex - 1].CdBetweenShots);

        GUILayout.Space(5);
        weaponListRef.weaponList[viewIndex - 1].NumberToChange = EditorGUILayout.FloatField(new GUIContent("Number to change", "It's seconds to change, ammo to change or hits/kills to change, depending on the changer"),
            weaponListRef.weaponList[viewIndex - 1].NumberToChange);
    }
}
