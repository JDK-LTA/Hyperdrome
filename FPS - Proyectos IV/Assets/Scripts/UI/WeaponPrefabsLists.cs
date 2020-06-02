using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaponPrefabsLists : MonoBehaviour
{
    public static WeaponPrefabsLists Instance;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    public List<List<GameObject>> weaponPrefabLists;
    public GameObject[] prefabs;
    public GameObject inventory;
    // Start is called before the first frame update
    void Start()
    {
        prefabs = Resources.LoadAll<GameObject>("WEAPON PREFABS");
        weaponPrefabLists = new List<List<GameObject>>();

        for (int i = 0; i < 30; i++)
        {
            weaponPrefabLists.Add(new List<GameObject>());
        }

        for (int i = 0; i < prefabs.Length; i++)
        {
            WeaponBase wb = prefabs[i].GetComponent<WeaponBase>();
            weaponPrefabLists[wb.LevelRequired].Add(prefabs[i]);
        }

        weaponPrefabLists.TrimExcess();
    }

    public void Init()
    {
        inventory = Resources.FindObjectsOfTypeAll<InventoryManager>()[0].gameObject;
    }
    public void ResetInit()
    {
        inventory = null;
    }
}
