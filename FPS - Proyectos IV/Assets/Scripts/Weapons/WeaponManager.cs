using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance;

    public delegate void WeaponEvents(EnemyBase enemyHit, float damageDealt);
    public event WeaponEvents OnHit;

    private void Awake()
    {
        Instance = this;
    }

    public int selectedWeapon = 0;
    public int maxNumberOfWeapons = 2;

    private List<GameObject> weapons;

    public List<GameObject> Weapons { get => weapons; }

    private void Start()
    {
        weapons = ArrangeWeapons();
        NextWeapon();
        InputManager.Instance.OnChangeWeapon += NextWeapon;
    }

    private void Update()
    {
    }

    public void NextWeapon()
    {
        if (selectedWeapon >= maxNumberOfWeapons - 1)
            selectedWeapon = 0;
        else
            selectedWeapon++;

        for (int i = 0; i < weapons.Count; i++)
        {
            if (i == selectedWeapon)
                weapons[i].SetActive(true);
            else
                weapons[i].SetActive(false);
        }

    }

    private List<GameObject> ArrangeWeapons()
    {
        List<GameObject> listWeapons = new List<GameObject>();

        PositionInBuild[] positions = GetComponentsInChildren<PositionInBuild>();
        for (int i = 0; i < maxNumberOfWeapons; i++)
        {
            for (int j = 0; j < positions.Length; j++)
            {
                if (positions[j].positionInBuild == i)
                {
                    listWeapons.Add(positions[i].gameObject);
                    break;
                }
            }
        }
        return listWeapons;
    }

    public void EnemyHit(EnemyBase enemy, float damage)
    {
        Debug.Log(enemy.transform.name);
        OnHit?.Invoke(enemy, damage);
    }
}