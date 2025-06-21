using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rubik : FruitSpecial
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
            if (a == null && b == null)
            {
                cells = GetMostColorCells();

            }
            else if (a.GetFruitType() != FruitType.Rubik)
            {
                if (f.GetFruitType() == a.GetFruitType() && !cells.Contains(f))
                {
                    cells.Add(f);
                }

            }
            else if (b.GetFruitType() != FruitType.Rubik)
            {
                if (f.GetFruitType() == b.GetFruitType() && !cells.Contains(f))
                {
                    cells.Add(f);
                }
            }
        }
        cells.Add(this.transform.parent.GetComponent<FruitCell>());
        return cells;


    }

}
