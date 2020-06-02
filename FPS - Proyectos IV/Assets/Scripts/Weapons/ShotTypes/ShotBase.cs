using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotBase : MonoBehaviour
{
    protected float cdBetweenShots;
    protected bool canShoot = true;
    protected WeaponBase weapon;

    protected bool shooting = false;
    protected float auxTimer = 0;

    public bool CanShoot { get => canShoot; set => canShoot = value; }
    public float CdBetweenShots { get => cdBetweenShots; set => cdBetweenShots = value; }
    public WeaponBase Weapon { get => weapon; set => weapon = value; }

    protected virtual void Awake()
    {
        weapon = GetComponent<WeaponBase>();
    }
    protected virtual void Start()
    {
        InputManager.Instance.OnHoldShoot += IsShooting;
        cdBetweenShots = weapon.CdBetweenShots;
    }
    protected virtual void Update() { }

    public virtual void IsShooting(bool hold)
    {
        shooting = hold;
        if (!hold)
        {
            weapon.StopDrawingLaser();
        }
    }
}
