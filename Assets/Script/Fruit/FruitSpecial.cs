/*using NaughtyAttributes.Test;
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

public abstract class FruitSpecial : Fruit
{
   
    [SerializeField] protected Board board;
    //[SerializeField] protected FruitType type;
    [SerializeField] protected Vector2 pos;
    [SerializeField] protected GameObject lineRenPrefab;
    protected override void Start()
    {
        base.Start();
        this.type = gameObject.GetComponent<Fruit>().GetFruitType();
        this.pos = transform.parent.GetComponent<FruitCell>().GetXY();
        board = FindObjectOfType<Board>();
    }
    public virtual void ActiveSpecialEffect(int index, FruitCell b)
    {
        if (index == 0)
            MissileWithMissileCase(b);
        else if (index == 1)
            MissileWithBombCase(b);
        else if (index == 2)
            BombWithBombCase(b);
        else if (index == 3)
            RubikWithRubik(b);
        else if (index == 4)
            StartCoroutine(RubikWithMissile());


    }

    public IEnumerator ActiveEffect(FruitCell a=null, FruitCell b=null)
    {
        //yield return StartCoroutine(Active(a, b));
        FruitController.instance.StartTrackedCoroutine(Active(a, b));
        yield break;
    }
    protected abstract IEnumerator Active(FruitCell a, FruitCell b);

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
    protected void MissileWithMissileCase(FruitCell b)
    {
        List<FruitCell> cells = new List<FruitCell>();
        foreach(FruitCell f in board.fruitCells)
        {
            if ((f.GetXY().x==b.GetXY().x && !cells.Contains(f))||((f.GetXY().y == b.GetXY().y && !cells.Contains(f))))
            {
                cells.Add(f);
            }
        }
        cells.Add(this.GetComponent<FruitCell>());
        if (cells.Count == 0)
            return;
        foreach (FruitCell cell in cells)
        {
           // StartCoroutine(cell?.GetFruit()?.GetComponent<Fruit>()?.DestroyThis());
            cell?.GetFruit()?.GetComponent<Fruit>()?.DestroyThis(0);
        }

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
            StartCoroutine(cell?.GetFruit()?.GetComponent<Fruit>()?.DestroyThis());
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
            StartCoroutine(cell?.GetFruit()?.GetComponent<Fruit>()?.DestroyThis());
        }

    }
    protected void RubikWithRubik(FruitCell b)
    {
        ac = true;
        List<FruitCell> cells = new List<FruitCell>();
        foreach (FruitCell f in board.fruitCells)
        {
            cells.Add(f);
        }

        if (cells.Count == 0)
            return;
        foreach (FruitCell cell in cells)
        {
            StartCoroutine(cell?.GetFruit()?.GetComponent<Fruit>()?.DestroyThis());
        }
    }
    public IEnumerator RubikWithMissile()
    {
        ac = true;
        List<FruitCell> cells = new List<FruitCell>();
        cells = GetMostColorCells();
        if (cells.Count == 0)
            yield break;
        foreach (FruitCell cell in cells)
        {
            StartCoroutine( cell?.GetFruit()?.GetComponent<Fruit>()?.DestroyThis());
            Spawner.Instance.SpawnSpecialFruit(UnityEngine.Random.Range(0, 2), cell);
        }
        yield return new WaitForSeconds(1);
        foreach (FruitCell cell in cells)
            StartCoroutine( cell?.GetFruit()?.GetComponent<Fruit>()?.DestroyThis());
    }
    public IEnumerator RubikWithBomb()
    {
        ac = true;
        List<FruitCell> cells = new List<FruitCell>();
        cells = GetMostColorCells();
        if (cells.Count == 0)
            yield break;
        foreach (FruitCell cell in cells)
        {
            StartCoroutine(cell?.GetFruit()?.GetComponent<Fruit>()?.DestroyThis());
            Spawner.Instance.SpawnSpecialFruit(2, cell);
        }
        yield return new WaitForSeconds(1);
        foreach (FruitCell cell in cells)
            StartCoroutine(cell?.GetFruit()?.GetComponent<Fruit>()?.DestroyThis());
    }
    
}
*/
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
    [SerializeField] protected bool isRubik;
    [SerializeField] protected GameObject RubikWithMissileEffectPrefab;
    [SerializeField] protected GameObject RubikWithBombEffectPrefab;
    [SerializeField] protected GameObject RubikWithRubikPrefab;
    [SerializeField] protected GameObject BombWithBomPrefab;

    protected bool isActive = true;
    protected virtual void Start()
    {
        this.type = gameObject.GetComponent<Fruit>().GetFruitType();
        this.pos = transform.parent.GetComponent<FruitCell>().GetXY();
        board = FindObjectOfType<Board>();
    }
    public void ActiveSpecialEffect(int index, FruitCell b, FruitCell a =null)
    {
        if (index == 0)
            MissileWithMissileCase(b);
        else if (index == 1)
            MissileWithBombCase(b);
        else if (index == 2)
            BombWithBombCase(b,a);
        else if (index == 3)
            RubikWithRubik(b,a);
        /*else if (index == 4)
            StartCoroutine(RubikWithMissile());*/

    }

    public void ActiveEffect(FruitCell a = null, FruitCell b = null)
    {
        Active(a, b);
    }
    protected abstract void Active(FruitCell a, FruitCell b);

    protected virtual List<FruitCell> FruitCells(FruitCell a = null, FruitCell b = null)
    {
        this.pos = transform.parent.GetComponent<FruitCell>().GetXY();

        return null;
    }


    protected List<FruitCell> GetMostColorCells()
    {
        FruitType[] colors = new FruitType[] { FruitType.orange, FruitType.Banana, FruitType.grapes, FruitType.Pear };
        List<FruitCell> maxCells = new List<FruitCell>();
        for (int i = 0; i < colors.Length; i++)
        {
            List<FruitCell> cells = new List<FruitCell>();
            foreach (FruitCell f in board.fruitCells)
            {
                if (f.GetFruitType() == colors[i] && !cells.Contains(f))
                    cells.Add(f);
            }
            if (maxCells.Count <= cells.Count)
                maxCells = cells;
        }
        return maxCells;
    }
    protected void MissileWithMissileCase(FruitCell b)
    {
        List<FruitCell> cells = new List<FruitCell>();
        foreach (FruitCell f in board.fruitCells)
        {
            if ((f.GetXY().x == b.GetXY().x && !cells.Contains(f)) || ((f.GetXY().y == b.GetXY().y && !cells.Contains(f))))
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
    protected void MissileWithBombCase(FruitCell b)
    {
        List<FruitCell> cells = new List<FruitCell>();
        foreach (FruitCell f in board.fruitCells)
        {
            if (Mathf.Abs(f.GetXY().x - b.GetXY().x) <= 1 && !cells.Contains(f))
            {
                cells.Add(f);
            }
            else if (Mathf.Abs(f.GetXY().y - b.GetXY().y) <= 1 && !cells.Contains(f))
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
    protected void BombWithBombCase(FruitCell b, FruitCell a)
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
        /*foreach (FruitCell cell in cells)
        {
            cell?.GetFruit()?.GetComponent<Fruit>()?.DestroyThis();
        }*/
        GameObject he = Instantiate(BombWithBomPrefab, this.gameObject.transform.position, Quaternion.identity);
        he.transform.SetParent(GameObject.Find("HandleEffectPos").transform);

        CoroutineRunner.Instance.RunCoroutine(he.gameObject.GetComponent<BombWithBombEffect>().Active(cells, this.transform));
        
        this.transform.parent.GetComponent<FruitCell>().GetFruit()?.GetComponent<Fruit>()?.Destroyit(0.1f);
        a.GetComponent<FruitCell>().GetFruit()?.GetComponent<Fruit>()?.Destroyit(0.1f);

    }
    protected void RubikWithRubik(FruitCell b, FruitCell a)
    {
        isActive = false;
        List<FruitCell> cells = new List<FruitCell>();
        foreach (FruitCell f in board.fruitCells)
        {
            cells.Add(f);
        }

        if (cells.Count == 0)
            return;
        /*foreach (FruitCell cell in cells)
        {
            cell?.GetFruit()?.GetComponent<Fruit>()?.DestroyThis();
        }*/
        GameObject he = Instantiate(RubikWithRubikPrefab, this.gameObject.transform.position, Quaternion.identity);
        he.transform.SetParent(GameObject.Find("HandleEffectPos").transform);

        StartCoroutine(he.gameObject.GetComponent<RubikWithRubikEffect>().Active(cells));

        this.transform.parent.GetComponent<FruitCell>().GetFruit()?.GetComponent<Fruit>()?.DestroyThis(0.1f);
        a.GetComponent<FruitCell>().GetFruit()?.GetComponent<Fruit>()?.DestroyThis(0.1f);

    }
    public IEnumerator RubikWithMissile(FruitCell a, FruitCell b)
    {
        isActive = false;
        List<FruitCell> cells = new List<FruitCell>();
        cells = GetMostColorCells();
        if (cells.Count == 0)
            yield break;
        /*foreach (FruitCell cell in cells)
        {
            cell?.GetFruit()?.GetComponent<Fruit>()?.DestroyThis();
            Spawner.Instance.SpawnSpecialFruit(UnityEngine.Random.Range(0, 2), cell);
        }
        yield return new WaitForSeconds(1);
        foreach (FruitCell cell in cells)
            cell?.GetFruit()?.GetComponent<Fruit>()?.DestroyThis();*/


        GameObject he = Instantiate(RubikWithMissileEffectPrefab, this.gameObject.transform.position, Quaternion.identity);
        he.transform.SetParent(GameObject.Find("HandleEffectPos").transform);

        StartCoroutine(he.gameObject.GetComponent<RubikWithMissileEffect>().Active(cells));

        this.transform.parent.GetComponent<FruitCell>().GetFruit()?.GetComponent<Fruit>()?.DestroyThis(0.1f);
        a.GetFruit()?.GetComponent<Fruit>()?.DestroyThis(0.1f);
    }
    public IEnumerator RubikWithBomb(FruitCell a, FruitCell b)
    {
        isActive = false;
        List<FruitCell> cells = new List<FruitCell>();
        cells = GetMostColorCells();
        if (cells.Count == 0)
            yield break;
        /*foreach (FruitCell cell in cells)
        {
            cell?.GetFruit()?.GetComponent<Fruit>()?.DestroyThis();
            Spawner.Instance.SpawnSpecialFruit(2, cell);
        }
        yield return new WaitForSeconds(1);
        foreach (FruitCell cell in cells)
            cell?.GetFruit()?.GetComponent<Fruit>()?.DestroyThis();*/
        GameObject he = Instantiate(RubikWithBombEffectPrefab, this.gameObject.transform.position, Quaternion.identity);

        he.transform.SetParent(GameObject.Find("HandleEffectPos").transform);
        CoroutineRunner.Instance.RunCoroutine(he.gameObject.GetComponent<RubikWithBombEffect>().Active(cells));

        this.transform.parent.GetComponent<FruitCell>().GetFruit()?.GetComponent<Fruit>()?.DestroyThis(0.1f);
        //
        a.GetFruit()?.GetComponent<Fruit>()?.DestroyThis(0.1f);
    }
}