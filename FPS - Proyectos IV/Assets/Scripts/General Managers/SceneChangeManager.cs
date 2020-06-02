using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChangeManager : Singleton<SceneChangeManager>
{
    [SerializeField] GameObject loadingPanel;
    [SerializeField] Image loadingBar;

    private void Start()
    {
        SceneManager.activeSceneChanged += OnLoadFirstLvl;
    }

    public void LoadLevel(int i)
    {
        StartCoroutine(LoadAsync(i));
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void SetLoadingPanelActive(bool active)
    {
        loadingPanel?.SetActive(active);
    }
    public void SetLoadingBarFillAmount(float value)
    {
        loadingBar.fillAmount = value;
    }

    IEnumerator LoadAsync(int i)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync("Nivel" + i);

        SetLoadingPanelActive(true);

        while (!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / 0.9f);
            SetLoadingBarFillAmount(progress);

            yield return null;
        }
    }

    private void OnLoadFirstLvl(Scene current, Scene next)
    {
        if (next.name == "Nivel1")
        {
            GameManager.Instance.Init();
            InputManager.Instance.Init();
            WeaponPrefabsLists.Instance.Init();
            WaveManager.Instance.Init();
            WeaponManager.Instance._player.Init();
        }
        else if (next.name == "MainMenu")
        {
            OnLoadMenu();
        }

        SetLoadingPanelActive(false);
    }
    private void OnLoadMenu()
    {
        Destroy(FindObjectOfType<WaveManager>().gameObject);
        Destroy(FindObjectOfType<WeaponPrefabsLists>().gameObject);
        //Destroy(FindObjectOfType<InputManager>().gameObject);
        InputManager.Instance.ResetInit();
        GameManager.Instance.ResetInit();

        WeaponManager.Instance._player.Unsubscribe();
        //Destroy(FindObjectOfType<GameManager>().gameObject);

        Destroy(FindObjectOfType<WeaponManager>().gameObject);
        Destroy(FindObjectOfType<EnemiesManager>().gameObject);
        Destroy(FindObjectOfType<CanvasDDOL>().gameObject);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
