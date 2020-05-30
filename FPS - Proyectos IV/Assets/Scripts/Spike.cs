using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spike : MonoBehaviour
{
    private void SpikeEnd()
    {
        GetComponentInParent<SpikeParent>().DisableSpike();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.Instance.PlayerTakeHit(GetComponentInParent<SpikeParent>().damage);
        }
    }
}
