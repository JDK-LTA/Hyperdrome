using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotAuto : ShotBase
{
    protected override void Update()
    {
        if (!canShoot)
        {
            auxTimer += Time.deltaTime;
            if (auxTimer >= cdBetweenShots)
            {
                canShoot = true;
            }
        }
        else
        {
            if (shooting)
            {
                weapon.Shoot();
                canShoot = false;
            }
        }
    }

}
