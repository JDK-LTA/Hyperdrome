using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: CHANGE WEAPONS WHEN DRAG AND DROPPED ON TOP OF ANOTHER ONE. NOT A CLEAR ANSWER YET AS HOW TO DO IT
public class InventorySystem : MonoBehaviour
{
    [SerializeField] Transform itemsParent;
    [SerializeField] private List<UIWeapon> weaponSlots;
    public UIWeapon selectedWeaponSlot;
    public Transform selectedItem;

    [SerializeField] private GameObject weaponPanel;

    public delegate void InventoryEvents();
    public event InventoryEvents OnBeginDragEvent;
    public event InventoryEvents OnDragEvent;
    public event InventoryEvents OnEndDragEvent;
    public event InventoryEvents OnDropEvent;

    public static InventorySystem Instance;
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        weaponSlots = new List<UIWeapon>();
        WeaponManager.Instance.OnWeaponsInit += WeaponsInitiated;
    }
    private void WeaponsInitiated()
    {
        //for (int i = 0; i < WeaponManager.Instance.Weapons.Count; i++)
        //{
        //    GameObject go = Instantiate(weaponPanel, transform);
        //    go.GetComponentInChildren<UnityEngine.UI.Image>().color = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255);
        //    weaponSlots.Add(go.GetComponentInChildren<UIWeapon>());
        //    weaponSlots[i].positionInPanel = i;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwapWeapons(UIWeapon wpIn)
    {
        UIWeapon aux = selectedWeaponSlot;
        //int auxSib = selectedWeaponSlot.transform.GetSiblingIndex();

        //selectedWeaponSlot.transform.parent.SetSiblingIndex(wpIn.transform.GetSiblingIndex());
        ////selectedWeaponSlot.transform.position = wpIn.transform.position;
        selectedWeaponSlot.positionInPanel = wpIn.positionInPanel;

        //wpIn.transform.parent.SetSiblingIndex(auxSib);
        ////wpIn.transform.position = aux.transform.position;
        wpIn.positionInPanel = aux.positionInPanel;

        WeaponManager.Instance.Weapons[wpIn.positionInPanel].GetComponent<PositionInBuild>().positionInBuild = selectedWeaponSlot.positionInPanel;
        WeaponManager.Instance.Weapons[selectedWeaponSlot.positionInPanel].GetComponent<PositionInBuild>().positionInBuild = wpIn.positionInPanel;
        WeaponManager.Instance.UpdateWeapons();
    }
}
