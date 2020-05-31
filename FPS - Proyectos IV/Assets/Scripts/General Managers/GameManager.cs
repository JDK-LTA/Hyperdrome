using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GameManager : Singleton<GameManager>
{
    public int nOfPiecesGot = 0;
    [SerializeField] private float playerHp = 100;
    private float playerMaxHp;

    private bool regenerating = false;
    private bool needToRegen = false;
    [SerializeField] private float regenAmountPerSec = 10;
    [SerializeField] private float cdAfterHitToRegen = 4;
    private float cdAux = 0;

    PostProcessVolume ppvPlayer;

    public float PlayerHp { get => playerHp; }

    private void Start()
    {
        ppvPlayer = WeaponManager.Instance._player.GetComponent<PostProcessVolume>();
        playerMaxHp = playerHp;
    }

    public void BringPieces()
    {
        WaveManager.Instance.PiecesToEndWave -= nOfPiecesGot;
        UIManager.Instance.UpdatePiecesText(WaveManager.Instance.PiecesToEndWave);
        if (WaveManager.Instance.PiecesToEndWave <= 0)
        {
            WaveManager.Instance.PrepareToEndWave();
        }
        nOfPiecesGot = 0;
    }

    public void PlayerTakeHit(float dmg)
    {
        playerHp -= dmg;

        regenerating = false;
        needToRegen = true;
        cdAux = 0;

        if (playerHp <= 0)
        {
            PlayerDeath();
        }
        else
        {
            PostProCalc();
        }
    }

    private void PostProCalc()
    {
        float x = playerHp / playerMaxHp;
        float y = 0.025f + 1.95f * x - x * x;
        ppvPlayer.weight = 1 - y;
    }

    private void PlayerDeath()
    {
        Debug.Log("MUERTA");
        Time.timeScale = 0;
    }

    private void Update()
    {
        if (regenerating)
        {
            playerHp += regenAmountPerSec * Time.deltaTime;
            PostProCalc();

            if (playerHp >= playerMaxHp)
            {
                playerHp = playerMaxHp;
                regenerating = false;
                needToRegen = false;
            }
        }
        else
        {
            if (needToRegen)
            {
                cdAux += Time.deltaTime;
                if (cdAux >= cdAfterHitToRegen)
                {
                    cdAux = 0;
                    regenerating = true;
                }
            }
        }
    }
}
