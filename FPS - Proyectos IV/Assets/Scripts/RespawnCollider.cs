using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.Instance.PlayerTakeHit(35);
            WeaponManager.Instance.Respawn();
        }
    }
}
