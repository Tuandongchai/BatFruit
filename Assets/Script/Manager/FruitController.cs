using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.WSA;

public class FruitController : MonoBehaviour
{
    public static FruitController instance;

    [Header("Setup")]
    [SerializeField] private LayerMask fruitLayer;
    [SerializeField] private Camera fruitCamera;
    [SerializeField] private Board fruitBoard;
    [SerializeField] private Spawner spawner;
    [SerializeField] private GameObject handleEffectTrans;


    private FruitCell firstSelectedCell;
    private FruitCell secondSelectedCell;
    private Vector3 mouseDownWorldPos;

    public bool isMatching = false;
    private bool pause = false;

    public static Action swap;

    private void OnDisable()
    {
        InGameUIManager.pause -= SetPauseGame;

    }
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);

        fruitBoard = FindObjectOfType<Board>();
        spawner = FindObjectOfType<Spawner>();
        InGameUIManager.pause += SetPauseGame;
    }
    private void Update()
    {
        if (pause) return;
        if (isMatching) return;
        ManagerController();
    }

    private void ManagerController()
    {
        if (Input.GetMouseButtonDown(0))
            ManageMouseDown();

        if (Input.GetMouseButtonUp(0))
            ManageMouseUp();
    }

    private void ManageMouseDown()
    {
        firstSelectedCell = GetCellUnderMouse();

        mouseDownWorldPos = GetMouseWorldPosition();
    }

    private void ManageMouseUp()
    {
        Debug.Log(firstSelectedCell);
        if (firstSelectedCell == null) return;


        Vector3 mouseUpWorldPos = GetMouseWorldPosition();
        Debug.Log("Mouse up tai: " + mouseUpWorldPos);
        Vector3 delta = mouseUpWorldPos - mouseDownWorldPos;
        if (delta.magnitude < 0) return;

        Vector2Int direction;

        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            direction = delta.x > 0 ? new Vector2Int(1, 0) : new Vector2Int(-1, 0);
        else
            direction = delta.y > 0 ? new Vector2Int(0, 1) : new Vector2Int(0, -1);

        Vector2 firstPos = firstSelectedCell.GetXY();
        Vector2 secondPos = firstPos + direction;

        FruitCell secondSelectedCell = FindCellAt(secondPos);

        if (secondSelectedCell != null)
            StartCoroutine(TrySwap(firstSelectedCell, secondSelectedCell));

        firstSelectedCell = null;
        mouseDownWorldPos = Vector3.zero;
    }
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = 0f;
        return fruitCamera.ScreenToWorldPoint(mouseScreenPos);
    }
    private FruitCell GetCellUnderMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = fruitCamera.ScreenToWorldPoint(mousePos);
        worldPos.z = 0f;

        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

        if (hit.collider != null)
            return hit.collider.GetComponent<FruitCell>();
        return null;
    }
    private FruitCell FindCellAt(Vector2 pos)
    {

        foreach (FruitCell cell in fruitBoard.fruitCells)
        {
            if (cell.GetXY() == pos)
                return cell;
        }
        return null;
    }
    IEnumerator TrySwap(FruitCell a, FruitCell b)
    {

        GameObject tempA = a.GetFruit();
        GameObject tempB = b.GetFruit();

        a.ChangeFruit(tempB);
        b.ChangeFruit(tempA);

        Debug.Log($"Swapped: {a.GetXY()} <-> {b.GetXY()}");

        yield return StartCoroutine(HandleFruitSpecial(a, b));

        HandleMatches();
        StartCoroutine(BackFruit(a, b, tempA, tempB));
    }
    bool IsMissile(FruitType type) => (type == FruitType.Missile_Ver || type == FruitType.Missile_Hor);
    //switch case de
    IEnumerator HandleFruitSpecial(FruitCell a, FruitCell b)
    {
        while (pause)
            yield return null;
        List<FruitType> specialType = new List<FruitType> { FruitType.Missile_Hor, FruitType.Missile_Ver, FruitType.Bomb, FruitType.Rubik };
        FruitType aType = a.GetFruitType();
        FruitType bType = b.GetFruitType();
        if (specialType.Contains(aType) && specialType.Contains(bType))
        {

            if (IsMissile(aType) && IsMissile(bType))
            {
                b.transform.GetChild(0)?.GetComponent<FruitSpecial>().ActiveSpecialEffect(0, b);
                yield return StartCoroutine(WaitToFallAndSpawn());
            }
            else if ((IsMissile(aType) && bType == FruitType.Bomb) || (IsMissile(bType) && aType == FruitType.Bomb))
            {
                b.transform.GetChild(0)?.GetComponent<FruitSpecial>().ActiveSpecialEffect(1, b);
                yield return StartCoroutine(WaitToFallAndSpawn());
            }
            else if (aType == FruitType.Bomb && bType == FruitType.Bomb)
            {
                b.transform.GetChild(0)?.GetComponent<FruitSpecial>().ActiveSpecialEffect(2, b);
                yield return StartCoroutine(WaitToFallAndSpawn());
            }
            else if (aType == FruitType.Rubik && bType == FruitType.Rubik)
            {
                b.transform.GetChild(0)?.GetComponent<FruitSpecial>().ActiveSpecialEffect(3, b);
                yield return StartCoroutine(WaitToFallAndSpawn());
            }
            else if ((aType == FruitType.Rubik && IsMissile(bType)) || (bType == FruitType.Rubik && IsMissile(aType)))
            {
                yield return StartCoroutine(b.transform.GetChild(0)?.GetComponent<FruitSpecial>().RubikWithMissile());
                yield return StartCoroutine(WaitToFallAndSpawn());
            }
            else if ((aType == FruitType.Rubik && bType == FruitType.Bomb || (bType == FruitType.Rubik && aType == FruitType.Bomb)))
            {

                yield return StartCoroutine(b.transform.GetChild(0)?.GetComponent<FruitSpecial>().RubikWithBomb());
                yield return StartCoroutine(WaitToFallAndSpawn());
            }
        }
        else if (!specialType.Contains(bType) && specialType.Contains(aType))
        {
            //a.transform.GetChild(0)?.GetComponent<FruitSpecial>().ActiveEffect(a, b);
            a.transform.GetChild(0)?.GetComponent<Fruit>().DestroyThis(0.02f,a, b);
            yield return StartCoroutine(WaitToFallAndSpawn());

        }

        else if (specialType.Contains(bType) && !specialType.Contains(aType))
        {
            //b.transform.GetChild(0)?.GetComponent<FruitSpecial>().ActiveEffect(a, b);
            b.transform.GetChild(0)?.GetComponent<Fruit>().DestroyThis(0.02f,a, b);
            yield return StartCoroutine(WaitToFallAndSpawn());
        }


    }
    private IEnumerator BackFruit(FruitCell a, FruitCell b, GameObject tempA, GameObject tempB)
    {

        List<List<FruitCell>> matchGroups = MatchChecker.FindMatches(fruitBoard.fruitCells);
        if (matchGroups.Count > 0)
        {
            swap?.Invoke();
            yield break;

        }

        yield return new WaitForSeconds(0.3f);
        b.ChangeFruit(tempB);
        a.ChangeFruit(tempA);
    }
    public IEnumerator WaitToFallAndSpawn()
    {
        while(handleEffectTrans.transform.childCount>0) 
            yield return null;

        while (pause)
            yield return null;
        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(spawner.Falling());
    }
    private void OnDrawGizmos()
    {
        if (!UnityEngine.Application.isPlaying) return;

        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = fruitCamera.ScreenToWorldPoint(mousePos);
        worldPos.z = 0;
        
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(worldPos, 0.1f);
    }
    public void HandleMatches()
    {
        StartCoroutine(HandleMatchesCoroutine());
    }

    private IEnumerator HandleMatchesCoroutine()
    {
        while (true)
        {
            while (pause)
                yield return null;
            isMatching = false;
            yield return new WaitForSeconds(0.3f);
            List<List<FruitCell>> matchGroups = MatchChecker.FindMatches(fruitBoard.fruitCells);
            if (matchGroups.Count <= 0) break;


            isMatching = true;
            foreach (var group in matchGroups)
            {

                foreach (var cell in group)
                {
                    /*Destroy(cell.GetFruit());*/
                    cell?.GetFruit()?.GetComponent<Fruit>().DestroyThis();
                    cell.ChangeFruit(null);
                }
                SpawnFruitSpecial(group, group[UnityEngine.Random.Range(0, group.Count)]);

            }

            /*yield return new WaitForSeconds(0.2f); */
            yield return StartCoroutine(WaitToFallAndSpawn());
        }
    }
    private void SpawnFruitSpecial(List<FruitCell> group, FruitCell cell)
    {
        if (group.Count == 3)
        {
            return;
        }
        else if (group.Count == 4)
        {
            int index = (int)UnityEngine.Random.Range(0, 2);
            Spawner.Instance.SpawnSpecialFruit(index, cell);
        }
        else if (group.Count == 5)
        {
            Spawner.Instance.SpawnSpecialFruit(3, cell);

        }
        else if (group.Count >= 6)
        {
            Spawner.Instance.SpawnSpecialFruit(2, cell);
        }
        else
            return;
    }
    private void SetPauseGame(bool pause)
    {
        this.pause = pause;
    }
}