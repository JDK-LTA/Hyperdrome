using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DmgLake : MonoBehaviour
{
    bool inWater = false;
    private void Update()
    {
        if (inWater)
        {
            GameManager.Instance.PlayerTakeHit(15 * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            inWater = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            inWater = false;
        }
    }
}
