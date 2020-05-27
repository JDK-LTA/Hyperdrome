using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//TODO: CLEAR CODE AND PREPARE FOR WAVE-ENDING STUFF
public class InventorySystem : MonoBehaviour
{
    [SerializeField] private List<GameObject> weaponSlots;
    public UIWeapon selectedWeaponSlot;
    public Transform selectedItem;

    public GameObject draggableItem;

    [SerializeField] private GameObject weaponPanelPrefab;
    [SerializeField] private GameObject pickPanel;
    [SerializeField] private GameObject buildPanel;

    public static InventorySystem Instance;
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //weaponSlots = new List<UIWeapon>();
        WeaponManager.Instance.OnWeaponsInit += WeaponsInitiated;
    }
    private void WeaponsInitiated()
    {
        CreateBuildPanels();
    }
    private void CreateBuildPanels()
    {
        for (int i = 0; i < WeaponManager.Instance.Weapons.Count; i++)
        {
            GameObject go = Instantiate(weaponPanelPrefab, transform);
            go.GetComponentInChildren<UnityEngine.UI.Image>().color = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255);

            weaponSlots.Add(go);

            go.GetComponentInChildren<UIWeapon>().positionInPanel = i;
        }
    }
    private void DestroyBuildPanels()
    {
        for (int i = weaponSlots.Count - 1; i >= 0; i--)
        {
            Destroy(weaponSlots[i]);
        }
    }
    private void CreatePickPanels()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwapWeapons(UIWeapon wpIn)
    {
        int aux = selectedWeaponSlot.positionInPanel;

        WeaponManager.Instance.Weapons[selectedWeaponSlot.positionInPanel].GetComponent<PositionInBuild>().positionInBuild = wpIn.positionInPanel;
        WeaponManager.Instance.Weapons[wpIn.positionInPanel].GetComponent<PositionInBuild>().positionInBuild = aux;

        selectedWeaponSlot.positionInPanel = wpIn.positionInPanel;
        wpIn.positionInPanel = aux;


        SortChildren();

        WeaponManager.Instance.UpdateWeapons();
    }

    private void SortChildren()
    {
        List<Transform> children = new List<Transform>();

        int aux = transform.childCount - 1;

        for (int i = aux; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
            children.Add(child);
            child.parent = null;
        }

        children.Sort((Transform t1, Transform t2) => { return t1.GetComponentInChildren<UIWeapon>().positionInPanel.CompareTo(t2.GetComponentInChildren<UIWeapon>().positionInPanel); });

        foreach (Transform child in children)
        {
            Debug.Log(child.GetComponentInChildren<UIWeapon>().positionInPanel);
            child.parent = transform;
        }
    }
}
