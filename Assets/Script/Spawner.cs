using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance;

    [SerializeField] private Board board;

    [SerializeField] private List<Fruit> fruits = new List<Fruit>();
    [SerializeField] private List<FruitSpecial> fruitsSpecial = new List<FruitSpecial>();
    private Dictionary<Vector2Int, FruitCell> cellMap = new Dictionary<Vector2Int, FruitCell>();


    private int completedColumnCount = 0;
    private int totalColumns = 0;

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else 
            Destroy(gameObject);
        Init();
    }

    public void Init()
    {
        cellMap.Clear();
        foreach (FruitCell fc in board.fruitCells)
        {
            Vector2Int pos = Vector2Int.RoundToInt(fc.GetXY());
            cellMap[pos] = fc;
        }
    }

    
    public IEnumerator Falling()
    {
        Init();

        int xMin = cellMap.Keys.Min(pos => pos.x);
        int xMax = cellMap.Keys.Max(pos => pos.x);

        completedColumnCount = 0;
        totalColumns = xMax - xMin + 1;

        for (int x = xMin; x <= xMax; x++)
        {
            StartCoroutine(FallAndSpawnInColumn(x));
        }

        // Ch? cho ??n khi t?t c? coroutine hoàn thành
        yield return new WaitUntil(() => completedColumnCount == totalColumns);
    }

    private IEnumerator FallAndSpawnInColumn(int x)
    {
        int yMin = cellMap.Keys.Min(pos => pos.y);
        int yMax = cellMap.Keys.Max(pos => pos.y);

        int countFruitToSpawn = 0;

        for (int y = yMin + 1; y <= yMax; y++)
        {
            Vector2Int currentPos = new Vector2Int(x, y);
            if (!cellMap.ContainsKey(currentPos)) continue;

            FruitCell currentCell = cellMap[currentPos];
            if (currentCell.GetFruit() == null) continue;

            int fallY = y - 1;
            while (fallY >= yMin)
            {
                yield return new WaitForSeconds(0.07f);
                Vector2Int belowPos = new Vector2Int(x, fallY);
                if (cellMap.ContainsKey(belowPos))
                {
                    FruitCell belowCell = cellMap[belowPos];
                    if (belowCell.GetFruit() == null)
                    {
                        belowCell.ChangeFruit(currentCell.GetFruit());
                        currentCell.ChangeFruit(null);
                        currentCell = belowCell;
                        fallY--;
                    }
                    else break;
                }
                else break;
            }
        }

        // Cout cell has  not fruit
        for (int i = yMin; i <= yMax; i++)
        {
            Vector2Int pos = new Vector2Int(x, i);
            if (cellMap.TryGetValue(pos, out FruitCell cell))
            {
                if (cell.GetFruit() == null)
                    countFruitToSpawn++;
            }
        }
        while (countFruitToSpawn > 0)
        {
            Vector2Int spawnPos = new Vector2Int(x, yMax + 1);
            GameObject fruitSpawn = Instantiate(fruits[Random.Range(0, fruits.Count)].gameObject, Vector3.zero, Quaternion.identity);

            FruitCell targetCell = null;

            for (int y = yMax; y >= yMin; y--)
            {
                Vector2Int pos = new Vector2Int(x, y);
                if (cellMap.TryGetValue(pos, out targetCell) && targetCell.GetFruit() == null)
                {
                    fruitSpawn.transform.SetParent(targetCell.transform);
                    targetCell.ChangeFruit(fruitSpawn);

                    int fallY = y;
                    FruitCell current = targetCell;

                    while (fallY > yMin)
                    {
                        yield return new WaitForSeconds(0.07f);
                        fallY--;
                        Vector2Int belowPos = new Vector2Int(x, fallY);
                        if (cellMap.TryGetValue(belowPos, out FruitCell belowCell) && belowCell.GetFruit() == null)
                        {
                            belowCell.ChangeFruit(current.GetFruit());
                            current.ChangeFruit(null);
                            current = belowCell;
                        }
                        else break;
                    }

                    break;
                }
            }

            countFruitToSpawn--;
        }
        completedColumnCount ++;
        
    }
    public void SpawnSpecialFruit(int fruitIndex, FruitCell cell)
    {
        GameObject fruitSpawn = Instantiate(fruitsSpecial[fruitIndex].gameObject, Vector3.zero, Quaternion.identity);
        fruitSpawn.transform.SetParent(cell.gameObject.transform);
        cell.ChangeFruit(fruitSpawn);
    }


}
