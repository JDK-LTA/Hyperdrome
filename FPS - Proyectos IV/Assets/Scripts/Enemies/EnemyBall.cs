﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBall : EnemyBase
{
    private bool readyToExplode = false, isRunning = false, canMove = true;
    [SerializeField] private float distanceToRun = 30f;
    [SerializeField] private float distanceToExplode = 2f;
    [SerializeField] private float explosionRadius = 8f;
    [SerializeField] private float explosionForce = 4f;

    public delegate void OnBall();
    public event OnBall ReadyToRun;
    public event OnBall ReadyToExplode;
    // Start is called before the first frame update
    protected override void Start()
    {
        EnemyAnimBase anim = GetComponentInChildren<EnemyAnimBase>();
        anim.Explode += Explode;
        anim.StartRolling += Run;
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
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, 3f);
            }
        }

        Destroy(gameObject, 1f);
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (player != null)
        {
            if (Vector3.Distance(transform.position, player.transform.position) > distanceToExplode)
            {
                RaycastHit hit;
                Physics.Raycast(transform.position, player.transform.position, out hit, distanceToRun);
                if (hit.collider == player.GetComponent<Collider>() && !isRunning)
                {
                    isRunning = true;
                    canMove = false;
                    ReadyToRun();
                }


                if (canMove)
                {
                    agent.isStopped = false;
                }
                else
                {
                    agent.isStopped = true;
                }

                agent.destination = player.transform.position;
            }
            else
            {
                if (!readyToExplode)
                {
                    readyToExplode = true;
                    canMove = false;
                    agent.isStopped = true;
                    ReadyToExplode();
                }
                //ANIMACION DE EXPLOTAR
            }
        }
    }
}
