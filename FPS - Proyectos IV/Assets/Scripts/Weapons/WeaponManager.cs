using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance;

    public delegate void WeaponEvents(EnemyBase enemyHit, float damageDealt);
    public event WeaponEvents OnHit;
    public delegate void WeaponSimpleEvents();
    public event WeaponSimpleEvents OnWeaponsInit;
    public event WeaponSimpleEvents OnShoot;

    public void TriggerShootEvent()
    {
        OnShoot?.Invoke();
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }


    public int selectedWeapon = 0;
    public int maxNumberOfWeapons = 2;

    [SerializeField] private List<GameObject> weapons;

    public List<GameObject> Weapons { get => weapons; }

    [HideInInspector] public RbFPSController _player;

    private void Start()
    {
        _player = GetComponent<RbFPSController>();

        UpdateWeapons();
        InputManager.Instance.OnChangeWeapon += NextWeapon;
    }

    bool init = false;
    private void Update()
    {
        if (!init)
        {
            init = true;
            OnWeaponsInit?.Invoke();
        }
    }

    private void UpdateUIWeaponsTexts()
    {
        WeaponBase wb = weapons[selectedWeapon].GetComponent<WeaponBase>();

        UIManager.Instance.UpdateNoToChangeText(wb.auxNoToChange, wb.NumberToChange, wb.Changer);
        UIManager.Instance.UpdateChangerText(wb.Changer);
        UIManager.Instance.UpdateSelectedWeaponText(selectedWeapon + 1, weapons.Count);
    }

    private void GetFirstWeapon()
    {
        selectedWeapon = 0;
        weapons[selectedWeapon].SetActive(true);

        if (weapons.Count > 1)
        {
            for (int i = 1; i < weapons.Count; i++)
            {
                weapons[i].SetActive(false);
            }
        }

        UpdateUIWeaponsTexts();
    }
    public void NextWeapon()
    {
        if (selectedWeapon >= weapons.Count - 1)
            selectedWeapon = 0;
        else
            selectedWeapon++;

        for (int i = 0; i < weapons.Count; i++)
        {
            if (i == selectedWeapon)
            {
                weapons[i].SetActive(true);
            }
            else
                weapons[i].SetActive(false);
        }

        UpdateUIWeaponsTexts();
    }

    public IEnumerator UpdateWeaponsCoroutine()
    {
        yield return null;
        UpdateWeapons();
    }

    public void UpdateWeapons()
    {
        weapons = ArrangeWeapons();
        GetFirstWeapon();

        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].GetComponent<WeaponBase>().ResetWeapon();
        }
    }
    private List<GameObject> ArrangeWeapons()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].SetActive(true);
        }

        List<GameObject> listWeapons = new List<GameObject>();
        PositionInBuild[] positions = _player.weaponsParent.GetComponentsInChildren<PositionInBuild>();

        for (int i = 0; i < positions.Length; i++)
        {
            listWeapons.Add(positions[i].gameObject);
        }

        listWeapons.Sort(delegate (GameObject p1, GameObject p2) { return p1.GetComponent<PositionInBuild>().positionInBuild.CompareTo(p2.GetComponent<PositionInBuild>().positionInBuild); });

        return listWeapons;
    }

    public void EnemyHit(EnemyBase enemy, float damage)
    {
        OnHit?.Invoke(enemy, damage);
    }
}