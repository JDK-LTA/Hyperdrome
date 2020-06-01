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
    private GameObject tooltipPanel;
    private Text weaponNameText, weaponTooltipText;

    [SerializeField] protected bool showDeactivated;
    private int positionInBuild = -1;
    private bool picked = false;

    public bool ShowDeactivated { get => showDeactivated; set => showDeactivated = value; }
    public int PositionInBuild { get => positionInBuild; set => positionInBuild = value; }
    public bool Picked { get => picked; set => picked = value; }
    public GameObject Weapon { get => weapon; set => weapon = value; }
    public Image WeaponImage { get => weaponImage; set => weaponImage = value; }
    public bool CurrentTrueNewFalse { get => currentTrueNewFalse; set => currentTrueNewFalse = value; }

    protected void Awake()
    {
        weaponImage = transform.Find("WeaponImage").GetComponent<Image>();
        tooltipPanel = transform.Find("TooltipPanel").gameObject;
        weaponNameText = tooltipPanel.transform.Find("TooltipName").GetComponent<Text>();
        weaponTooltipText = tooltipPanel.transform.Find("TooltipText").GetComponent<Text>();
    }

    public void UpdateTooltip()
    {
        WeaponBase wb = weapon.GetComponent<WeaponBase>();

        string typeT, triggerT, changerT;

        switch (wb.WeaponType)
        {
            case WeaponType.NORMAL:
                typeT = "Bullets";
                break;
            case WeaponType.LASER:
                typeT = "Laser";
                break;
            case WeaponType.SHOTGUN:
                typeT = "Shotgun";
                break;
            default:
                typeT = "xxxxx";
                break;
        }
        switch (wb.ShootingType)
        {
            case ShootingType.LOCK:
                triggerT = "Lock";
                break;
            case ShootingType.SEMI_AUTOMATIC:
                triggerT = "Semi";
                break;
            case ShootingType.AUTOMATIC:
                triggerT = "Auto";
                break;
            case ShootingType.HOLD:
                triggerT = "Hold";
                break;
            default:
                triggerT = "xxxx";
                break;
        }
        switch (wb.Changer)
        {
            case Changer.AMMO:
                changerT = "Ammo";
                break;
            case Changer.TIME:
                changerT = "Time";
                break;
            case Changer.HIT:
                changerT = "Hits";
                break;
            default:
                changerT = "xxxx";
                break;
        }

        float accuracy = 100 - 10 * wb.Variance;
        string dmgT = wb.WeaponType == WeaponType.SHOTGUN ? wb.DamagePerHit.ToString() + "×" + wb.GetComponent<WeaponShotgun>().NOfBulletsPerShot : wb.DamagePerHit.ToString();
        string freqT = wb.ShootingType == ShootingType.LOCK ? "0.75" : wb.CdBetweenShots.ToString();

        weaponNameText.text = wb.Name + ":";

        weaponTooltipText.text = "Type: " + typeT + "\n"
            + "Trigger: " + triggerT + "\n"
            + "Changer: " + changerT + "\n"
            + changerT + ": " + wb.NumberToChange + "\n"
            + "Damage: " + dmgT + "\n"
            + "Range: " + wb.Range + "\n"
            + "Freq. : " + freqT + "secs" + "\n"
            + "Accuracy: " + accuracy;
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
            tooltipPanel.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryManager.Instance.PanelHovering = null;
        tooltipPanel.SetActive(false);
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
