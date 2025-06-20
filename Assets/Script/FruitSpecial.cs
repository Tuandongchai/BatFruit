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
    protected abstract void Active(FruitCell a, FruitCell b);

    protected virtual List<FruitCell> FruitCells(FruitCell a, FruitCell b)
    {
        this.pos = transform.parent.GetComponent<FruitCell>().GetXY();
        List<FruitCell> cells = new List<FruitCell>();
        if (type == FruitType.Missile_Hor)
        {
            foreach(FruitCell f in board.fruitCells)
            {
                if(f.GetXY().x == pos.x)
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
        else if (type == FruitType.Bomb)
        {
           
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
        else if (type == FruitType.Rubik)
        {
            // test if it run as expected
            // not qualified
            foreach (FruitCell f in board.fruitCells)
            {
                if(a.GetFruitType()!= FruitType.Rubik)
                {
                    if (f.GetFruitType()==a.GetFruitType() && !cells.Contains(f))
                    {
                        cells.Add(f);
                    }

                }
                else
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
        return null;
    }

    protected virtual List<FruitCell> GetMostColorCells()
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
}
