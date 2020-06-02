using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    public static EnemiesManager Instance;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    public GameObject goldenPiece;

    private void Start()
    {
        WeaponManager.Instance.OnHit += EnemyHasBeenHit;
    }

    private void EnemyHasBeenHit(EnemyBase enemy, float damage, Vector3 hitPoint)
    {
        enemy.TakeHit(damage);
        enemy.PlayHitParticles(hitPoint);
        AudioSource.PlayClipAtPoint(GameManager.Instance.hitClip, hitPoint);
    }
}
