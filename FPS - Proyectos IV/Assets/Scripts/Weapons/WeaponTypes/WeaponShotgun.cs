using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShotgun : WeaponBase
{
    [SerializeField] private int nOfBulletsPerShot = 8;

    public int NOfBulletsPerShot { get => nOfBulletsPerShot; set => nOfBulletsPerShot = value; }

    
    protected override void ShotsThatAreShot()
    {
        //AudioSource.PlayClipAtPoint(GameManager.Instance.shotgunClip, _raycastSpot.position);
        audioSource.Play();
        for (int i = 0; i < nOfBulletsPerShot; i++)
        {            
            ShootingBullet(GetVariedDirection());
        }
    }
}
