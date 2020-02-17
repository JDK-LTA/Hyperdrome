using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponInfo
{
    [SerializeField] private WeaponType weaponType = WeaponType.PISTOL;
    [SerializeField] private Changer changer = Changer.AMMO;
    [SerializeField] private string name;

    [SerializeField] private Mesh mesh;
    [SerializeField] private GameObject bullet;

    [SerializeField] private int levelRequired;

    [SerializeField] private int ammo;
    [SerializeField] private float weight;
    [SerializeField] private float damagePerHit;

    [SerializeField] private float ammoToChange;
    [SerializeField] private float timeToChange;
    [SerializeField] private float hitsToChange;

    public WeaponType WeaponType { get => weaponType; set => weaponType = value; }
    public Changer Changer { get => changer; set => changer = value; }
    public string Name { get => name; set => name = value; }
    public Mesh Mesh { get => mesh; set => mesh = value; }
    public GameObject Bullet { get => bullet; set => bullet = value; }
    public int LevelRequired { get => levelRequired; set => levelRequired = value; }
    public int Ammo { get => ammo; set => ammo = value; }
    public float Weight { get => weight; set => weight = value; }
    public float DamagePerHit { get => damagePerHit; set => damagePerHit = value; }
    public float AmmoToChange { get => ammoToChange; set => ammoToChange = value; }
    public float TimeToChange { get => timeToChange; set => timeToChange = value; }
    public float HitsToChange { get => hitsToChange; set => hitsToChange = value; }
}

public enum WeaponType
{
    PISTOL,
    AUTOMATIC,
    SEMI_AUTOMATIC,
    SHOTGUN,
    ROCKET_LAUNCHER
}
public enum Changer
{
    AMMO,
    TIME,
    HIT
}