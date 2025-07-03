/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum FruitType { Apple, Banana, Blueberry, grapes, orange, Pear, Strawberry, Missile_Hor, Missile_Ver, Bomb, Rubik, none};
public class Fruit : MonoBehaviour, IDestroy
{
    public FruitType type;
    [SerializeField] protected GameObject parent;
    [SerializeField] protected GameObject particleDestroy;

    public static Action<FruitType> broken;
    protected bool isDestroyed = false;

    protected bool ac = false;
    protected virtual void Start()
    {
        SetParent(transform.parent);
    }
    public void SetParent(Transform parent)
    {
        this.parent = parent.gameObject;
    }
    public void ChangeParent(GameObject parent)
    {
        gameObject.transform.SetParent(parent.transform);
        gameObject.transform.localPosition = Vector3.zero;
    }

    //
    public virtual IEnumerator DestroyThis()
    {
        if (isDestroyed) yield break;
        isDestroyed = true;
        if (gameObject == null)
            yield break;
        //gameObject?.GetComponent<FruitSpecial>()?.ActiveEffect(parent?.GetComponent<FruitCell>(), parent?.GetComponent<FruitCell>());
        if (gameObject?.GetComponent<FruitSpecial>() != null)
            yield return StartCoroutine(gameObject?.GetComponent<FruitSpecial>()?.ActiveEffect());

        broken?.Invoke(type);
        if (particleDestroy && parent != null)
        {
            GameObject go = Instantiate(particleDestroy, transform.position, Quaternion.identity);
            *//*go.transform.localPosition = Vector3.zero;*//*
            go.transform.SetParent(null);
        }
        Destroy(gameObject,0.02f);
       // yield return StartCoroutine(CheckFruitSpecial(type));
        yield break;

    }
    public virtual IEnumerator DestroyThis(bool t, float timeDes)
    {
        if (isDestroyed) yield break;
        isDestroyed = true;
        if (gameObject == null)
            yield break;
        //gameObject?.GetComponent<FruitSpecial>()?.ActiveEffect(parent?.GetComponent<FruitCell>(), parent?.GetComponent<FruitCell>());
        if (gameObject?.GetComponent<Rubik>() != null)
            yield return StartCoroutine(gameObject?.GetComponent<FruitSpecial>()?.ActiveEffect());

        broken?.Invoke(type);
        if (particleDestroy && parent != null)
        {
            GameObject go = Instantiate(particleDestroy, transform.position, Quaternion.identity);
            *//*go.transform.localPosition = Vector3.zero;*//*
            go.transform.SetParent(null);
        }
        // yield return StartCoroutine(CheckFruitSpecial(type));
        yield return new WaitForSeconds(timeDes);
    }

    public virtual void DestroyThisGameObject()
    {

        if (this== null)
            return;
        Destroy(this.gameObject);
    }
    //
    public virtual void DestroyThis(int a)
    {
        if (isDestroyed) 
            return;
        isDestroyed = true;
        if (gameObject == null)
            return;
        //gameObject?.GetComponent<FruitSpecial>()?.ActiveEffect(parent?.GetComponent<FruitCell>(), parent?.GetComponent<FruitCell>());
        if (gameObject?.GetComponent<FruitSpecial>() != null)
            StartCoroutine(gameObject?.GetComponent<FruitSpecial>()?.ActiveEffect());

        broken?.Invoke(type);
        if (particleDestroy && parent != null)
        {
            GameObject go = Instantiate(particleDestroy, transform.position, Quaternion.identity);
            *//*go.transform.localPosition = Vector3.zero;*//*
            go.transform.SetParent(null);
        }
        Destroy(gameObject, 0.02f);
    }

    public FruitType GetFruitType()
    {
        return this.type;
    }
    
}
*/
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
        if (this.isDestroyed) return;
        this.isDestroyed = true;
        if (gameObject == null)
            return;
        //gameObject?.GetComponent<FruitSpecial>()?.ActiveEffect(parent?.GetComponent<FruitCell>(), parent?.GetComponent<FruitCell>());
        //
        if (attackPoint != null)
        {
            GameObject score;
            pool.Spawn(attackPoint.transform.position, out score);
            ScorePlusUI scoreUI = score.GetComponent<ScorePlusUI>();
            scoreUI.Init(attackPoint.transform);
            scoreUI.Effect();

        }
        //
        gameObject?.GetComponent<FruitSpecial>()?.ActiveEffect(a,b);

        broken?.Invoke(type);

        if (particleDestroy && parent != null)
        {
            GameObject go = Instantiate(particleDestroy, transform.position, Quaternion.identity);
            /*go.transform.localPosition = Vector3.zero;*/
            go.transform.SetParent(null);
           
        }
        Destroy(gameObject, i);
    }

    public FruitType GetFruitType()
    {
        return this.type;
    }
    public void Destroyit(float i)
    {
        Destroy(gameObject, i);

    }
}