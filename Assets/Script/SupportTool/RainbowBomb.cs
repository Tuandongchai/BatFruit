using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowBomb : ToolBase
{
    [SerializeField] private GameObject root_VFX;
    [SerializeField] private GameObject lineRenPrefab;
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
        toolUI.RainbowBtnUIEndAnimation();
        List<FruitCell> cells = GetFruitCells();
        if (cells.Count == 0)
            yield break;
        GameObject root = Instantiate(root_VFX, cellChoose.transform.position, Quaternion.identity);
        foreach (FruitCell cell in cells)
        {
            GameObject fruit = cell?.GetFruit();
            if (fruit != null)
            {
                cell.ChangeFruit(null);
                StartCoroutine(SpawnLine(cellChoose.transform, cell.transform, fruit, root));
                yield return new WaitForSeconds(0.3f);
                
            }
        }
        isActive = false;

        yield return new WaitForSeconds(0.5f);
        LeanTween.scale(root, Vector3.zero, 0.3f).setEase(LeanTweenType.easeOutBack);
        Destroy(root,0.5f);
        yield return StartCoroutine(FruitController.instance.WaitToFallAndSpawn());
        FruitController.instance.HandleMatches();
        FruitController.instance.isMatching = false;

    }

    protected List<FruitCell> GetFruitCells()
    {
        FruitType type= base.cellChoose.GetFruitType();
        if (board == null)
            board = GameObject.FindObjectOfType<Board>();
        List<FruitCell> cells = new List<FruitCell>();

        foreach (FruitCell f in board.fruitCells)
        {
            if(f.GetFruitType()== type)
                cells.Add(f);
        }
        return cells;

    }

    protected virtual IEnumerator SpawnLine(Transform startPos, Transform endPos, GameObject fruit,GameObject root)
    {
        GameObject line = Instantiate(lineRenPrefab, Vector2.zero, Quaternion.identity);
        line.GetComponent<LineRenderer>().SetPosition(0, startPos.position);
        line.GetComponent<LineRenderer>().SetPosition(1, endPos.position);

        yield return new WaitForSeconds(0.3f);
        int shakeTweenId = LeanTween.rotateZ(fruit, 50f, 0.1f)
                     .setEase(LeanTweenType.easeInOutSine)
                     .setLoopPingPong().uniqueId;
        yield return new WaitForSeconds(0.4f);
        LeanTween.cancel(shakeTweenId);
        fruit.transform.rotation = Quaternion.identity;
        LeanTween.move(fruit.gameObject, cellChoose.transform.position, 0.4f).setEase(LeanTweenType.easeInOutQuad);

        yield return new WaitForSeconds(0.4f);
        LeanTween.scale(root, root.transform.localScale + new Vector3( 0.02f,0.02f,0.02f), 0.01f).setEase(LeanTweenType.easeOutBack);
        fruit.GetComponent<Fruit>().DestroyThis();
        yield return null;
    }

}
