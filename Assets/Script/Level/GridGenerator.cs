using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [Header("Fruit")]
    [SerializeField] private FruitCell fruitObject;
    [SerializeField] private Grid grid;

    [Header("Settings")]
    [OnValueChanged("GenerateGrid")]
    [SerializeField] private int gridSize;

    private void GenerateGrid()
    {
        for (int i = -gridSize; i < gridSize; i++)
        {
            for(int j = -gridSize; j < gridSize; j++)
            {
                Vector3 spawnPos = grid.CellToWorld(new Vector3Int(i,j,0)) + new Vector3(0.5f, 0.5f, 0f);
                GameObject fruitSpawner = Instantiate(fruitObject.gameObject, spawnPos, Quaternion.identity, transform);
                fruitSpawner.GetComponent<FruitCell>().Init(i,j);
            }
        }
    }

}
