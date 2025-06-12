using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class GridTester : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Grid grid;

    [Header(" Setting ")]
    [OnValueChanged("UpdateGridPos")]
    [SerializeField] private Vector3Int gridPos;
    Vector3 half = new Vector3(0.5f, 0.5f, 0.5f);

    private void UpdateGridPos() => transform.position = grid.CellToWorld(gridPos) + half;
}
