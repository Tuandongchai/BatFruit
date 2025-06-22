using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour, IGameStateListener
{
    public static InGameUIManager Instance { get; private set; }

    public static Action<bool> pause;

    [Header("Panels")]
    [SerializeField] private GameObject gamePanel,pausePanel, winPanel, losePanel;
    private GameObject[] panels;

    [SerializeField] private Button pauseBtn, pauseCloseBtn;
    [SerializeField] private SceneSO sceneManager;
    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance);

        panels = new GameObject[]
        {
            gamePanel,
            winPanel,
            losePanel,
            pausePanel
        };

        pauseBtn.onClick.AddListener(()=>GameManager.instance.SetGameState(GameState.Pause));
        pauseCloseBtn.onClick.AddListener(()=>GameManager.instance.SetGameState(GameState.Play));


/*        AudioManager.instance.BGSoundOn(2);*/
    }

    private void Show(GameObject panel)
    {
        for (int i = 0; i < panels.Length; i++)
            panels[i].SetActive(panels[i] == panel);
    }

    public void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Play:
                pause?.Invoke(false);
                Show(gamePanel);
                /*GameManager.instance.SetGamePaused(true);*/
                break;
            case GameState.Win:
                pause?.Invoke(true);
                Show(winPanel);
                /*GameManager.instance.SetGamePaused(true);*/
                break;
            case GameState.Lose:
                pause?.Invoke(true);
                Show(losePanel);
                /*GameManager.instance.SetGamePaused(true);*/
                break;
            case GameState.Pause:
                pause?.Invoke(true);
                Show(pausePanel);
                /*GameManager.instance.SetGamePaused(true);*/

                break;
            default:
                break;
        }
    }
    public void GoToMenu()
    {
        sceneManager.GoToMainMenu();
    }
    public void RePlay()
    {
        
        sceneManager.RestartCurrentLevel();
    }

}
