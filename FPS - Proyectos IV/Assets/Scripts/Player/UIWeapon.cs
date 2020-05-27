using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIWeapon : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    Vector2 originalPosition;

    public int positionInPanel = 0;

    private GameObject draggable;
    private Image draggableImage;

    private Image thisImage;

    private bool picked = false;

    private void Start()
    {
        draggable = InventorySystem.Instance.draggableItem;
        draggableImage = draggable.GetComponent<Image>();
        thisImage = GetComponent<Image>();
    }

    #region testing
    private void OnMouseDown()
    {
        originalPosition =  transform.position;
        //Debug.Log("a");
    }
    private void OnMouseDrag()
    {
        transform.position = Input.mousePosition;
    }
    private void OnMouseUp()
    {
        if (InventorySystem.Instance.selectedWeaponSlot == null || InventorySystem.Instance.selectedWeaponSlot == this)
        {
            transform.position = originalPosition;
        }
        else
        {
            InventorySystem.Instance.SwapWeapons(this);
        }
    }
    private void OnMouseEnter()
    {
        InventorySystem.Instance.selectedWeaponSlot = this;
    }
    private void OnMouseExit()
    {
        InventorySystem.Instance.selectedWeaponSlot = null;
    }
    #endregion

    public void OnPointerDown(PointerEventData eventData)
    {
        originalPosition = transform.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        draggableImage.sprite = thisImage.sprite;
        draggable.transform.position = Input.mousePosition;
        draggableImage.enabled = true;

        thisImage.enabled = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //transform.position = Input.mousePosition;
        draggable.transform.position = Input.mousePosition;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        draggableImage.enabled = false;
        thisImage.enabled = true;

        if (InventorySystem.Instance.selectedWeaponSlot == null || InventorySystem.Instance.selectedWeaponSlot == this)
        {
            //transform.localPosition = Vector3.zero;
        }
        else
        {
            InventorySystem.Instance.SwapWeapons(this);
            //transform.localPosition = Vector3.zero;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        //if (InventorySystem.Instance.selectedWeaponSlot == null || InventorySystem.Instance.selectedWeaponSlot == this)
        //{
        //    //transform.localPosition = Vector3.zero;
        //}
        //else
        //{
        //    Debug.Log("a");
        //    InventorySystem.Instance.SwapWeapons(this);
        //    //transform.localPosition = Vector3.zero;
        //}
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        InventorySystem.Instance.selectedWeaponSlot = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InventorySystem.Instance.selectedWeaponSlot = null;
    }
}
