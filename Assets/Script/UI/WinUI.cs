using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private SceneSO sceneSO;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Button nextBtn, restartBtn;

    private void Start()
    {
        scoreText.text = LevelManager.instance.GetScore().ToString();
        nextBtn.onClick.AddListener(()=>NextLevel());
        restartBtn.onClick.AddListener(()=>RestartLevel());
    }
    private void RestartLevel()
    {
        sceneSO.RestartCurrentLevel();
    }
    private void NextLevel()
    {
        sceneSO.LoadPlayScene();
    }
}
