using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
//TODO: Creación y aplicación de cambios de prefabs (una vez cree el arma básica)
public class WeaponEditor : EditorWindow
{
    WeaponList weaponListRef;
    private int viewIndex = 1;
    [SerializeField] private GameObject prefabToCopy;

    //public List<Material[]> materialsList;
    //public Material[] materials;
    //List<SerializedObject> so;
    //List<SerializedProperty> sp;

    [MenuItem("Tools/Weapon Creator")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(WeaponEditor));
    }

    private void OnEnable()
    {
        //so = new List<SerializedObject>();
        //sp = new List<SerializedProperty>();
        //materialsList = new List<Material[]>();

        weaponListRef = AssetDatabase.LoadAssetAtPath("Assets/Tools/Lists/WeaponList.asset", typeof(WeaponList)) as WeaponList;

        //for (int i = 0; i < weaponListRef.weaponList.Count; i++)
        //{
        //    MaterialsAuxiliarMethod(weaponListRef.weaponList[i].Materials);
        //}
    }

    //private void MaterialsAuxiliarMethod(Material[] aux)
    //{
    //    materials = aux;
    //    materialsList.Add(materials);

    //    SerializedObject serializedObject = new SerializedObject(this);
    //    so.Add(serializedObject);
    //    sp.Add(serializedObject.FindProperty("materials"));
    //}

    private void OnGUI()
    {
        GUILayout.Label("Weapon Creator", EditorStyles.boldLabel);
        GUILayout.Space(10);
        if (weaponListRef != null)
        {
            PrintTopMenu();
        }
        else
        {
            GUILayout.Space(10);
            GUILayout.Label("Can't load the weapon list");
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(weaponListRef);
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

        //MaterialsAuxiliarMethod(newWeapon.Materials);

        viewIndex = weaponListRef.weaponList.Count;
    }

    void DeleteFromList(int i)
    {
        if (!weaponListRef.weaponList[i].IsCreated)
        {
            weaponListRef.weaponList.RemoveAt(i);
            if (i == weaponListRef.weaponList.Count)
            {
                viewIndex--;
            }
            //materialsList.RemoveAt(i);
        }

        //IF THE PREFAB ASSET HAS BEEN DESTROYED MANUALLY
        else if (AssetDatabase.LoadMainAssetAtPath("Assets/Resources/WEAPON PREFABS/CW_" + weaponListRef.weaponList[i].PreviousName + ".prefab") == null)
        {
            throw new System.Exception("The " + weaponListRef.weaponList[i].Name + " has been manually deleted. Hit the 'Destroy prefab' button first.");
        }
        //IF THE PREFAB EXISTS AND YOU WANT TO REMOVE IT FROM THE LIST
        else
        {
            throw new System.Exception("The " + weaponListRef.weaponList[i].Name + " prefab still exists. You cannot remove it from the list.");
        }
    }

    void CreateOrApplyPrefab(int i)
    {
        GameObject tempWeapon;

        if (!weaponListRef.weaponList[i].IsCreated)
        {
            weaponListRef.weaponList[i].IsCreated = true;
            weaponListRef.weaponList[i].PreviousName = weaponListRef.weaponList[i].Name;

            prefabToCopy = (GameObject)AssetDatabase.LoadMainAssetAtPath("Assets/Tools/WeaponCreator/CW_Configurable.prefab");

            tempWeapon = (GameObject)PrefabUtility.InstantiatePrefab(prefabToCopy);
            ApplyChanges(tempWeapon, i);

            PrefabUtility.SaveAsPrefabAsset(tempWeapon, "Assets/Resources/WEAPON PREFABS/CW_" + weaponListRef.weaponList[i].Name + ".prefab");
        }
        else
        {
            prefabToCopy = (GameObject)AssetDatabase.LoadMainAssetAtPath("Assets/Resources/WEAPON PREFABS/CW_" + weaponListRef.weaponList[i].PreviousName + ".prefab");

            if (prefabToCopy == null)
            {
                throw new System.Exception("The " + weaponListRef.weaponList[i].Name + " prefab has been manually deleted. Hit the 'Destroy button', and create it again.");
            }
            else
            {
                tempWeapon = (GameObject)PrefabUtility.InstantiatePrefab(prefabToCopy);
                ApplyChanges(tempWeapon, i);

                //IF CAR NAME WAS CHANGED, CHANGE ITS PREFAB'S NAME
                if (weaponListRef.weaponList[i].PreviousName != weaponListRef.weaponList[i].Name)
                {
                    AssetDatabase.RenameAsset("Assets/Resources/WEAPON PREFABS/CW_" + weaponListRef.weaponList[i].PreviousName + ".prefab", "CW_" + weaponListRef.weaponList[i].Name);
                    weaponListRef.weaponList[i].PreviousName = weaponListRef.weaponList[i].Name;
                }

                PrefabUtility.ApplyPrefabInstance(tempWeapon, InteractionMode.UserAction);
            }
        }

        DestroyImmediate(tempWeapon);
    }
    void CreateOrApplyPrefab()
    {
        //Debug.Log(materialsList[0][0].name);
        //Debug.Log(weaponListRef.weaponList.Count);
        for (int i = 0; i < weaponListRef.weaponList.Count; i++)
        {
            CreateOrApplyPrefab(i);
        }
    }
    void DestroyPrefab()
    {
        weaponListRef.weaponList[viewIndex - 1].IsCreated = false;
        FileUtil.DeleteFileOrDirectory("Assets/Resources/WEAPON PREFABS/CW_" + weaponListRef.weaponList[viewIndex - 1].Name + ".prefab");
        AssetDatabase.Refresh();
    }

    void ApplyChanges(GameObject go, int i)
    {
        WeaponBase wc;
        ShotBase sc;
        WeaponInfo info = weaponListRef.weaponList[i];

        LineRenderer lr;
        if (lr = go.GetComponent<LineRenderer>())
        {
            DestroyImmediate(lr);
        }

        switch (info.WeaponType)
        {
            case WeaponType.NORMAL:
                if (!(wc = go.GetComponent<WeaponBase>()))
                {
                    wc = go.AddComponent<WeaponNormal>();
                }
                else
                {
                    if (!(wc is WeaponNormal))
                    {
                        DestroyImmediate(wc);
                        wc = go.AddComponent<WeaponNormal>();
                    }
                }
                break;
            case WeaponType.LASER:
                if (!(wc = go.GetComponent<WeaponBase>()))
                {
                    wc = go.AddComponent<WeaponLaser>();
                }
                else
                {
                    if (!(wc is WeaponLaser))
                    {
                        DestroyImmediate(wc);
                        wc = go.AddComponent<WeaponLaser>();
                    }
                }
                lr = go.AddComponent<LineRenderer>();
                WeaponLaser wl = wc.GetComponent<WeaponLaser>();
                wl.LineStartWidth = info.LineStartWidth;
                wl.LineEndWidth = info.LineEndWidth;
                wl.EndSpeed = info.EndSpeed;
                break;
            case WeaponType.SHOTGUN:
                if (!(wc = go.GetComponent<WeaponBase>()))
                {
                    wc = go.AddComponent<WeaponShotgun>();
                }
                else
                {
                    if (!(wc is WeaponShotgun))
                    {
                        DestroyImmediate(wc);
                        wc = go.AddComponent<WeaponShotgun>();
                    }
                }

                wc.GetComponent<WeaponShotgun>().NOfBulletsPerShot = info.NOfBulletsPerShot;
                break;
            case WeaponType.ROCKET_LAUNCHER:
                if (!(wc = go.GetComponent<WeaponBase>()))
                {
                    wc = go.AddComponent<WeaponRocket>();
                }
                else
                {
                    if (!(wc is WeaponRocket))
                    {
                        DestroyImmediate(wc);
                        wc = go.AddComponent<WeaponRocket>();
                    }
                }
                break;
            default:
                if (!(wc = go.GetComponent<WeaponBase>()))
                {
                    wc = go.AddComponent<WeaponNormal>();
                }
                else
                {
                    if (!(wc is WeaponNormal))
                    {
                        DestroyImmediate(wc);
                        wc = go.AddComponent<WeaponNormal>();
                    }
                }
                break;
        }
        switch (info.ShootingType)
        {
            case ShootingType.LOCK:
                if (!(sc = go.GetComponent<ShotBase>()))
                {
                    sc = go.AddComponent<ShotLock>();
                }
                else
                {
                    if (!(sc is ShotLock))
                    {
                        DestroyImmediate(sc);
                        sc = go.AddComponent<ShotLock>();
                    }
                }
                break;
            case ShootingType.SEMI_AUTOMATIC:
                if (!(sc = go.GetComponent<ShotBase>()))
                {
                    sc = go.AddComponent<ShotSemi>();
                }
                else
                {
                    if (!(sc is ShotSemi))
                    {
                        DestroyImmediate(sc);
                        sc = go.AddComponent<ShotSemi>();
                    }
                }
                break;
            case ShootingType.AUTOMATIC:
                if (!(sc = go.GetComponent<ShotBase>()))
                {
                    sc = go.AddComponent<ShotAuto>();
                }
                else
                {
                    if (!(sc is ShotAuto))
                    {
                        DestroyImmediate(sc);
                        sc = go.AddComponent<ShotAuto>();
                    }
                }
                break;
            case ShootingType.HOLD:
                if (!(sc = go.GetComponent<ShotBase>()))
                {
                    sc = go.AddComponent<ShotHold>();
                }
                else
                {
                    if (!(sc is ShotHold))
                    {
                        DestroyImmediate(sc);
                        sc = go.AddComponent<ShotHold>();
                    }
                }
                break;
            default:
                if (!(sc = go.GetComponent<ShotBase>()))
                {
                    sc = go.AddComponent<ShotAuto>();
                }
                else
                {
                    if (!(sc is ShotAuto))
                    {
                        DestroyImmediate(sc);
                        sc = go.AddComponent<ShotAuto>();
                    }
                }
                break;
        }

        #region General properties
        wc.Name = info.Name;

        wc.ShootingType = info.ShootingType;
        wc.WeaponType = info.WeaponType;
        wc.Changer = info.Changer;

        go.GetComponent<MeshFilter>().mesh = info.Mesh;
        //Debug.Log(materials[i].name);
        //go.GetComponent<MeshRenderer>().materials = materialsList[i];

        wc.Bullet = info.Bullet;
        wc.FireSound = info.FireSound;

        wc.LevelRequired = info.LevelRequired;

        //wc.Ammo = info.Ammo;
        //wc.Weight = info.Weight;
        wc.DamagePerHit = info.DamagePerHit;
        wc.ForceToApply = info.ForceToApply;
        wc.Range = info.Range;
        wc.CdBetweenShots = info.CdBetweenShots;
        sc.CdBetweenShots = info.CdBetweenShots;
        wc.Variance = info.Variance;
        wc.VarianceDecreaseWhenAim = info.VarianceDecreaseWhenAim;
        wc.SpeedDecreaseWhenAim = info.SpeedDecreaseWhenAim;

        wc.NumberToChange = info.NumberToChange;

        //wc.RaycastSpot = info.RaycastSpot;
        #endregion
    }

    void WeaponListConfigurator()
    {
        WeaponInfo weaponInfo = weaponListRef.weaponList[viewIndex - 1];

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

        GUILayout.Label("\"Type of weapon\" variables", EditorStyles.boldLabel);
        weaponInfo.Name = EditorGUILayout.TextField(new GUIContent("Name"), weaponInfo.Name as string);
        weaponInfo.ShootingType = (ShootingType)EditorGUILayout.EnumPopup("Shooting type", weaponInfo.ShootingType);
        weaponInfo.WeaponType = (WeaponType)EditorGUILayout.EnumPopup("Weapon type", weaponInfo.WeaponType);
        weaponInfo.Changer = (Changer)EditorGUILayout.EnumPopup("Changer", weaponInfo.Changer);
        weaponInfo.NumberToChange = EditorGUILayout.FloatField(new GUIContent("Number to change", "It's seconds to change, ammo to change or hits/kills to change, depending on the changer"),
            weaponInfo.NumberToChange);
        weaponInfo.LevelRequired = EditorGUILayout.IntField("Level required", weaponInfo.LevelRequired);

        GUILayout.Label("Art and design variables", EditorStyles.boldLabel);
        weaponInfo.Mesh = (Mesh)EditorGUILayout.ObjectField("Weapon Model", weaponInfo.Mesh, typeof(Mesh), false);

        //EditorGUILayout.PropertyField(sp[viewIndex - 1], true);
        //so[viewIndex - 1].ApplyModifiedProperties();
        //weaponInfo.Materials = materials;

        weaponInfo.CrosshairTexture = (Sprite)EditorGUILayout.ObjectField("Crosshair texture", weaponInfo.CrosshairTexture, typeof(Sprite), false);
        weaponInfo.Bullet = (GameObject)EditorGUILayout.ObjectField("Bullet prefab", weaponInfo.Bullet, typeof(GameObject), false);
        weaponInfo.FireSound = (AudioClip)EditorGUILayout.ObjectField("Fire sound", weaponInfo.FireSound, typeof(AudioClip), false);


        GUILayout.Label("General weapon variables", EditorStyles.boldLabel);
        //weaponInfo.Ammo = EditorGUILayout.IntField("Ammo", weaponInfo.Ammo);
        //weaponInfo.Weight = EditorGUILayout.FloatField("Weight", weaponInfo.Weight);
        weaponInfo.DamagePerHit = EditorGUILayout.FloatField("Damage per hit", weaponInfo.DamagePerHit);
        weaponInfo.ForceToApply = EditorGUILayout.FloatField("Force to apply", weaponInfo.ForceToApply);
        weaponInfo.Range = EditorGUILayout.FloatField("Range", weaponInfo.Range);
        weaponInfo.CdBetweenShots = EditorGUILayout.FloatField("Cooldown between shots", weaponInfo.CdBetweenShots);
        weaponInfo.Variance = EditorGUILayout.FloatField(new GUIContent("Variance between shots", "Less variance = more accuracy"), weaponInfo.Variance);
        weaponInfo.VarianceDecreaseWhenAim = EditorGUILayout.FloatField(new GUIContent("Variance decrease aiming", "It's a rate. It divides the Variance property"), weaponInfo.VarianceDecreaseWhenAim);
        weaponInfo.SpeedDecreaseWhenAim = EditorGUILayout.FloatField(new GUIContent("Speed decrease aiming", "It's a rate. It divides the speed"), weaponInfo.SpeedDecreaseWhenAim);

        GUILayout.Space(5);
        GUILayout.Label("Specific weapon variables", EditorStyles.boldLabel);
        switch (weaponListRef.weaponList[viewIndex - 1].WeaponType)
        {
            case WeaponType.NORMAL:
                break;
            case WeaponType.LASER:
                weaponInfo.EndSpeed = EditorGUILayout.FloatField("Speed of laser's end", weaponInfo.EndSpeed);

                weaponInfo.LineStartWidth = EditorGUILayout.FloatField("Width of laser's start", weaponInfo.LineStartWidth);

                weaponInfo.LineEndWidth = EditorGUILayout.FloatField("Width of laser's end", weaponInfo.LineEndWidth);
                break;
            case WeaponType.SHOTGUN:
                weaponInfo.NOfBulletsPerShot = EditorGUILayout.IntField("Number of bullets per shot", weaponInfo.NOfBulletsPerShot);
                break;
            case WeaponType.ROCKET_LAUNCHER:
                break;
        }

        GUILayout.Space(20);
        GUILayout.BeginHorizontal();

        //Debug.Log(weaponInfo.IsCreated);
        if (!weaponInfo.IsCreated)
        {
            if (GUILayout.Button("Create Prefab", GUILayout.ExpandWidth(false)))
            {
                CreateOrApplyPrefab(viewIndex - 1);
            }
        }
        else
        {
            if (GUILayout.Button("Apply Changes", GUILayout.ExpandWidth(false)))
            {
                CreateOrApplyPrefab(viewIndex - 1);
            }
            if (GUILayout.Button("Destroy Prefab", GUILayout.ExpandWidth(false)))
            {
                DestroyPrefab();
            }
        }

        if (GUILayout.Button("Create and apply ALL", GUILayout.ExpandWidth(false)))
        {
            CreateOrApplyPrefab();
        }

        GUILayout.EndHorizontal();
    }
}
#endif