using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimShooter : EnemyAnimBase
{
    protected override void Start()
    {
        base.Start();

        transform.parent.parent.GetComponent<EnemyShooter>().OnShoot += TriggerShoot;
    }
    private void TriggerShoot()
    {
        _animator.SetTrigger("Attack");
    }
}
