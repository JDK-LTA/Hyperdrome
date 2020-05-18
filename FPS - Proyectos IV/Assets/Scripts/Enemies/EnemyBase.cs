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
    public float damagePerAttack = 10;

    protected float currentHp;
    protected bool isDead = false, canMove = true;

    [SerializeField] protected bool golden = false;

    public bool Golden { get => golden; set => golden = value; }
    public float CurrentHp { get => currentHp; set => currentHp = value; }

    public delegate void EnemyEvents();
    public event EnemyEvents ReadyToRun;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<RbFPSController>();
    }

    protected virtual void Start()
    {
        currentHp = maxHp;
    }

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

        if (golden)
        {
            Instantiate(EnemiesManager.Instance.goldenPiece, transform.position, transform.rotation);
        }
        WaveManager.Instance.AddDifficulty(difficulty, golden);
    }

    protected virtual void OnReadyToRun()
    {
        ReadyToRun?.Invoke();
    }
}
