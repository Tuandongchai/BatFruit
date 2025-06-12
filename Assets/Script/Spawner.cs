using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private int yMin;
    [SerializeField] private int yMax;
    [SerializeField] private int xMin;
    [SerializeField] private int xMax;
    [SerializeField] private Board board;

    private Dictionary<Vector2Int, FruitCell> cellMap = new Dictionary<Vector2Int, FruitCell>();

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        cellMap.Clear();
        foreach (FruitCell fc in board.fruitCells)
        {
            Vector2Int pos = Vector2Int.RoundToInt(fc.GetXY());
            cellMap[pos] = fc;
        }
    }

    public void Falling()
    {
        Init(); // ??m b?o map c?p nh?t

        for (int x = xMin; x < xMax; x++)
        {
            for (int y = yMin + 1; y < yMax; y++)
            {
                Vector2Int currentPos = new Vector2Int(x, y);
                if (!cellMap.ContainsKey(currentPos)) continue;

                FruitCell currentCell = cellMap[currentPos];
                if (currentCell.GetFruit() == null) continue;

                int fallY = y - 1;

                // Tìm v? trí th?p nh?t có th? r?i xu?ng
                while (fallY >= yMin)
                {
                    Vector2Int belowPos = new Vector2Int(x, fallY);
                    if (cellMap.ContainsKey(belowPos))
                    {
                        FruitCell belowCell = cellMap[belowPos];
                        if (belowCell.GetFruit() == null)
                        {
                            // Di chuy?n trái cây xu?ng
                            belowCell.ChangeFruit(currentCell.GetFruit());
                            currentCell.ChangeFruit(null);

                            currentCell = belowCell;
                            fallY--;
                        }
                        else break;
                    }
                    else break;
                }
            }
        }
    }
}
