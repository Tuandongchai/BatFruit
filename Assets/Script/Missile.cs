using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : FruitSpecial
{
    public void ActiveEffect(FruitCell a, FruitCell b)
    {
        Active(a, b);
    }
    protected override void Active(FruitCell a, FruitCell b)
    {
        List<FruitCell> cells = new List<FruitCell>();
        cells = FruitCells(a, b);
        foreach (FruitCell cell in cells)
        {
            cell.GetFruit().GetComponent<Fruit>().DestroyThis();
        }
    }


}
