using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] SceneSO sceneSO;

    [Header("Button")]
    [SerializeField] Button startGameBtn, optionBtn, quiteBtn;

    [Header("Panel")]
    [SerializeField] GameObject optionPanel;
    private void Start()
    {
        startGameBtn.onClick.AddListener(()=>sceneSO.LoadPlayScene());
        optionBtn.onClick.AddListener(()=>ShowOptionPanel());
    }
    private void ShowOptionPanel()
    {
        optionPanel.SetActive(true);
    }

}
