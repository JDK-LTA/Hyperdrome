using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : EnemyBase
{
    [SerializeField] private float distanceToShoot = 8f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float cdBetweenShots = 1.5f;
    [SerializeField] private Transform shootingSpot;
    [SerializeField] private float bulletSpeed = 4f;

    float timer;

    protected override void Start()
    {
        base.Start();

        Vector3 offsetPos = transform.position;
        offsetPos.y += 5;
        agent.Warp(offsetPos);

        timer = cdBetweenShots;
    }

    protected override void Update()
    {
        base.Update();

        transform.LookAt(player.transform);
        shootingSpot.LookAt(player.transform);

        if (Vector3.Distance(transform.position, player.transform.position) > distanceToShoot)
        {
            agent.isStopped = false;
            agent.destination = player.transform.position;
        }

        else
        {
            agent.isStopped = true;
            timer += Time.deltaTime;
            if (timer >= cdBetweenShots)
            {
                timer = 0;
                Shoot();
            }
        }

    }

    private void Shoot()
    {
        GameObject go = Instantiate(bulletPrefab, shootingSpot.position, shootingSpot.rotation);
        EnemyBullet eb = go.GetComponent<EnemyBullet>();
        eb.Speed = bulletSpeed;
        eb.Dmg = damagePerAttack;
    }

    protected override void Die()
    {
        base.Die();

        Destroy(gameObject);
    }
}
