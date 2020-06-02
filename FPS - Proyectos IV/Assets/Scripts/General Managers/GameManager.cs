using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GameManager : Singleton<GameManager>
{
    public int nOfPiecesGot = 0;
    [HideInInspector] public bool canDeliverPieces = false;

    [SerializeField] private float playerHp = 100;
    private float playerMaxHp;

    private bool regenerating = false;
    private bool needToRegen = false;
    [SerializeField] private float regenAmountPerSec = 10;
    [SerializeField] private float cdAfterHitToRegen = 4;
    private float cdAux = 0;

    public AudioClip pieceObtainedClip, hitClip, ballDeathClip, spiderDeathClip, droneDeathClip, droneShootClip, shotgunClip, shotClip;

    PostProcessVolume ppvPlayer;

    private bool gamePaused = false, initiated = false;

    public float PlayerHp { get => playerHp; }

    private void Start()
    {
        playerMaxHp = playerHp;
        InputManager.Instance.OnTriggerEscape += TogglePauseMenu;
    }
    public void Init()
    {
        ppvPlayer = WeaponManager.Instance._player.GetComponent<PostProcessVolume>();
        initiated = true;
        Time.timeScale = 1;
    }
    public void ResetInit()
    {
        ppvPlayer = null;
        initiated = false;
    }

    public void CanDeliver(bool value)
    {
        canDeliverPieces = value;
        CanvasDDOL.Instance.SetPressFTextActive(value);

        if (value)
        {
            InputManager.Instance.OnTriggerDeliver += BringPieces;
        }
        else
        {
            InputManager.Instance.OnTriggerDeliver -= BringPieces;
        }
    }
    public void BringPieces()
    {
        WaveManager.Instance.PiecesToEndWave -= nOfPiecesGot;
        UIManager.Instance.UpdatePiecesText(WaveManager.Instance.PiecesToEndWave);
        if (WaveManager.Instance.PiecesToEndWave <= 0)
        {
            CanvasDDOL.Instance.pressFText.text = "Kill remaining enemies";
            WaveManager.Instance.PrepareToEndWave();
        }
        else
        {
            CanvasDDOL.Instance.pressFText.text = "Press F to deliver 0 pieces";
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
            EndGame(false);
        }
        else
        {
            PostProCalc();
        }
    }

    private void PostProCalc()
    {
        float x = playerHp / playerMaxHp;
        float y = 0.025f - 0.05f * x + x * x;
        ppvPlayer.weight = 1 - y;
    }

    public void EndGame(bool win)
    {
        WeaponManager.Instance._player.CanMove(false);
        CanvasDDOL.Instance.SetEndPanelActive(true, win);

        Time.timeScale = 0;
    }
    public void TogglePauseMenu()
    {
        if (initiated)
        {
            gamePaused = !gamePaused;

            WeaponManager.Instance._player.CanMove(!gamePaused);
            CanvasDDOL.Instance.SetPauseMenuActive(gamePaused);

            Time.timeScale = gamePaused ? 0 : 1;
        }
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
