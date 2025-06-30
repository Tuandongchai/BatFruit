using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class BombSupportTool : ToolBase
{
    [SerializeField] private GameObject bomb_VFX;
    protected override void Start()
    {
        base.Start(); 
    }
    public override void Active()
    {
        base.Active();

       
    }
    public override IEnumerator ActiveEffect()
    {
        StartCoroutine(base.ActiveEffect());

        if (cellChoose == null)
            yield break;
        toolUI.BombBtnUIEndAnimation();
        List<FruitCell> cells = GetFruitCells();
        if (cells.Count == 0)
            yield break;
        SpawnVFX();

        yield return new WaitForSeconds(0.5f);
        foreach (FruitCell cell in cells)
        {
            GameObject fruit = cell?.GetFruit();
            if (fruit != null)
            {
                Debug.Log("cell to destroy (x, y): ("+ cell.GetXY().x + ", "+ cell.GetXY().y +")");
                cell.ChangeFruit(null);
                fruit.GetComponent<Fruit>().DestroyThis();
            }
        }
        isActive = false;

        yield return new WaitForSeconds(0.2f);
        yield return StartCoroutine(FruitController.instance.WaitToFallAndSpawn());
        FruitController.instance.HandleMatches();
        FruitController.instance.isMatching = false;

    }

    protected List<FruitCell> GetFruitCells()
    {
        Vector2 pos = base.cellChoose.GetXY();
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
        cells.Add(cellChoose);
        return cells;

    }
    private void SpawnVFX()
    {
        GameObject vfx = Instantiate(bomb_VFX, Vector2.zero, Quaternion.identity);
        vfx.transform.position = new Vector3(cellChoose.transform.position.x, cellChoose.transform.position.y, -3);
    }
}

