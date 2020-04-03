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
            ShootingBullet(GetVariedDirection());
        }
    }
}
