using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : EnemyBase
{
    bool readyToAttack = false;

    [SerializeField] private float distanceToAttack = 3f;

    public event EnemyEvents ReadyToAttack;
    public event EnemyEvents MeleeDie;

    private BoxCollider attackTrigger;

    protected override void Start()
    {
        base.Start();

        attackTrigger = GetComponentInChildren<BoxCollider>();
    }

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
                //Attack();
            }
        }
    }

    private void AttackEnded()
    {
        readyToAttack = false;
        canMove = true;
        agent.isStopped = false;

    }
    private void DeactivateAttackTrigger()
    {
        attackTrigger.gameObject.SetActive(false);
    }
    private void ActivateAttackTrigger()
    {
        attackTrigger.gameObject.SetActive(true);
    }
    protected override void Die()
    {
        base.Die();
        MeleeDie?.Invoke();
    }
}
