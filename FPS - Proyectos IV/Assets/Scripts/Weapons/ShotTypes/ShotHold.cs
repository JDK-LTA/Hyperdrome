using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotHold : ShotBase
{
    private float auxTwo = 0.05f;
    protected override void Start()
    {
        base.Start();
        auxTimer = auxTwo;
    }
    protected override void Update()
    {
        if (shooting)
        {
            auxTimer -= Time.deltaTime;
            if (auxTimer < 0)
            {
                auxTimer = auxTwo;
                weapon.Shoot();
            }
        }
    }
}
