using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningTool : ToolBase
{
    [SerializeField] private GameObject lightning_VFX;
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
        toolUI.lightningBtnUIEndAnimation();
        List<FruitCell> cells = GetFruitCells();
        if (cells.Count == 0)
            yield break;
        SpawnVFX();
        foreach (FruitCell cell in cells)
        {
            GameObject fruit = cell?.GetFruit();
            if (fruit != null)
            {
                Debug.Log("cell to destroy (x, y): (" + cell.GetXY().x + ", " + cell.GetXY().y + ")");
                cell.ChangeFruit(null);
                fruit.GetComponent<Fruit>().DestroyThis();
            }
        }
        isActive = false;

        yield return new WaitForSeconds(0.3f);
        yield return StartCoroutine(FruitController.instance.WaitToFallAndSpawn());
        FruitController.instance.HandleMatches();
        FruitController.instance.isMatching = false;

    }

    protected List<FruitCell> GetFruitCells()
    {
        FruitType type = base.cellChoose.GetFruitType();
        if (board == null)
            board = GameObject.FindObjectOfType<Board>();
        List<FruitCell> cells = new List<FruitCell>();

        cells.Add(cellChoose);
        return cells;
    }
    private void SpawnVFX()
    {
        GameObject vfx = Instantiate(lightning_VFX, Vector2.zero, Quaternion.identity);
        vfx.transform.position = cellChoose.transform.position;
    }
}
