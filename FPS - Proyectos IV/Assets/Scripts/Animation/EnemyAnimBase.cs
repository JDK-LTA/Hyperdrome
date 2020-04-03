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

    
    // Start is called before the first frame update
    void Awake()
    {
        cmpAnimator = this.GetComponent<Animator>();
        cmpAgent = this.GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        float agentSpeed = cmpAgent.velocity.magnitude;
        animatorSpeed = Mathf.MoveTowards(animatorSpeed, agentSpeed, interpolationSpeed * Time.deltaTime);
        cmpAnimator.SetFloat("Speed", animatorSpeed);
    }
}
