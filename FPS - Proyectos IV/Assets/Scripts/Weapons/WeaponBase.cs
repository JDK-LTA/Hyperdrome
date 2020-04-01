﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    #region Weapon variables
    [SerializeField] protected string _nameOfWeapon;
    [SerializeField] protected ShootingType _shootingType;
    [SerializeField] protected WeaponType _weaponType;
    [SerializeField] protected Changer _changer;

    [SerializeField] protected GameObject _bullet;
    [SerializeField] protected AudioClip _fireSound;

    [SerializeField] protected int _levelRequired;

    [SerializeField] protected int _ammo;
    [SerializeField] protected float _weight;
    [SerializeField] protected float _damagePerHit;
    [SerializeField] protected float _forceToApply;
    [SerializeField] protected float _range;
    [SerializeField] protected float _cdBetweenShots;
    [SerializeField] protected float variance;
    [SerializeField] protected float varianceDecreaseWhenAim;

    [SerializeField] protected float _numberToChange;

    [SerializeField] protected Transform _raycastSpot;


    public string Name { get => _nameOfWeapon; set => _nameOfWeapon = value; }
    public ShootingType ShootingType { get => _shootingType; set => _shootingType = value; }
    public WeaponType WeaponType { get => _weaponType; set => _weaponType = value; }
    public Changer Changer { get => _changer; set => _changer = value; }
    public GameObject Bullet { get => _bullet; set => _bullet = value; }
    public AudioClip FireSound { get => _fireSound; set => _fireSound = value; }
    public int LevelRequired { get => _levelRequired; set => _levelRequired = value; }
    public int Ammo { get => _ammo; set => _ammo = value; }
    public float Weight { get => _weight; set => _weight = value; }
    public float DamagePerHit { get => _damagePerHit; set => _damagePerHit = value; }
    public float ForceToApply { get => _forceToApply; set => _forceToApply = value; }
    public float Range { get => _range; set => _range = value; }
    public float CdBetweenShots { get => _cdBetweenShots; set => _cdBetweenShots = value; }
    public float NumberToChange { get => _numberToChange; set => _numberToChange = value; }
    public Transform RaycastSpot { get => _raycastSpot; set => _raycastSpot = value; }
    public float Variance { get => variance; set => variance = value; }
    public float VarianceDecreaseWhenAim { get => varianceDecreaseWhenAim; set => varianceDecreaseWhenAim = value; }
    #endregion

    ShotBase shotComp;
    public ShotBase ShotComp { get => shotComp; set => shotComp = value; }

    protected AudioSource audioSource;
    protected bool canShoot = false;
    protected bool isAiming = false;

    private float auxTimer;

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        shotComp = GetComponent<ShotBase>();
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        RaycastSpot = transform.Find("Shooting spot");
    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }

    public void Shoot()
    {
        ShotsThatAreShot();

        audioSource.PlayOneShot(FireSound);
        shotComp.CanShoot = false;
    }

    protected virtual void ShotsThatAreShot()
    {
        ShootingBullet(GetVariedDirection());
    }
    protected void ShootingBullet(Vector3 direction)
    {
        Ray ray = new Ray(Camera.main.transform.position, direction);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Range))
        {

            Debug.Log("Hit" + hit.transform.name);
            if (hit.rigidbody)
            {
                //hit.rigidbody.AddForce(ray.direction * ForceToApply);
                Debug.Log("Hit");
            }
        }
        Debug.DrawLine(Camera.main.transform.position, hit.point, Color.green, 10f);

    }

    protected Vector3 GetVariedDirection()
    {
        Vector3 direction;
        if (!isAiming)
        {
            direction = Random.insideUnitCircle * variance;
        }
        else
        {
            direction = Random.insideUnitSphere * (variance / varianceDecreaseWhenAim);
        }

        direction.z = Range; // circle is at Z units 
        direction = Camera.main.transform.TransformDirection(direction.normalized);

        return direction;
    }
}
