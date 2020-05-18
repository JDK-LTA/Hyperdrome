using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int nOfPiecesGot = 0;
    
    public void BringPieces()
    {
        WaveManager.Instance.PiecesToEndWave -= nOfPiecesGot;
        if (WaveManager.Instance.PiecesToEndWave <= 0)
        {
            WaveManager.Instance.PrepareToEndWave();
        }
        nOfPiecesGot = 0;
    }
}
