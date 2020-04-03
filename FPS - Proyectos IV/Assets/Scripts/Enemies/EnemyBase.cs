using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected RbFPSController player;

    [SerializeField] protected float walkingSpeed = 3f;
    [SerializeField] protected float runningSpeed = 6f;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<RbFPSController>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }
}
