using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    protected int difficulty = 1;
    public int Difficulty { get => difficulty; }

    protected NavMeshAgent agent;
    protected RbFPSController player;

    [SerializeField] protected float maxHp = 100;
    protected float currentHp;
    protected bool isDead = false;

    [SerializeField] protected float walkingSpeed = 3f;
    [SerializeField] protected float runningSpeed = 6f;

    public float CurrentHp { get => currentHp; set => currentHp = value; }

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

    public virtual void TakeHit(float damage)
    {
        currentHp -= damage;
        if (currentHp <= 0 && !isDead)
        {
            isDead = true;
            Die();
        }
    }

    protected virtual void Die()
    {

    }
}
