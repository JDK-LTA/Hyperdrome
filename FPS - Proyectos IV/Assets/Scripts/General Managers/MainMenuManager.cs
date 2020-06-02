using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public void PlayButton()
    {
        SceneChangeManager.Instance.LoadLevel(1);
    }
    public void ExitButton()
    {
        SceneChangeManager.Instance.ExitGame();
    }
}
