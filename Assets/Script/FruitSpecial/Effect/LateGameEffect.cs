using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LateGameEffect : HandleEffect
{
    [SerializeField] private GameObject bombFruit;
    [SerializeField] private GameObject effectSpawn;
    [SerializeField] private RectTransform uiObject;
    [SerializeField] private GameObject particelUIPrefab;
    [SerializeField] private Camera cam;
    private GameObject go;
    private bool created=false;
    protected override void Start()
    {
        if (cam == null) cam = Camera.main;
        board = FindObjectOfType<Board>();
        uiObject = FindObjectOfType<FlyToWorldTargetUI>().GetComponent<RectTransform>();
        Destroy(gameObject, 60f);
    }
    public override IEnumerator Active(List<FruitCell> list, Transform trans = null)
    {
        StartCoroutine(FruitController.instance.WaitToFallAndSpawn());
        List<FruitCell> cellList = list;
        if (RubikParticle != null)
            SpawnRubikParticle();
/*        while (FruitController.instance.isMatching)
        {
            yield return null;
        }*/
        if (cellList.Count == 0)
        {
            yield break;
        }
        yield return StartCoroutine(FlyToFruitCellandSpawn(list));

        foreach (FruitCell cell in cellList)
        {
            GameObject fruit = cell?.GetFruit();
            if (fruit != null)
            {
                cell.ChangeFruit(null);
                fruit.GetComponent<Fruit>().DestroyThis();
            }
        }
        yield return null;
        Destroy(go);
        yield return new WaitForSeconds(1.1f);
        GameManager.instance.SetGameState(GameState.Win);
    }
    /*private IEnumerator SpawnBomb(List<FruitCell> cells)
    {
        foreach (FruitCell cell in cells)
        {
            GameObject fruit = cell?.GetFruit();
            if (fruit != null)
            {
                cell.ChangeFruit(null);
                fruit.GetComponent<Fruit>().DestroyThis();
                GameObject bomb = Instantiate(bombFruit);
                cell.ChangeFruit(bomb);
                bomb.GetComponent<Fruit>().ChangeParent(cell.gameObject);
            }
            yield return null;
        }
        
        yield return new WaitForSeconds(1);
    }*/

    private IEnumerator FlyToFruitCellandSpawn(List<FruitCell> cells)
    {
        foreach (FruitCell cell in cells)
        {
            yield return StartCoroutine(Fly(cell));
            Instantiate(effectSpawn, cell.transform.position, Quaternion.identity);
            //
            LevelManager.instance.ReduceStep();
            //
            GameObject fruit = cell?.GetFruit();
            if (fruit != null)
            {
                cell.ChangeFruit(null);
                fruit.GetComponent<Fruit>().DestroyThis(0.02f);
                StartCoroutine(FruitController.instance.WaitToFallAndSpawn());
            }
            GameObject bomb = Instantiate(bombFruit);
            cell.ChangeFruit(bomb);
            bomb.GetComponent<Fruit>().ChangeParent(cell.gameObject);
            yield return null;
        }

        yield return new WaitForSeconds(1);
    }

    private IEnumerator Fly(FruitCell cell)
    {
        if (cam == null) cam = Camera.main;
        Vector2 screenPos = cam.WorldToScreenPoint(cell.transform.position);
        if(uiObject == null)
            uiObject = FindObjectOfType<FlyToWorldTargetUI>().GetComponent<RectTransform>();
        Vector2 startPos = uiObject.position;

        if(created == false) {
            GameObject go = Instantiate(particelUIPrefab);
            go.transform.SetParent(FindObjectOfType<Canvas>().transform);
            go.transform.localPosition = startPos;
            created = true;
        
        }
        yield return null;

        Vector2 controlPoint = (startPos + screenPos) / 2 + new Vector2(Random.Range(-100f, 100f), Random.Range(50f, 150f));

        LeanTween.value(0f, 1f, 0.5f)
            .setOnUpdate((float t) =>
            {
                Vector2 p0 = startPos;
                Vector2 p1 = controlPoint;
                Vector2 p2 = screenPos;

                Vector2 curved = Mathf.Pow(1 - t, 2) * p0
                               + 2 * (1 - t) * t * p1
                               + Mathf.Pow(t, 2) * p2;

                uiObject.position = curved;
            })
            .setEase(LeanTweenType.easeInOutCubic)
            .setOnComplete(() =>
            {
                Debug.Log("complete!");
            });

        yield return new WaitForSeconds(0.5f);
        
    }

    public List<FruitCell> GetCellsRandomFromBoard(int n)
    {
        if(board == null)
            board =FindObjectOfType<Board>();
        List<FruitCell> cells = GetRandomSubset(board.fruitCells, n);
        return cells;
    }
    List<T> GetRandomSubset<T>(List<T> source, int count)
    {
        if (count > source.Count)
        { 
            return null;
        }

        List<T> temp = new List<T>(source);
        for (int i = 0; i < temp.Count; i++)
        {
            int rand = Random.Range(i, temp.Count);
            (temp[i], temp[rand]) = (temp[rand], temp[i]);
        }

        return temp.GetRange(0, count);
    }
}
