using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasDDOL : MonoBehaviour
{
    public static CanvasDDOL Instance;

    [SerializeField] private GameObject endPanel, pauseMenu;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    public void SetEndPanelActive(bool value, bool win)
    {
        endPanel.SetActive(value);

        Text endText = endPanel.GetComponentInChildren<Text>();
        if (win)
        {
            endText.text = "YOU WIN!";
        }
        else
        {
            endText.text = "YOU DIED";
        }
    }
    public void SetPauseMenuActive(bool value)
    {
        pauseMenu.SetActive(value);
    }
}
