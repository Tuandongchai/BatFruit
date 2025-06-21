using NaughtyAttributes.Test;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;

public abstract class FruitSpecial : MonoBehaviour
{
    [SerializeField] protected Board board;
    [SerializeField] protected FruitType type;
    [SerializeField] protected Vector2 pos;

    protected virtual void Start()
    {
        this.type = gameObject.GetComponent<Fruit>().GetFruitType();
        this.pos = transform.parent.GetComponent<FruitCell>().GetXY();
        board = FindObjectOfType<Board>();
    }
    public void ActiveSpecialEffect(int index, FruitCell b)
    {
        if (index == 0)
            MissileWithMissileCase(b);
        else if (index == 1)
            MissileWithBombCase(b);
        else if (index == 2)
            BombWithBombCase(b);
        else if( index == 3)
            RubikWithRubik(b);
        else if (index == 4)
            StartCoroutine(RubikWithMissile());

    }

    public void ActiveEffect(FruitCell a=null, FruitCell b=null)
    {
        Active(a, b);
    }
    protected void Active(FruitCell a, FruitCell b)
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

    protected virtual List<FruitCell> FruitCells(FruitCell a=null, FruitCell b=null)
    {
        this.pos = transform.parent.GetComponent<FruitCell>().GetXY();
        
        return null;
    }
   

    protected List<FruitCell> GetMostColorCells()
    {
        FruitType[] colors = new FruitType[] { FruitType.orange, FruitType.Banana, FruitType.grapes, FruitType.Pear };
        List<FruitCell> maxCells= new List<FruitCell>();
        for (int i=0; i<colors.Length; i++)
        {
            List<FruitCell> cells= new List<FruitCell>();
            foreach (FruitCell f in board.fruitCells)
            {
                if (f.GetFruitType() == colors[i] && !cells.Contains(f))
                    cells.Add(f);
            }
            if(maxCells.Count <= cells.Count)
                maxCells = cells;
        }
        return maxCells;
    }
    protected List<FruitCell> MissileWithMissileCase(FruitCell b)
    {
        List<FruitCell> cells = new List<FruitCell>();
        foreach(FruitCell f in board.fruitCells)
        {
            if ((f.GetXY().x==b.GetXY().x && !cells.Contains(f))||((f.GetXY().y == b.GetXY().y && !cells.Contains(f))))
            {
                cells.Add(f);
            }
        }
        return cells;            

    }
    protected void MissileWithBombCase(FruitCell b)
    {
        List<FruitCell> cells = new List<FruitCell>();
        foreach (FruitCell f in board.fruitCells)
        {
            if (Mathf.Abs(f.GetXY().x - b.GetXY().x) <=1&& !cells.Contains(f))
            {
                cells.Add(f);
            }
            else if(Mathf.Abs(f.GetXY().y - b.GetXY().y) <= 1 && !cells.Contains(f))
            {
                cells.Add(f);

            }
        }
        if (cells.Count == 0)
            return;
        foreach (FruitCell cell in cells)
        {
            cell?.GetFruit()?.GetComponent<Fruit>()?.DestroyThis();
        }

    }
    protected void BombWithBombCase(FruitCell b)
    {
        List<FruitCell> cells = new List<FruitCell>();
        foreach (FruitCell f in board.fruitCells)
        {
            Vector2 xy = f.GetXY();
            int dx = (int)Mathf.Abs(xy.x - b.GetXY().x);
            int dy = (int)Mathf.Abs(xy.y - b.GetXY().y);

            if ((dx <= 3 && dy <= 3) && !(dx == 0 && dy == 0))
            {
                if (!cells.Contains(f))
                    cells.Add(f);
            }
        }
        cells.Add(this.transform.parent.GetComponent<FruitCell>());
        if (cells.Count == 0)
            return;
        foreach (FruitCell cell in cells)
        {
            cell?.GetFruit()?.GetComponent<Fruit>()?.DestroyThis();
        }

    }
    protected void RubikWithRubik(FruitCell b)
    {
        List<FruitCell> cells = new List<FruitCell>();
        foreach(FruitCell f in board.fruitCells)
        {
            cells.Add(f);
        }

        if (cells.Count == 0)
            return;
        foreach (FruitCell cell in cells)
        {
            cell?.GetFruit()?.GetComponent<Fruit>()?.DestroyThis();
        }
    }
    public IEnumerator RubikWithMissile()
    {
        List<FruitCell> cells = new List<FruitCell>();
        cells = GetMostColorCells();
        if (cells.Count == 0)
            yield break;
        foreach (FruitCell cell in cells)
        {
            cell?.GetFruit()?.GetComponent<Fruit>()?.DestroyThis();
            Spawner.Instance.SpawnSpecialFruit(UnityEngine.Random.Range(0,2), cell);
        }
        yield return new WaitForSeconds(1);
        foreach(FruitCell cell in cells)
            cell?.GetFruit()?.GetComponent<Fruit>()?.DestroyThis();
    }
    public IEnumerator RubikWithBomb()
    {
        List<FruitCell> cells = new List<FruitCell>();
        cells = GetMostColorCells();
        if (cells.Count == 0)
            yield break;
        foreach (FruitCell cell in cells)
        {
            cell?.GetFruit()?.GetComponent<Fruit>()?.DestroyThis();
            Spawner.Instance.SpawnSpecialFruit(2, cell);
        }
        yield return new WaitForSeconds(1);
        foreach (FruitCell cell in cells)
            cell?.GetFruit()?.GetComponent<Fruit>()?.DestroyThis();
    }
}
