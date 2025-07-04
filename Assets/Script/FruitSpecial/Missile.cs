
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : FruitSpecial
{
    [SerializeField] private GameObject MissileHorEffectPrefab;
    [SerializeField] private GameObject MissileVerEffectPrefab;
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
        /*foreach (FruitCell cell in cells)
        {
            GameObject fruit = cell?.GetFruit();
            if (fruit != null)
            {
                // X�a reference tr??c
                cell.ChangeFruit(null);
                fruit.GetComponent<Fruit>().DestroyThis();
            }
        }*/
        if (type == FruitType.Missile_Ver)
        {

            GameObject he = Instantiate(MissileHorEffectPrefab, this.gameObject.transform.position, Quaternion.identity);
            he.transform.SetParent(GameObject.Find("HandleEffectPos").transform);

            //StartCoroutine(he.gameObject.GetComponent<MissileHorEffect>().Active(cells, this.transform));
            CoroutineRunner.Instance.RunCoroutine(he.GetComponent<MissileHorEffect>().Active(cells, this.transform));

            this.transform.parent.GetComponent<FruitCell>().GetFruit()?.GetComponent<Fruit>()?.DestroyThis(0.1f);
        }
        else if(type == FruitType.Missile_Hor)
        {
            GameObject he = Instantiate(MissileVerEffectPrefab, this.gameObject.transform.position, Quaternion.identity);
            he.transform.SetParent(GameObject.Find("HandleEffectPos").transform);

            //StartCoroutine(he.gameObject.GetComponent<MissileHorEffect>().Active(cells, this.transform));
            CoroutineRunner.Instance.RunCoroutine(he.GetComponent<MissileVerEffect>().Active(cells, this.transform));

            this.transform.parent.GetComponent<FruitCell>().GetFruit()?.GetComponent<Fruit>()?.DestroyThis(0.1f);
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