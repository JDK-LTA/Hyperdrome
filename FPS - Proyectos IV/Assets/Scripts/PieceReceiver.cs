using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceReceiver : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.Instance.CanDeliver(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.Instance.CanDeliver(false);
        }
    }
}
