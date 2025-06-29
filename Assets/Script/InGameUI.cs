using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{

    [Header("Elements")]
    [SerializeField] LevelSO[] requires = new LevelSO[] { };
    [SerializeField] Dictionary<FruitType,GameObject> goalsUI = new Dictionary<FruitType, GameObject> { };

    [Header("Setup")]
    [SerializeField] private TextMeshProUGUI stepText, scoreText;
    [SerializeField] private Transform goalPos;
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject fruitGoalPrefab;
    [SerializeField] private GameObject plusPrefab;
    [SerializeField] private GameObject star1, star2,star3;

    private void OnEnable()
    {
        FruitController.swap += UpdateStepUI;
        Fruit.broken += UpdateScoreUI;
        LevelManager.reduceGoals += UpdateGoalsUI;

    }


    private void OnDisable()
    {
        FruitController.swap -= UpdateStepUI;
        Fruit.broken -= UpdateScoreUI;
        LevelManager.reduceGoals -= UpdateGoalsUI;

    }
    private void Start()
    {
        InitRequirement();
    }
    private void InitRequirement()
    {
        int currentLevel = StatsManager.Instance.GetLevelCurrent();
        this.requires = LevelManager.instance.GetLevelInfo();
        Debug.Log("currentlevel: " + currentLevel);
        stepText.text = requires[currentLevel-1].step.ToString();
        SetupGoal(currentLevel);

    }
    private void SetupGoal(int cl)
    {
        int length = requires[cl - 1].items.Count;
        for (int i = 0; i < length; i++)
        {
            GameObject go = Instantiate(fruitGoalPrefab, Vector2.zero, Quaternion.identity, goalPos);
            go.GetComponentInChildren<Image>().sprite = requires[cl - 1].items[i].item;
            go.GetComponentInChildren<TextMeshProUGUI>().text = requires[cl - 1].items[i].amount.ToString();

            goalsUI.Add(requires[cl - 1].items[i].type, go);

            if (i == length - 1) return;
            Instantiate(plusPrefab, Vector2.zero, Quaternion.identity, goalPos);
        }

    }
    private void UpdateStepUI()=>stepText.text = LevelManager.instance.GetCurrentStep().ToString();
    private void UpdateGoalsUI()
    {
        Dictionary<FruitType, int> goals = new Dictionary<FruitType, int>();
        goals = LevelManager.instance.GetGoalUpdate();

        foreach (GoalItem item in requires[StatsManager.Instance.GetLevelCurrent()-1].items)
        {
            goalsUI[item.type].GetComponentInChildren<TextMeshProUGUI>().text = goals[item.type].ToString();
        }
    }
    private void UpdateScoreUI(FruitType type)
    {
        scoreText.text = LevelManager.instance.GetScore().ToString();
        float percentSlider = (float)LevelManager.instance.GetScore() / 1000f;
        slider.value = percentSlider;

        if (percentSlider >= 0.8)
        {
            star3.SetActive(true);

        }
        else if (percentSlider >= 0.55)
        {
            star2.SetActive(true) ;
        }
        else if (percentSlider >= 0.2)
        {
            star1.SetActive(true);
        }
    }
    
}
