using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int nOfPiecesGot = 0;
    [SerializeField] private float playerHp = 100;
    
    public float PlayerHp { get => playerHp; }

    public void BringPieces()
    {
        WaveManager.Instance.PiecesToEndWave -= nOfPiecesGot;
        if (WaveManager.Instance.PiecesToEndWave <= 0)
        {
            WaveManager.Instance.PrepareToEndWave();
        }
        nOfPiecesGot = 0;
    }

    public void PlayerTakeHit(float dmg)
    {
        playerHp -= dmg;
        //Debug.Log("Hit. HP: " + playerHp);

        if (playerHp <= 0)
        {
            PlayerDeath();
        }
    }

    private void PlayerDeath()
    {
        Debug.Log("MUERTA");
        Time.timeScale = 0;
    }

}
