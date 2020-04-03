using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimBase : MonoBehaviour
{
    Animator cmpAnimator;
    NavMeshAgent cmpAgent;

    public float interpolationSpeed = 3;
    float animatorSpeed;

    public delegate void BallAnimations();
    public event BallAnimations StartRolling;
    public event BallAnimations Explode;

    private bool isPreparingToRoll = false, isExploding = false;

    // Start is called before the first frame update
    void Awake()
    {
        cmpAnimator = this.GetComponent<Animator>();
        cmpAgent = this.GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        GetComponent<EnemyBall>().ReadyToRun += TriggerRunning;
        GetComponent<EnemyBall>().ReadyToExplode += TriggerExploding;
    }

    private void Update()
    {
        float agentSpeed = cmpAgent.velocity.magnitude;
        animatorSpeed = Mathf.MoveTowards(animatorSpeed, agentSpeed, interpolationSpeed * Time.deltaTime);
        cmpAnimator.SetFloat("Speed", animatorSpeed);

        if (isPreparingToRoll)
        {

        }
    }

    private void TriggerRunning()
    {
        cmpAnimator?.SetTrigger("PrepareToRoll");
        isPreparingToRoll = true;
    }
    private void TriggerExploding()
    {
        //cmpAnimator?.SetTrigger("Explode");
        isExploding = true;
    }
}
