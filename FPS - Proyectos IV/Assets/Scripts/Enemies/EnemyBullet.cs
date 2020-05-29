using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private float speed = 4f;
    private float dmg = 10f;
    public float Speed { get => speed; set => speed = value; }
    public float Dmg { get => dmg; set => dmg = value; }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.Instance.PlayerTakeHit(dmg);
        }
        if (!other.GetComponent<EnemyShooter>())
        {
            Destroy(gameObject);
        }
    }
}
