using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PickablePanel : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private bool currentTrueNewFalse;
    [SerializeField] private GameObject weapon;
    private Image weaponImage;

    [SerializeField] protected bool showDeactivated;
    private int positionInBuild = -1;
    private bool picked = false;

    public bool ShowDeactivated { get => showDeactivated; set => showDeactivated = value; }
    public int PositionInBuild { get => positionInBuild; set => positionInBuild = value; }
    public bool Picked { get => picked; set => picked = value; }
    public GameObject Weapon { get => weapon; set => weapon = value; }
    public Image WeaponImage { get => weaponImage; set => weaponImage = value; }
    public bool CurrentTrueNewFalse { get => currentTrueNewFalse; set => currentTrueNewFalse = value; }

    protected void Start()
    {
        weaponImage = GetComponentInChildren<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!showDeactivated)
        {
            GeneralClickPanelStuff();
        }
    }

    private void GeneralClickPanelStuff()
    {
        if (!InventoryManager.Instance.AnyPanelClicked)
        {
            InventoryManager.Instance.PanelClicked = this;
            InventoryManager.Instance.AnyPanelClicked = true;
            ClickBehaviour();

            if (WeaponManager.Instance.Weapons.Count < WeaponManager.Instance.maxNumberOfWeapons && !currentTrueNewFalse)
            {
                InventoryManager.Instance.AddWeapon();
            }
        }
        else
        {
            if (InventoryManager.Instance.PanelClicked.CurrentTrueNewFalse)
            {
                InventoryManager.Instance.SwapBuildPanels();
            }
            else
            {
                InventoryManager.Instance.SwapPickAndBuildPanels();
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!showDeactivated)
        {
            InventoryManager.Instance.PanelHovering = this;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryManager.Instance.PanelHovering = null;
    }

    protected void ClickBehaviour()
    {
        picked = true;

        InventoryManager.Instance.DeactivateUnusablePanels(false);
    }

    public void PanelSetActive(bool active)
    {
        //VISUAL (DE)ACTIVATION STUFF
        Color tempColor = weaponImage.color;
        tempColor.a = active ? 1f : 0.5f;
        weaponImage.color = tempColor;

        showDeactivated = !active;
    }
}
