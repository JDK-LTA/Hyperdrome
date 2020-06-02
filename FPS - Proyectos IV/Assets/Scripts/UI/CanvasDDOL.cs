using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasDDOL : MonoBehaviour
{
    public static CanvasDDOL Instance;

    [SerializeField] private GameObject endPanel, pauseMenu;
    [SerializeField] private Text pressFText;
    [SerializeField] private Image endImage;
    [SerializeField] private Sprite youWinSprite, youLoseSprite;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    public void SetEndPanelActive(bool value, bool win)
    {
        endPanel.SetActive(value);
        if (win)
        {
            endImage.sprite = youWinSprite;
        }
        else
        {
            endImage.sprite = youLoseSprite;
        }
    }
    public void SetPauseMenuActive(bool value)
    {
        pauseMenu.SetActive(value);
    }
    public void SetPressFTextActive(bool value)
    {
        pressFText.text = "Press F to deliver " + GameManager.Instance.nOfPiecesGot + " pieces";
        pressFText.gameObject.SetActive(value);
    }

    public void Resume()
    {
        GameManager.Instance.TogglePauseMenu();
    }
    public void ExitToMenu()
    {
        SceneChangeManager.Instance.LoadMenu();
    }
}
