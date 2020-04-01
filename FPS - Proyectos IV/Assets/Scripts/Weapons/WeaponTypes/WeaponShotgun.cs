using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShotgun : WeaponBase
{
    //TENGO QUE HACER ESTO EN LA TOOL
    [SerializeField] private int nOfBulletsPerShot = 8;

    public int NOfBulletsPerShot { get => nOfBulletsPerShot; set => nOfBulletsPerShot = value; }

    protected override void ShotsThatAreShot()
    {
        for (int i = 0; i < nOfBulletsPerShot; i++)
        {
            Vector3 direction = Random.insideUnitCircle * variance;
            direction.z = Range; // circle is at Z units 
            direction = Camera.main.transform.TransformDirection(direction.normalized);
            
            ShootingBullet(direction);
        }
    }
}
