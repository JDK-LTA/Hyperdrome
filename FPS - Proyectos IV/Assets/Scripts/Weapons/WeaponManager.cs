using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;

        _player = GetComponent<RbFPSController>();
        SceneManager.activeSceneChanged += OnSceneSwitch;
    }

    public int selectedWeapon = 0;
    public int maxNumberOfWeapons = 2;

    [SerializeField] private List<GameObject> weapons;

    private Vector3 respawnPos = Vector3.zero;

    public List<GameObject> Weapons { get => weapons; }

    [HideInInspector] public RbFPSController _player;

    public delegate void WeaponEvents(EnemyBase enemyHit, float damageDealt, Vector3 hitPoint);
    public event WeaponEvents OnHit;
    public delegate void WeaponSimpleEvents();
    public event WeaponSimpleEvents OnWeaponsInit;
    public event WeaponSimpleEvents OnShoot;

    public void TriggerShootEvent()
    {
        OnShoot?.Invoke();
    }

    private void OnSceneSwitch(Scene current, Scene next)
    {
        if (next.name == "Nivel2")
        {
            respawnPos = new Vector3(-45f, -35f, 95f);
            Respawn();
        }
        else if (next.name == "Nivel3")
        {
            respawnPos = new Vector3(262f, 189f, 84f);
            Respawn();
        }
        else if (next.name == "MainMenu")
        {
        }
    }
    public void Respawn()
    {
        transform.position = respawnPos;
    }


    private void Start()
    {
        if (!InputManager.Instance.debug)
        {
            respawnPos = new Vector3(-15.11f, 0.5f, -16.498f);
            Respawn();
        }

        UpdateWeapons();
        InputManager.Instance.OnChangeWeapon += NextWeapon;
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

    public void EnemyHit(EnemyBase enemy, float damage, Vector3 hitPoint)
    {
        OnHit?.Invoke(enemy, damage, hitPoint);
    }
}