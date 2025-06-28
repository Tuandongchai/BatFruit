/*using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Missile : FruitSpecial
{
    protected override void Start()
    {
        base.Start();
    }
    protected override IEnumerator Active(FruitCell a, FruitCell b)
    {
        if (this == null)
            yield break;
        List<FruitCell> cells = new List<FruitCell>();
        cells = FruitCells(a, b);
        if (cells.Count == 0)
            yield break;
        foreach (FruitCell cell in cells)
        {
            GameObject go = cell?.GetFruit();
            if (go == null || go.Equals(null)) continue;

            Fruit fruit = go.GetComponent<Fruit>();
            if (fruit == null) continue;

            *//*if (fruit.GetFruitType() == FruitType.Rubik)
            {
                yield return StartCoroutine(fruit.DestroyThis(true, 1.1f));
                fruit.DestroyThisGameObject();
            }
            else 
                fruit.DestroyThis(0);*//*
            fruit.DestroyThis(0);
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
*/
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
            GameObject fruit = cell?.GetFruit();
            if (fruit != null)
            {
                // Xóa reference tr??c
                cell.ChangeFruit(null);
                fruit.GetComponent<Fruit>().DestroyThis();
            }
        }

    }
    protected override List<FruitCell> FruitCells(FruitCell a = null, FruitCell b = null)
    {
        this.pos = transform.parent.GetComponent<FruitCell>().GetXY();
        if (board == null)
            board = GameObject.FindObjectOfType<Board>();

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