/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : FruitSpecial
{
    protected override void Start()
    {
        base.Start();
    }
    protected override IEnumerator Active(FruitCell a, FruitCell b)
    {
        List<FruitCell> cells = new List<FruitCell>();
        cells = FruitCells(a, b);
        if (cells.Count == 0)
            yield break;
        foreach (FruitCell cell in cells)
        {
            *//*if(cell.GetFruitType()==FruitType.Rubik)
                yield return StartCoroutine(cell?.GetFruit()?.GetComponent<Fruit>()?.DestroyThis());
            else
                cell?.GetFruit()?.GetComponent<Fruit>()?.DestroyThis(0);*//*
            GameObject go = cell?.GetFruit();

            // ki?m tra an toàn tr??c khi g?i GetComponent
            if (go == null || go.Equals(null)) continue;

            Fruit fruit = go.GetComponent<Fruit>();
            if (fruit == null) continue;

            *//*if (fruit.GetFruitType() == FruitType.Rubik)
            {
                yield return StartCoroutine(fruit.DestroyThis(true,1.1f));
                fruit.DestroyThisGameObject();
            }
            else
                fruit.DestroyThis(0);*//*
            fruit.DestroyThis(0);
        }

    }
    protected override List<FruitCell> FruitCells(FruitCell a = null, FruitCell b = null)
    {
        if (board == null)
            board = GameObject.FindObjectOfType<Board>();
        //base.FruitCells(a, b);
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
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : FruitSpecial
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
            //cell?.GetFruit()?.GetComponent<Fruit>()?.DestroyThis();
        }

    }
    protected override List<FruitCell> FruitCells(FruitCell a = null, FruitCell b = null)
    {
        this.pos = transform.parent.GetComponent<FruitCell>().GetXY();
        if (board == null)
            board = GameObject.FindObjectOfType<Board>();
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