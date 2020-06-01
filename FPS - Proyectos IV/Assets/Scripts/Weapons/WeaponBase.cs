using System.Collections;
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
    [SerializeField] protected float speedDecreaseWhenAim;

    [SerializeField] protected float _numberToChange;

    [SerializeField] protected Transform _raycastSpot;


    public string Name { get => _nameOfWeapon; set => _nameOfWeapon = value; }
    public ShootingType ShootingType { get => _shootingType; set => _shootingType = value; }
    public WeaponType WeaponType { get => _weaponType; set => _weaponType = value; }
    public Changer Changer { get => _changer; set => _changer = value; }
    public float NumberToChange { get => _numberToChange; set => _numberToChange = value; }
    public GameObject Bullet { get => _bullet; set => _bullet = value; }
    public AudioClip FireSound { get => _fireSound; set => _fireSound = value; }
    public int LevelRequired { get => _levelRequired; set => _levelRequired = value; }
    public int Ammo { get => _ammo; set => _ammo = value; }
    public float Weight { get => _weight; set => _weight = value; }
    public float DamagePerHit { get => _damagePerHit; set => _damagePerHit = value; }
    public float ForceToApply { get => _forceToApply; set => _forceToApply = value; }
    public float Range { get => _range; set => _range = value; }
    public float CdBetweenShots { get => _cdBetweenShots; set => _cdBetweenShots = value; }
    public Transform RaycastSpot { get => _raycastSpot; set => _raycastSpot = value; }
    public float Variance { get => variance; set => variance = value; }
    public float VarianceDecreaseWhenAim { get => varianceDecreaseWhenAim; set => varianceDecreaseWhenAim = value; }
    public float SpeedDecreaseWhenAim { get => speedDecreaseWhenAim; set => speedDecreaseWhenAim = value; }
    #endregion

    ShotBase shotComp;
    [SerializeField] private bool isAiming = false;
    public ShotBase ShotComp { get => shotComp; set => shotComp = value; }
    public bool IsAiming { get => isAiming; set => isAiming = value; }

    protected AudioSource audioSource;
    protected bool canShoot = false, firstShot = false;

    private float auxTimer = 0;
    public float auxNoToChange;

    private ParticleSystem muzzle;

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        shotComp = GetComponent<ShotBase>();
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        _raycastSpot = transform.Find("Shooting spot");
        muzzle = _raycastSpot.GetComponent<ParticleSystem>();

        auxNoToChange = _numberToChange;

        InputManager.Instance.OnHoldAim += Aiming;
    }

    public void ResetWeapon()
    {
        auxNoToChange = _numberToChange;
        firstShot = false;
    }

    protected virtual void Aiming(bool aux)
    {
        isAiming = aux;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (_changer == Changer.TIME && firstShot)
        {
            auxTimer += Time.deltaTime;
            UIManager.Instance.UpdateNoToChangeText(_numberToChange - auxTimer, _numberToChange, _changer);
            if (auxTimer >= _numberToChange)
            {
                auxTimer = 0;

                ResetWeapon();
                WeaponManager.Instance.NextWeapon();
            }
        }
    }

    public void Shoot()
    {
        if (!firstShot)
        {
            firstShot = true;
        }
        ShotsThatAreShot();

        audioSource.PlayOneShot(FireSound);
        if (_weaponType != WeaponType.LASER)
        {
            muzzle.Play();
        }
        shotComp.CanShoot = false;
        WeaponManager.Instance.TriggerShootEvent();


        if (_changer == Changer.AMMO)
        {
            auxNoToChange--;
            UIManager.Instance.UpdateNoToChangeText(auxNoToChange, _numberToChange, _changer);
            if (auxNoToChange == 0)
            {
                ResetWeapon();
                WeaponManager.Instance.NextWeapon();
            }
        }
    }

    protected virtual void ShotsThatAreShot()
    {
        ShootingBullet(GetVariedDirection());
    }
    protected virtual void ShootingBullet(Vector3 direction)
    {
        Ray ray = new Ray(Camera.main.transform.position, direction);
        RaycastHit hit;
        //EnemyBase enemyHit;

        if (Physics.Raycast(ray, out hit, Range))
        {
            EnemyHitBehaviour(ref hit, ray);
        }
        Debug.DrawLine(Camera.main.transform.position, hit.point, Color.green, 10f);

    }

    protected void EnemyHitBehaviour(ref RaycastHit hit, Ray ray)
    {
        EnemyBase enemyHit = hit.transform.GetComponent<EnemyBase>();

        if (enemyHit != null)
        {
            if (_changer == Changer.HIT)
            {
                auxNoToChange--;
                UIManager.Instance.UpdateNoToChangeText(auxNoToChange, _numberToChange, _changer);
                if (auxNoToChange == 0)
                {
                    ResetWeapon();
                    WeaponManager.Instance.NextWeapon();
                }
            }

            hit.rigidbody?.AddForce(ray.direction * ForceToApply);
            WeaponManager.Instance.EnemyHit(enemyHit, _damagePerHit);

        }
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
            direction = Random.insideUnitCircle * (variance / varianceDecreaseWhenAim);
        }

        direction.z = Range; // circle is at Z units 
        direction = Camera.main.transform.TransformDirection(direction.normalized);

        return direction;
    }

    public virtual void StopDrawingLaser() { }
}
