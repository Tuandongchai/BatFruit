using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : FruitSpecial
{
    protected override void Start()
    {
        base.Start();
    }
    protected override void Active(FruitCell a, FruitCell b)
    {
        List<FruitCell> cells = new List<FruitCell>();
        cells = FruitCells(a, b);
        if (cells.Count == 0)
            return;
        foreach (FruitCell cell in cells)
        {
            cell?.GetFruit()?.GetComponent<Fruit>()?.DestroyThis();
        }
    }
    protected override List<FruitCell> FruitCells(FruitCell a = null, FruitCell b = null)
    {
        base.FruitCells(a, b);
        List<FruitCell> cells = new List<FruitCell>();
        if (type == FruitType.Missile_Hor)
        {
            foreach (FruitCell f in board.fruitCells)
            {
                if (f.GetXY().x == pos.x)
                    cells.Add(f);
            }
            return cells;
        }
        else if (type == FruitType.Missile_Ver)
        {
            foreach (FruitCell f in board.fruitCells)
            {
                if (f.GetXY().y == pos.y)
                    cells.Add(f);
            }
            return cells;
        }
        return null;
    }


}
