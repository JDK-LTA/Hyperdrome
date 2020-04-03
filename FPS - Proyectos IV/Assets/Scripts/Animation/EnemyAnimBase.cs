using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimBase : MonoBehaviour
{
    Animator _animator;
    NavMeshAgent _agent;

    public float interpolationSpeed = 3;
    float animatorSpeed;

    public delegate void BallAnimations();
    public event BallAnimations StartRolling;
    public event BallAnimations Explode;

    private bool isPreparingToRoll = false, isExploding = false;

    // Start is called before the first frame update
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _agent = transform.parent.GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        transform.parent.GetComponent<EnemyBall>().ReadyToRun += TriggerRunning;
        transform.parent.GetComponent<EnemyBall>().ReadyToExplode += TriggerExploding;
    }

    private void Update()
    {
        float agentSpeed = _agent.velocity.magnitude;
        animatorSpeed = Mathf.MoveTowards(animatorSpeed, agentSpeed, interpolationSpeed * Time.deltaTime);
        _animator.SetFloat("Speed", animatorSpeed);
    }

    private void TriggerRunning()
    {
        _animator.SetTrigger("PrepareToRoll");
        isPreparingToRoll = true;
    }
    private void TriggerExploding()
    {
        //cmpAnimator?.SetTrigger("Explode");
        isExploding = true;
    }

    public void StartRunning()
    {
        StartRolling();
    }
}
