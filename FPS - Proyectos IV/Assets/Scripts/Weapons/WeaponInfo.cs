﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponInfo
{
    [SerializeField] private ShootingType shootingType = ShootingType.LOCK;
    [SerializeField] private WeaponType weaponType = WeaponType.PISTOL;
    [SerializeField] private Changer changer = Changer.AMMO;
    [SerializeField] private string name;

    [SerializeField] private Mesh mesh;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Texture2D crosshairTexture;
    [SerializeField] private AudioClip fireSound;

    [SerializeField] private int levelRequired;

    [SerializeField] private int ammo;
    [SerializeField] private float weight;
    [SerializeField] private float damagePerHit;
    [SerializeField] private float forceToApply;
    [SerializeField] private float range;
    [SerializeField] private float cdBetweenShots;

    [SerializeField] private float numberToChange;

    public ShootingType ShootingType { get => shootingType; set => shootingType = value; }
    public WeaponType WeaponType { get => weaponType; set => weaponType = value; }
    public Changer Changer { get => changer; set => changer = value; }
    public string Name { get => name; set => name = value; }
    public Mesh Mesh { get => mesh; set => mesh = value; }
    public GameObject Bullet { get => bullet; set => bullet = value; }
    public Texture2D CrosshairTexture { get => crosshairTexture; set => crosshairTexture = value; }
    public AudioClip FireSound { get => fireSound; set => fireSound = value; }
    public int LevelRequired { get => levelRequired; set => levelRequired = value; }
    public int Ammo { get => ammo; set => ammo = value; }
    public float Weight { get => weight; set => weight = value; }
    public float DamagePerHit { get => damagePerHit; set => damagePerHit = value; }
    public float ForceToApply { get => forceToApply; set => forceToApply = value; }
    public float Range { get => range; set => range = value; }
    /// <summary>
    /// It's seconds to change, ammo to change or hits/kills to change, depending on the changer
    /// </summary>
    public float NumberToChange { get => numberToChange; set => numberToChange = value; }
    public float CdBetweenShots { get => cdBetweenShots; set => cdBetweenShots = value; }
}

public enum ShootingType
{
    LOCK,
    SEMI_AUTOMATIC,
    AUTOMATIC
}
public enum WeaponType
{
    PISTOL,
    MACHINE_GUN,
    SHOTGUN,
    ROCKET_LAUNCHER
}
public enum Changer
{
    AMMO,
    TIME,
    HIT
}