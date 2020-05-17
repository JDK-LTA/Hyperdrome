using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIWeapon : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    Vector2 originalPosition;
    [HideInInspector] public int positionInPanel = 0;

    private void OnMouseDown()
    {
        originalPosition = transform.position;
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

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("clicked");

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = transform.position;
        //Debug.Log(originalPosition);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnDrop(PointerEventData eventData)
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
}
