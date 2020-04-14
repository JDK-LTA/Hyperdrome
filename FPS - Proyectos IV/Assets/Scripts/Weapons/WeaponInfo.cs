using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponInfo
{
    [SerializeField] private string previousName = "";
    public string PreviousName { get => previousName; set => previousName = value; }
    [SerializeField] private bool isCreated = false;
    public bool IsCreated { get => isCreated; set => isCreated = value; }

    #region Base properties
    [SerializeField] private string name;
    [SerializeField] private ShootingType shootingType = ShootingType.LOCK;
    [SerializeField] private WeaponType weaponType = WeaponType.NORMAL;
    [SerializeField] private Changer changer = Changer.AMMO;

    [SerializeField] private Mesh mesh;
    [SerializeField] private Material[] materials;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Sprite crosshairTexture;
    [SerializeField] private AudioClip fireSound;

    [SerializeField] private int levelRequired;

    [SerializeField] private int ammo;
    [SerializeField] private float weight;
    [SerializeField] private float damagePerHit;
    [SerializeField] private float forceToApply;
    [SerializeField] private float range;
    [SerializeField] private float cdBetweenShots;
    [SerializeField] private float variance;
    [SerializeField] private float varianceDecreaseWhenAim;
    [SerializeField] private float speedDecreaseWhenAim;

    [SerializeField] private float numberToChange;

    [SerializeField] private Transform raycastSpot;

    public string Name { get => name; set => name = value; }
    public ShootingType ShootingType { get => shootingType; set => shootingType = value; }
    public WeaponType WeaponType { get => weaponType; set => weaponType = value; }
    public Changer Changer { get => changer; set => changer = value; }
    public Mesh Mesh { get => mesh; set => mesh = value; }
    public Material[] Materials { get => materials; set => materials = value; }
    public GameObject Bullet { get => bullet; set => bullet = value; }
    public Sprite CrosshairTexture { get => crosshairTexture; set => crosshairTexture = value; }
    public AudioClip FireSound { get => fireSound; set => fireSound = value; }
    public int LevelRequired { get => levelRequired; set => levelRequired = value; }
    public int Ammo { get => ammo; set => ammo = value; }
    public float Weight { get => weight; set => weight = value; }
    public float DamagePerHit { get => damagePerHit; set => damagePerHit = value; }
    public float ForceToApply { get => forceToApply; set => forceToApply = value; }
    public float Range { get => range; set => range = value; }
    public float CdBetweenShots { get => cdBetweenShots; set => cdBetweenShots = value; }
    public float Variance { get => variance; set => variance = value; }
    public float VarianceDecreaseWhenAim { get => varianceDecreaseWhenAim; set => varianceDecreaseWhenAim = value; }
    public float SpeedDecreaseWhenAim { get => speedDecreaseWhenAim; set => speedDecreaseWhenAim = value; }
    /// <summary>
    /// It's seconds to change, ammo to change or hits/kills to change, depending on the changer
    /// </summary>
    public float NumberToChange { get => numberToChange; set => numberToChange = value; }
    public Transform RaycastSpot { get => raycastSpot; set => raycastSpot = value; }
    #endregion

    [SerializeField] private int nOfBulletsPerShot;
    public int NOfBulletsPerShot { get => nOfBulletsPerShot; set => nOfBulletsPerShot = value; }

    [SerializeField] private float endSpeed = 10f;
    [SerializeField] private float lineStartWidth = 0.05f;
    [SerializeField] private float lineEndWidth = 0.05f;
    public float EndSpeed { get => endSpeed; set => endSpeed = value; }
    public float LineStartWidth { get => lineStartWidth; set => lineStartWidth = value; }
    public float LineEndWidth { get => lineEndWidth; set => lineEndWidth = value; }
}

public enum ShootingType
{
    LOCK,
    SEMI_AUTOMATIC,
    AUTOMATIC,
    HOLD
}
public enum WeaponType
{
    NORMAL,
    LASER,
    SHOTGUN,
    ROCKET_LAUNCHER
}
public enum Changer
{
    AMMO,
    TIME,
    HIT
}