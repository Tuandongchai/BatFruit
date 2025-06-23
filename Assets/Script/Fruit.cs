using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum FruitType { Apple, Banana, Blueberry, grapes, orange, Pear, Strawberry, Missile_Hor, Missile_Ver, Bomb, Rubik, none};
public class Fruit : MonoBehaviour, IDestroy
{
    public FruitType type;
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject particleDestroy;

    public static Action<FruitType> broken;
    private bool isDestroyed = false;
    private void Start()
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

    
    public void DestroyThis()
    {
        if (isDestroyed) return;
        isDestroyed = true;
        if (gameObject == null)
            return;
        //gameObject?.GetComponent<FruitSpecial>()?.ActiveEffect(parent?.GetComponent<FruitCell>(), parent?.GetComponent<FruitCell>());
        gameObject?.GetComponent<FruitSpecial>()?.ActiveEffect();
        broken?.Invoke(type);
        if (particleDestroy && parent != null)
        {
            GameObject go = Instantiate(particleDestroy, transform.position, Quaternion.identity);
            /*go.transform.localPosition = Vector3.zero;*/
            go.transform.SetParent(null);
        }
        Destroy(gameObject,0.02f);
    }

    public FruitType GetFruitType()
    {
        return this.type;
    }
}
