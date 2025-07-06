using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rubik : FruitSpecial
{
    [SerializeField] private Transform handleEffectTrans;
    [SerializeField] private GameObject handleEffectPrefab;
    [SerializeField] private AudioSO audioSO;

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

        StartCoroutine(he.gameObject.GetComponent<RubikEffect>().Active(cells));

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