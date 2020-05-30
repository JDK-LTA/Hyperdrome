using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimator : MonoBehaviour
{
    Animator anim;
    public delegate void WeaponAnimEvents();
    public event WeaponAnimEvents LockFinished;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        WeaponManager.Instance.OnShoot += OnShoot;
    }

    private void OnShoot()
    {
        if (WeaponManager.Instance.Weapons[WeaponManager.Instance.selectedWeapon].GetComponent<WeaponBase>().ShootingType == ShootingType.LOCK)
        {
            anim.SetTrigger("Lock");
        }
        else
        {
            anim.ResetTrigger("Shoot");
            anim.SetTrigger("Shoot");
        }
    }
    private void OnEndLockAnim()
    {
        LockFinished?.Invoke();
    }
}
