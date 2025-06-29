using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum GameState
{
    Play,
    Pause,
    Win,
    Lose
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState gameState;


/*    public GameUIAnimation gameUIAnimation;*/
    private void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        gameState = GameState.Play;

    }
    /*public void SetGamePaused(bool paused)
    {
        Time.timeScale = paused ? 0 : 1;
        Debug.Log("Co pause khong: " + Time.timeScale);
    }*/
    public void SetGameState(GameState gameState)
    {
        this.gameState = gameState;

        IEnumerable<IGameStateListener> gameStateListeners =
            FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .OfType<IGameStateListener>();

        foreach (IGameStateListener gameStateListener in gameStateListeners)
            gameStateListener.GameStateChangedCallback(gameState);
        Debug.Log("New Game State: " + gameState);
    }
}
public interface IGameStateListener
{
    void GameStateChangedCallback(GameState gameState);
}
