using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum FruitType { Apple, Banana, Blueberry, grapes, orange, Pear, Strawberry};
public class Fruit : MonoBehaviour
{
    [SerializeField] public FruitType type;
    [SerializeField] private GameObject parent;

    public void SetParent(Transform parent)
    {
        this.parent = parent.gameObject;
    }
    public void ChangeParent(GameObject parent)
    {
        gameObject.transform.SetParent(parent.transform);
        gameObject.transform.localPosition = Vector3.zero;
    }
}
