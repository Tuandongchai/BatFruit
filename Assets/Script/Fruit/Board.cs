using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public List<FruitCell> fruitCells = new List<FruitCell>();

    private void Start()
    {
        Init();
    }
    private void Init()
    {
        fruitCells.Clear ();
        foreach(Transform child in transform)
        {
            fruitCells.Add(child.gameObject.GetComponent<FruitCell>());
        }
    }
}
