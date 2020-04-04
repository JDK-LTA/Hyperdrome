using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    public static EnemiesManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        WeaponManager.Instance.OnHit += EnemyHasBeenHit;
    }

    private void EnemyHasBeenHit(EnemyBase enemy, float damage)
    {
        enemy.TakeHit(damage);
    }
}
