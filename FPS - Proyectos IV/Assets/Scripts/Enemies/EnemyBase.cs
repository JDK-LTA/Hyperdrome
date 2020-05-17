using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] protected int difficulty = 1;
    public int Difficulty { get => difficulty; }

    protected NavMeshAgent agent;
    protected RbFPSController player;

    [SerializeField] protected float maxHp = 100;
    protected float currentHp;
    protected bool isDead = false, canMove = true;

    [SerializeField] protected float walkingSpeed = 3f;
    [SerializeField] protected float runningSpeed = 6f;

    public float CurrentHp { get => currentHp; set => currentHp = value; }

    public delegate void EnemyEvents();
    public event EnemyEvents ReadyToRun;

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
        //Debug.Log(gameObject.name + ": " + currentHp);
        //Debug.Log(isDead);
        if (currentHp <= 0 && !isDead)
        {
            isDead = true;
            Die();
        }
    }

    protected virtual void Die()
    {
        WaveManager.Instance.AddDifficulty(difficulty);
    }

    protected virtual void OnReadyToRun()
    {
        ReadyToRun?.Invoke();
    }
}
