using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotSemi : ShotBase
{
    protected override void Update()
    {
        if (!canShoot)
        {
            if (cdBetweenShots > 0)
            {
                auxTimer += Time.deltaTime;
                if (auxTimer >= cdBetweenShots)
                {
                    IfNotShootingCanShoot();
                }
            }
            else
            {
                IfNotShootingCanShoot();
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

    private void IfNotShootingCanShoot()
    {
        if (!shooting)
        {
            canShoot = true;
        }
    }
}
