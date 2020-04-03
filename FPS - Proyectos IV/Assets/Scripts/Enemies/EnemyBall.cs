using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBall : EnemyBase
{
    private bool readyToExplode = false, isRunning = false, canMove = true;
    [SerializeField] private float distanceToRun = 30f;
    [SerializeField] private float distanceToExplode = 2f;

    public delegate void OnBall();
    public event OnBall ReadyToRun;
    public event OnBall ReadyToExplode;
    // Start is called before the first frame update
    protected override void Start()
    {
        
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
                    agent.destination = player.transform.position;
                }
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
