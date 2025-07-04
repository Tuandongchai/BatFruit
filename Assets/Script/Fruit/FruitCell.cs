using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class FruitCell : MonoBehaviour
{
    public enum FruitState {Has ,None};
    [Header("Setup")]
    [SerializeField] Fruit[] fruitList;
    [SerializeField] private FruitState state;
    [SerializeField] private FruitType fruitType;
    [SerializeField] int x, y;

    [SerializeField] private Fruit fruit;
    [SerializeField] private GameObject fruitObject;

    public enum ObstacleState { None, Has };
    [Header("Setup Land")]
    [SerializeField] private ObstacleState oState;
    [SerializeField] Obstacle[] obstacleList;
    [SerializeField] ObstacleCellType obstacleType;
    [SerializeField] private GameObject obstacleObject =null;

    public enum ChainObstacleState { None, Has };
    [Header("Setup Land")]
    [SerializeField] private ChainObstacleState chainState;
    [SerializeField] Obstacle[] chainList;
    [SerializeField] ObstacleCellType chainObsType;
    [SerializeField] public GameObject chainObject = null;

    public void Init(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public Vector2 GetXY()
    {
        return new Vector2(x, y);
    }
    private void Start()
    {
        if(state == FruitState.None)
        {
            int index = UnityEngine.Random.Range(0, fruitList.Length);
            GameObject fruitIns = Instantiate(fruitList[index].gameObject, Vector3.zero, Quaternion.identity);
            fruitIns.transform.SetParent(transform);
            fruitIns.transform.localPosition = Vector3.zero;
            Configure(fruitIns.GetComponent<Fruit>());
            foreach (Transform go in gameObject.transform)
            {
                if(go.TryGetComponent(out Fruit fruit))
                {
                    fruitObject = go.gameObject; break;
                }
                
            }
            /*fruitObject = gameObject.transform.GetChild(0).gameObject;*/
            fruitObject.GetComponent<Fruit>().SetParent(gameObject.transform);
        }
        else
        {
            Fruit matchedFruit = fruitList.FirstOrDefault(f => f.type == fruitType);
            if (matchedFruit != null)
            {
                GameObject fruitIns = Instantiate(matchedFruit.gameObject, Vector3.zero, Quaternion.identity);
                fruitIns.transform.SetParent(transform);
                fruitIns.transform.localPosition = Vector3.zero;
                Configure(fruitIns.GetComponent<Fruit>());
                foreach (Transform go in gameObject.transform)
                {
                    if (go.TryGetComponent(out Fruit fruit))
                    {
                        fruitObject = go.gameObject; break;
                    }

                }
                //fruitObject = gameObject.transform.GetChild(0).gameObject;
                fruitObject.GetComponent<Fruit>().SetParent(gameObject.transform);
            }
        }
        if(oState == ObstacleState.Has)
        {
            Obstacle matchedObstacle = obstacleList.FirstOrDefault(f => f.type == obstacleType);
            {
                GameObject fruitIns = Instantiate(matchedObstacle.gameObject, Vector3.zero, Quaternion.identity);
                fruitIns.transform.SetParent(transform);
                fruitIns.transform.localPosition = Vector3.zero;
                foreach (Transform go in gameObject.transform)
                {
                    if (go.TryGetComponent(out LandObstacle obs ))
                    {
                        obstacleObject = go.gameObject; break;
                    }

                }
            }
        }
        if (chainState == ChainObstacleState.Has)
        {
            Obstacle matchedChain = chainList.FirstOrDefault(f => f.type == chainObsType);
            {
                GameObject fruitIns = Instantiate(matchedChain.gameObject, Vector3.zero, Quaternion.identity);
                fruitIns.transform.SetParent(transform);
                fruitIns.transform.localPosition = Vector3.zero;
                foreach (Transform go in gameObject.transform)
                {
                    if (go.TryGetComponent(out ChainObstacle obs))
                    {
                        chainObject = go.gameObject; break;
                    }

                }
            }
        }
    }
    public void Configure(Fruit fruit) => this.fruit = fruit;

    public GameObject GetFruit()
    {
        return fruitObject;
    }
    public GameObject GetLandObstacle()
    {
        return obstacleObject;
    }
    public GameObject GetChainObstacle()
    {
        return chainObject;
    }
    public FruitType GetFruitType()
    {
        var fruitObj = GetFruit();
        return fruitObj != null && fruitObj.GetComponent<Fruit>() != null
            ? fruitObj.GetComponent<Fruit>().GetFruitType()
            : FruitType.none;
    }
    public void ChangeFruit(GameObject fruit)
    {
        if (fruit == null)
        {
            this.fruitObject = null;
            this.fruit = null;
/*            this.fruitType = FruitType.none;*/
            return;
        }
        Configure(fruit.GetComponent<Fruit>());
        fruit.GetComponent<Fruit>().ChangeParent(this.gameObject);
        fruitObject = fruit;
    }
}
