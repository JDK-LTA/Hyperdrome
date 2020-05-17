using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimMelee : EnemyAnimBase
{
    protected override void Start()
    {
        base.Start();
        GetComponent<EnemyMelee>().ReadyToAttack += TriggerAttack;
        GetComponent<EnemyMelee>().MeleeDie += TriggerDeath;
    }

    private void TriggerAttack()
    {
        _animator.SetTrigger("Attack");
    }
    private void TriggerDeath()
    {
        _animator.SetTrigger("Die");
    }
    private void DestroyAfterAnim()
    {
        Destroy(gameObject);
    }
}
