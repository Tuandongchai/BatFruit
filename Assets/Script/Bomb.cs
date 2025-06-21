using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : FruitSpecial
{
    protected override void Start()
    {
        base.Start();
    }
    protected override List<FruitCell> FruitCells(FruitCell a = null, FruitCell b = null)
    {
        base.FruitCells(a, b);
        List<FruitCell> cells = new List<FruitCell>();
        foreach (FruitCell f in board.fruitCells)
        {
            Vector2 xy = f.GetXY();
            int dx = (int)Mathf.Abs(xy.x - pos.x);
            int dy = (int)Mathf.Abs(xy.y - pos.y);

            if ((dx <= 2 && dy <= 2) && !(dx == 0 && dy == 0))
            {
                if (!cells.Contains(f))
                    cells.Add(f);
            }
        }
        cells.Add(this.transform.parent.GetComponent<FruitCell>());
        return cells;

    }
}
