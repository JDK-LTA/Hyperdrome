using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotLock : ShotSemi
{
    // FALTA EL TEMA DE LA ANIMACIÓN. NO SÉ SI HACER COINCIDIR EL TIEMPO
    //DE ANIMACIÓN A MANO CON EL TIEMPO DE CD O INTENTAR PROGRAMARLO
    //EN CONJUNTO
    protected override void Start()
    {
        base.Start();
        transform.GetComponentInParent<WeaponAnimator>().LockFinished += IfNotShootingCanShoot;
    }

    protected override void Update()
    {
        if (canShoot)
        {
            if (shooting)
            {
                weapon.Shoot();
                canShoot = false;
            }
        }
    }

    protected override void IfNotShootingCanShoot()
    {
        canShoot = true;
    }
}
