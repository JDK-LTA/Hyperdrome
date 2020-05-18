using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenPiece : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.Instance.nOfPiecesGot++;
            Destroy(gameObject);
        }
    }
}
