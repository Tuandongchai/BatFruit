using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject lateGameEffectPrefab;
    public static LevelManager instance;

    public static Action reduceGoals;
    public static Action reduceStep;

    [Header("Elements")]
    [SerializeField] Transform boardPos;
    [SerializeField] GameObject[] maps = new GameObject[] { };
    [SerializeField] LevelSO[] requires = new LevelSO[] { };


    [Header("Parameters")]
    [SerializeField] private int step;
    [SerializeField] private Dictionary<FruitType, int> goals = new Dictionary<FruitType, int>();
    [SerializeField] private Dictionary<FruitType, int> goalsUpdate = new Dictionary<FruitType, int>();
    [SerializeField] private List<FruitType> fruitTypes = new List<FruitType>();
    [SerializeField] private int requireCount;
    [SerializeField] private int score;
    private bool ac=false;
    private void OnEnable()
    {
        FruitController.swap += ReduceStep;
        Fruit.broken += UpdateCore;
        Fruit.broken += UpdateGoal;
        Fruit.broken += CheckComplete;
    }


    private void OnDisable()
    {
        FruitController.swap -= ReduceStep;
        Fruit.broken -= UpdateCore;
        Fruit.broken += UpdateGoal;
        Fruit.broken -= CheckComplete;
    }
    private void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        InitMap();
        goalsUpdate = goals;
        requireCount = requires[StatsManager.Instance.GetLevelCurrent() - 1].items.Count;

        InitPameters(StatsManager.Instance.GetLevelCurrent());
    }

    private void InitMap()
    {
        int currentLevel = StatsManager.Instance.GetLevelCurrent();
        GameObject map = Instantiate(maps[currentLevel - 1], Vector2.zero, Quaternion.identity, boardPos);
        map.transform.localPosition = Vector3.zero;
    }
    
    private void InitPameters(int cl)
    {
        step = requires[cl - 1].step;
        foreach (GoalItem item in requires[cl-1].items)
        {
            goals[item.type]=item.amount;
        }

        /*foreach (GoalItem item in requires[cl - 1].items)
        {
            Debug.Log(goals[item.type]);
        }*/
    }

    public void ReduceStep()
    {
        step--;
        reduceStep?.Invoke();
        if (step < 0)
            GameManager.instance.SetGameState(GameState.Lose);
    }
    private void UpdateGoal(FruitType type)
    {
        foreach (GoalItem item in requires[StatsManager.Instance.GetLevelCurrent() - 1].items)
        {
            if (item.type == type)
            {
                goalsUpdate[type] = goalsUpdate[type]-1;
                Debug.Log(type +" co so luong: "+ goalsUpdate[type]);
                reduceGoals?.Invoke();
            }
            
        }
    }
    public LevelSO[] GetLevelInfo()
    {
        return requires;
    }
    public int GetCurrentStep() => step;
    public Dictionary<FruitType, int> GetGoalUpdate() => goalsUpdate;
    private void CheckComplete(FruitType type)
    {
        
        foreach (GoalItem item in requires[StatsManager.Instance.GetLevelCurrent() - 1].items)
        {
            bool result = goalsUpdate.TryGetValue(type, out int value);
            if (result && value<=0 && !fruitTypes.Contains(type))
            {
                requireCount -= 1;
                fruitTypes.Add(type);
            }
        }
        Debug.Log("count: "+ requireCount);
        if (requireCount <= 0 && !ac)
           StartCoroutine(WaitToShowCompleteUI());
    }

    IEnumerator WaitToShowCompleteUI()
    {
        ac =true;

        /*while (FruitController.instance.isMatching)
        {
            yield return null;
        }*/
        //yield return StartCoroutine(FruitController.instance.WaitToFallAndSpawn());
        //FruitController.instance.HandleMatches();

        GameObject he = Instantiate(lateGameEffectPrefab, this.gameObject.transform.position, Quaternion.identity);
        //he.transform.SetParent(GameObject.Find("HandleEffectPos").transform);

        List<FruitCell> cells = he.GetComponent<LateGameEffect>().GetCellsRandomFromBoard(step);

        yield return StartCoroutine(he.GetComponent<LateGameEffect>().Active(cells));


        

        yield return new WaitForSeconds(60f);
        //GameManager.instance.SetGameState(GameState.Lose);

    }
    private void UpdateCore(FruitType type) => score += 5;
    public int GetScore() => score;
}
