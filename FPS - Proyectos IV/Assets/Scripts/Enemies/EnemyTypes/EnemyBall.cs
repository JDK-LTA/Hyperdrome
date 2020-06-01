using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBall : EnemyBase
{

    private bool readyToExplode = false, isRunning = false;
    [SerializeField] private float distanceToRun = 30f;
    [SerializeField] private float distanceToExplode = 2f;
    [SerializeField] private float explosionRadius = 8f;
    [SerializeField] private float explosionForce = 4f;
    [SerializeField] protected float walkingSpeed = 3f;
    [SerializeField] protected float runningSpeed = 6f;

    public event EnemyEvents ReadyToExplode;

    // Start is called before the first frame update
    protected override void Start()
    {
        EnemyAnimBall anim = GetComponentInChildren<EnemyAnimBall>();
        anim.Explode += Explode;
        anim.StartRunningEvent += Run;
    }

    protected void Run()
    {
        agent.speed = runningSpeed;
        canMove = true;
    }

    protected void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in colliders)
        {
            Debug.DrawLine(transform.position, hit.transform.position, Color.black, 10f);
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                //Debug.Log("kaboom");
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, 6f);

                if (rb.gameObject == player.gameObject)
                {
                    float dmgDistance = distanceToExplode / Vector3.Distance(transform.position, player.gameObject.transform.position);

                    if (dmgDistance > 0.5f)
                    {
                        GameManager.Instance.PlayerTakeHit(damagePerAttack * dmgDistance);
                    }
                }
            }
        }

        Destroy(gameObject);
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (player != null)
        {
            if (Vector3.Distance(transform.position, player.gameObject.transform.position) > distanceToExplode)
            {
                RaycastHit hit;
                Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, distanceToRun);
                //Debug.DrawLine(transform.position, hit.point, Color.magenta, 5f);
                //Debug.Log(Vector3.Distance(transform.position, player.transform.position));
                //Debug.Log(hit.collider?.gameObject.name);
                if (hit.transform?.gameObject == player.gameObject && !isRunning)
                {
                    isRunning = true;
                    canMove = false;

                    OnReadyToRun();
                }


                //if (canMove)
                //{
                //    agent.isStopped = false;
                //}
                //else
                //{
                //    agent.isStopped = true;
                //}
                agent.isStopped = !canMove;
                agent.destination = player.transform.position;
            }
            else
            {
                if (!readyToExplode)
                {
                    readyToExplode = true;
                    canMove = false;
                    agent.isStopped = true;
                    ReadyToExplode?.Invoke();
                    Die();
                }
            }
        }
    }

    protected override void Die()
    {
        base.Die();
        Explode();
    }
}
