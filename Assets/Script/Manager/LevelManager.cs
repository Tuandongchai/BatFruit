using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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

    [SerializeField] private Dictionary<ObstacleCellType, int> goalsObs = new Dictionary<ObstacleCellType, int>();
    [SerializeField] private Dictionary<ObstacleCellType, int> goalsObsUpdate = new Dictionary<ObstacleCellType, int>();

    [SerializeField] private List<FruitType> fruitTypes = new List<FruitType>();
    [SerializeField] private List<ObstacleCellType> obsTypes = new List<ObstacleCellType>();
    [SerializeField] private int requireCount;
    [SerializeField] private int score;
    private bool ac=false;
    private void OnEnable()
    {
        FruitController.swap += ReduceStep;
        Fruit.broken += UpdateCore;
        Fruit.broken += UpdateGoal;
        Fruit.broken += CheckComplete;

        Obstacle.obstacleBroken += UpdateGoalObs;
        Obstacle.obstacleBroken += CheckCompleteObs;
    }


    private void OnDisable()
    {
        FruitController.swap -= ReduceStep;
        Fruit.broken -= UpdateCore;
        Fruit.broken += UpdateGoal;
        Fruit.broken -= CheckComplete;

        Obstacle.obstacleBroken -= UpdateGoalObs;
        Obstacle.obstacleBroken -= CheckCompleteObs;
    }
    private void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        InitMap();
        goalsUpdate = goals;
        goalsObsUpdate = goalsObs;
        requireCount = requires[StatsManager.Instance.GetLevelCurrent() - 1].items.Count + requires[StatsManager.Instance.GetLevelCurrent() - 1].obstacles.Count;

        InitPameters(StatsManager.Instance.GetLevelCurrent());
        Invoke("HandelMatch", 0.1f);
    }
    private void HandelMatch()
    {
        FruitController.instance.HandleMatches();

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

        foreach (GoaObstacle item in requires[cl - 1].obstacles)
        {
            goalsObs[item.obstacleType] = item.amount;
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
        if (step <= 0)
            GameManager.instance.SetGameState(GameState.Lose);
    }
    private void UpdateGoal(FruitType type)
    {
        foreach (GoalItem item in requires[StatsManager.Instance.GetLevelCurrent() - 1].items)
        {
            if (item.type == type)
            {
                goalsUpdate[type] = goalsUpdate[type]-1<=0? 0:goalsUpdate[type]-1;
                Debug.Log(type +" co so luong: "+ goalsUpdate[type]);
                reduceGoals?.Invoke();
            }
            
        }

        
    }
    private void UpdateGoalObs(ObstacleCellType type)
    {
        foreach (GoaObstacle item in requires[StatsManager.Instance.GetLevelCurrent() - 1].obstacles)
        {
            if (item.obstacleType == type)
            {
                goalsObsUpdate[type] = goalsObsUpdate[type] - 1<=0?0:goalsObsUpdate[type]-1;
                Debug.Log(type + " co so luong: " + goalsObsUpdate[type]);
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
    public Dictionary<ObstacleCellType, int> GetGoalObsUpdate() => goalsObsUpdate;
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
    private void CheckCompleteObs(ObstacleCellType type)
    {

        foreach (GoalItem item in requires[StatsManager.Instance.GetLevelCurrent() - 1].items)
        {
            bool result = goalsObsUpdate.TryGetValue(type, out int value);
            if (result && value <= 0 && !obsTypes.Contains(type))
            {
                requireCount -= 1;
                obsTypes.Add(type);
            }
        }
        Debug.Log("count: " + requireCount);
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

        List<FruitCell> cells = he.GetComponent<LateGameEffect>().GetCellsRandomFromBoard(step-2);

        yield return StartCoroutine(he.GetComponent<LateGameEffect>().Active(cells));


        

        yield return new WaitForSeconds(60f);
        //GameManager.instance.SetGameState(GameState.Lose);

    }
    private void UpdateCore(FruitType type) => score += 5;
    public int GetScore() => score;
}
