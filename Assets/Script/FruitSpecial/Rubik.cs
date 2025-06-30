/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rubik : FruitSpecial
{
    [SerializeField] private GameObject handleEffect;
    [SerializeField] private Transform handleEffectTrans;
    protected override void Start()
    {
        base.Start();
        
    }
    
    protected override IEnumerator Active(FruitCell a, FruitCell b)
    {
        if (ac) yield break;
        ac = true;

       *//* List<FruitCell> cells = FruitCells(a, b);
        if (cells.Count == 0)
        {
            ac = false;
            yield break;
        }

        int completed = 0;

        foreach (FruitCell cell in cells)
        {
            StartCoroutine(EffectSequence(cell, () => completed++));
        }

        while (completed < cells.Count)
        {
            yield return null; // ??i t?t c? coroutine ch?y xong
        }*//*
        GameObject he = Instantiate(handleEffect, this.transform.position, Quaternion.identity, handleEffectTrans);
        he.GetComponent<HandleEffect>().Active(FruitCells(a,b));

        ac = false;
    }

    


    *//*protected override IEnumerator Active(FruitCell a, FruitCell b)
    {
        List<FruitCell> cells = FruitCells(a, b);
        if (cells.Count == 0)
            yield break;

        int completedCount = 0;

        foreach (FruitCell cell in cells)
        {
            StartCoroutine(EffectSequence(cell, () => completedCount++));
        }

        while (completedCount < cells.Count)
            yield return null;
    }

    private IEnumerator EffectSequence(FruitCell cell, System.Action onComplete)
    {
        yield return StartCoroutine(SpawnLine(this.transform, cell.transform));
        yield return StartCoroutine(WaitToDestroy(cell));

        onComplete?.Invoke();
    }*//*

    protected override List<FruitCell> FruitCells(FruitCell a = null, FruitCell b = null)
    {
        //base.FruitCells(a, b);
        List<FruitCell> cells = new List<FruitCell>();
        foreach (FruitCell f in board?.fruitCells)
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
    
    *//*IEnumerator WaitToDestroy(FruitCell c)
    {
        yield return new WaitForSeconds(1f);
        c?.GetFruit()?.GetComponent<Fruit>()?.DestroyThis();
        yield break;
    }*//*
    
   
}
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rubik : FruitSpecial
{
    [SerializeField] private Transform handleEffectTrans;
    [SerializeField] private GameObject handleEffectPrefab;

    protected override void Start()
    {
        base.Start();
    }
    protected override void Active(FruitCell a, FruitCell b)
    {
        if (!isActive)
            return;
        List<FruitCell> cells = new List<FruitCell>();
        cells = FruitCells(a, b);
        if (cells.Count == 0)
            return;
        /*foreach (FruitCell cell in cells)
        {
            cell?.GetFruit()?.GetComponent<Fruit>()?.DestroyThis();
        }*/
        GameObject he = Instantiate(handleEffectPrefab, this.gameObject.transform.position, Quaternion.identity);
        he.transform.SetParent(GameObject.Find("HandleEffectPos").transform) ;

        StartCoroutine(he.gameObject.GetComponent<HandleEffect>().Active(cells));

        this.transform.parent.GetComponent<FruitCell>().GetFruit()?.GetComponent<Fruit>()?.DestroyThis(0.1f);
    }
    protected override List<FruitCell> FruitCells(FruitCell a = null, FruitCell b = null)
    {
        base.FruitCells(a, b);
        if (board == null)
            board = GameObject.FindObjectOfType<Board>();
        List<FruitCell> cells = new List<FruitCell>();
        foreach (FruitCell f in board?.fruitCells)
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
        //cells.Add(this.transform.parent.GetComponent<FruitCell>());
        return cells;


    }

}