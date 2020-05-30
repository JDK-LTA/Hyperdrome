using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeParent : MonoBehaviour
{
    [SerializeField] private GameObject actualSpike;
    [SerializeField] private float timerToSpike = 1;
    public float damage = 25;

    float t = 0;
    bool spikeTriggered = false;
    private void Update()
    {
        if (spikeTriggered)
        {
            t += Time.deltaTime;
            if (t>=timerToSpike)
            {
                t = 0;
                actualSpike.SetActive(true);
                spikeTriggered = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            spikeTriggered = true;
            GetComponent<Collider>().enabled = false;
        }
    }

    public void DisableSpike()
    {
        GetComponent<Collider>().enabled = true;
        actualSpike.SetActive(false);
    }
}
