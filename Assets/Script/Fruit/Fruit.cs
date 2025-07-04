
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public enum FruitType { Apple, Banana, Blueberry, grapes, orange, Pear, Strawberry, Missile_Hor, Missile_Ver, Bomb, Rubik, none };
public class Fruit : MonoBehaviour
{
    public FruitType type;
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject particleDestroy;
    [SerializeField] private ScoreObjectPooling pool;
    [SerializeField] private GameObject attackPoint;


    public static Action<FruitType> broken;
    public bool isDestroyed = false;
    private void Start()
    {
        if (transform.parent == null)
        {
            Destroy(gameObject);
            return;
        }
        pool = FindObjectOfType<ScoreObjectPooling>();
        SetParent(transform.parent);
    }
    public void SetParent(Transform parent)
    {
        if (parent == null)
        {
            Destroy(gameObject);
        }
        this.parent = parent.gameObject;
    }
    public void ChangeParent(GameObject parent)
    {
        gameObject.transform.SetParent(parent.transform);
        gameObject.transform.localPosition = Vector3.zero;
    }


    public void DestroyThis(float i=0.05f, FruitCell a =null, FruitCell b=null)
    {
        if(CkeckChainObstacle()) return;
        if (this.isDestroyed) return;
        this.isDestroyed = true;
        if (gameObject == null)
            return;
        //gameObject?.GetComponent<FruitSpecial>()?.ActiveEffect(parent?.GetComponent<FruitCell>(), parent?.GetComponent<FruitCell>());
        //
        SpawnScoreText();
        //
        gameObject?.GetComponent<FruitSpecial>()?.ActiveEffect(a, b);
        broken?.Invoke(type);

        if (particleDestroy && parent != null)
        {
            GameObject go = Instantiate(particleDestroy, transform.position, Quaternion.identity);
            /*go.transform.localPosition = Vector3.zero;*/
            go.transform.SetParent(null);

        }
        //
        CkeckLandObstacle();
        //

        Destroy(gameObject, i);
    }

    private void CkeckLandObstacle()
    {
        FruitCell cell = gameObject.transform.parent.GetComponent<FruitCell>();
        GameObject obstacle = cell?.GetLandObstacle();

        if (obstacle != null)
        {
            Obstacle obs = obstacle.GetComponent<LandObstacle>();
            obs?.DestroyThis();
        }
    }
    private bool CkeckChainObstacle()
    {
        FruitCell cell = gameObject.transform.parent.GetComponent<FruitCell>();
        GameObject obstacle = cell?.GetChainObstacle();

        if (obstacle != null)
        {
            cell.ChangeFruit(gameObject);
            Obstacle obs = obstacle.GetComponent<ChainObstacle>();
            obs?.DestroyThis();
            return true;
        }
        return false;
    }

    private void SpawnScoreText()
    {
        if (attackPoint != null)
        {
            GameObject score;
            pool.Spawn(attackPoint.transform.position, out score);
            ScorePlusUI scoreUI = score.GetComponent<ScorePlusUI>();
            scoreUI.Init(attackPoint.transform);
            scoreUI.Effect();

        }
    }

    public FruitType GetFruitType()
    {
        return this.type;
    }
    public void Destroyit(float i)
    {
        CkeckLandObstacle();
        Destroy(gameObject, i);

    }
}