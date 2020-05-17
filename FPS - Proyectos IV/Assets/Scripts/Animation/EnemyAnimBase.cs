using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimBase : MonoBehaviour
{
    protected Animator _animator;
    protected NavMeshAgent _agent;

    [SerializeField] protected float interpolationSpeed = 3;
    protected float animatorSpeed;

    public delegate void EnemyAnimEvents();

    protected bool isPreparingToRun = false;

    // Start is called before the first frame update
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        if (_agent == null)
        {
            _agent = transform.parent.GetComponent<NavMeshAgent>();
        }
    }

    protected virtual void Start()
    {
    }

    private void Update()
    {
        float agentSpeed = _agent.velocity.magnitude;
        animatorSpeed = Mathf.MoveTowards(animatorSpeed, agentSpeed, interpolationSpeed * Time.deltaTime);
        _animator.SetFloat("Speed", animatorSpeed);
    }


}
