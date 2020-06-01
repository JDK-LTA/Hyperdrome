using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] GameObject hitParticle;
    [SerializeField] GameObject deathParticles;

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
    public void PlayHitParticles(Vector3 pos)
    {
        hitParticle.transform.position = pos;

        hitParticle.transform.LookAt(WeaponManager.Instance._player.gameObject.transform);
        GameObject ps = Instantiate(hitParticle, pos, new Quaternion(0, 0, 0, 0));
        ps.transform.LookAt(WeaponManager.Instance._player.gameObject.transform);
    }

    protected virtual void Die()
    {
        if (golden)
        {
            Instantiate(EnemiesManager.Instance.goldenPiece, transform.position, transform.rotation);
        }
        WaveManager.Instance.AddDifficulty(difficulty, golden);

        GameObject ps = Instantiate(deathParticles, transform.position, transform.rotation);
    }

    protected virtual void OnReadyToRun()
    {
        ReadyToRun?.Invoke();
    }
}
