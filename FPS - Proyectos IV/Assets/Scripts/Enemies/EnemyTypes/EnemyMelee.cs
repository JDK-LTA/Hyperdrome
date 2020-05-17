using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : EnemyBase
{
    bool readyToAttack = false;

    [SerializeField] private float distanceToAttack = 3f;

    public event EnemyEvents ReadyToAttack;
    public event EnemyEvents MeleeDie;

    protected override void Update()
    {
        base.Update();

        if (Vector3.Distance(transform.position, player.transform.position) > distanceToAttack)
        {

            agent.isStopped = !canMove;
            agent.destination = player.transform.position;
        }
        else
        {
            if (!readyToAttack)
            {
                readyToAttack = true;
                canMove = false;
                agent.isStopped = true;
                ReadyToAttack?.Invoke();
                Attack();
            }
        }
    }

    private void AttackEnded()
    {
        readyToAttack = false;
        canMove = true;
        agent.isStopped = false;
    }

    private void Attack()
    {

    }
    protected override void Die()
    {
        base.Die();
        MeleeDie?.Invoke();
    }
}
