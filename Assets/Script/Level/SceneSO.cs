using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName ="SceneManager")]
public class SceneSO : ScriptableObject
{
    [SerializeField] int mainMenuBuildIndex = 0;
    [SerializeField] int playSceneBuildIndex = 1;

    public static Action onLevelFinished;

    internal static void levelFinished()
    {
        onLevelFinished?.Invoke();
    }

    public void GoToMainMenu()
    {
        LoadSceneByIndex(mainMenuBuildIndex);
    }
    public void LoadPlayScene()
    {
        LoadSceneByIndex(playSceneBuildIndex);
    }
    
    public void RestartCurrentLevel()
    {
        LoadSceneByIndex(SceneManager.GetActiveScene().buildIndex);

    }


    private void LoadSceneByIndex(int index)
    {
        SceneManager.LoadScene(index);

        /*GameManager.instance.SetGamePaused(false);*/
    }

}
