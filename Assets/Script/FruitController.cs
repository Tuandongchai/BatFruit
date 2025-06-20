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
    [Header("Setup")]
    [SerializeField] private LayerMask fruitLayer;
    [SerializeField] private Camera fruitCamera;
    [SerializeField] private Board fruitBoard;
    [SerializeField] private Spawner spawner;

    private FruitCell firstSelectedCell;
    private FruitCell secondSelectedCell;
    private Vector3 mouseDownWorldPos;

    private bool isMatching = false;

    private void Start()
    {
        /*HandleMatches();*/
    }
    private void Update()
    {
        if (isMatching) return;
        ManagerController();
    }

    private void ManagerController()
    {
        if(Input.GetMouseButtonDown(0))
            ManageMouseDown();
        
        if (Input.GetMouseButtonUp(0))
            ManageMouseUp();
    }

    private void ManageMouseDown()
    {
        firstSelectedCell = GetCellUnderMouse();

        mouseDownWorldPos = GetMouseWorldPosition();
        Debug.Log("Mouse down tai: " + mouseDownWorldPos);


    }

    private void ManageMouseUp()
    {
        Debug.Log(firstSelectedCell);
        if (firstSelectedCell == null) return;


        Vector3 mouseUpWorldPos = GetMouseWorldPosition();
        Debug.Log("Mouse up tai: "+ mouseUpWorldPos);
        Vector3 delta = mouseUpWorldPos - mouseDownWorldPos;
        if (delta.magnitude < 0) return;
        
        Vector2Int direction;

        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            direction = delta.x > 0 ? new Vector2Int(1, 0) : new Vector2Int(-1, 0);
        else
            direction = delta.y > 0 ? new Vector2Int(0, 1) : new Vector2Int(0, -1);

        Vector2 firstPos = firstSelectedCell.GetXY();
        Vector2 secondPos = firstPos + direction;

        Debug.Log("tod do up"+secondPos);
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

        yield return StartCoroutine(HandleFruitSpecial(a,b));
        
        HandleMatches();

        
        StartCoroutine(BackFruit(a, b, tempA, tempB));
    }
    //switch case de
    IEnumerator HandleFruitSpecial(FruitCell a, FruitCell b)
    {
        
        if (a.GetFruitType() == FruitType.Missile_Hor)
        {
            a.transform.GetChild(0)?.GetComponent<Missile>().ActiveEffect(a,b);
            yield return StartCoroutine(WaitToFallAndSpawn());

        }
        else if (b.GetFruitType() == FruitType.Missile_Hor)
        {
            b.transform.GetChild(0)?.GetComponent<Missile>().ActiveEffect(a,b);
            yield return StartCoroutine(WaitToFallAndSpawn());


        }
        else if(a.GetFruitType() == FruitType.Bomb)
        {
            a.transform.GetChild(0)?.GetComponent<Bomb>().ActiveEffect(a,b);
            yield return StartCoroutine(WaitToFallAndSpawn());
        }
        else if (b.GetFruitType() == FruitType.Bomb)
        {
            b.transform.GetChild(0)?.GetComponent<Bomb>().ActiveEffect(a,b);
            yield return StartCoroutine(WaitToFallAndSpawn());


        }
        else if (a.GetFruitType() == FruitType.Missile_Ver)
        {
            a.transform.GetChild(0)?.GetComponent<Missile>().ActiveEffect(a,b);
            yield return StartCoroutine(WaitToFallAndSpawn());

        }
        else if (b.GetFruitType() == FruitType.Missile_Ver)
        {
            b.transform.GetChild(0)?.GetComponent<Missile>().ActiveEffect(a,b);
            yield return StartCoroutine(WaitToFallAndSpawn());
        }
        else if (a.GetFruitType() == FruitType.Rubik)
        {
            a.transform.GetChild(0)?.GetComponent<Rubik>().ActiveEffect(a,b);
            yield return StartCoroutine(WaitToFallAndSpawn());

        }
        else if (b.GetFruitType() == FruitType.Rubik)
        {
            b.transform.GetChild(0)?.GetComponent<Rubik>().ActiveEffect(a,b);
            yield return StartCoroutine(WaitToFallAndSpawn());
        }
    }
    private IEnumerator BackFruit(FruitCell a, FruitCell b, GameObject tempA, GameObject tempB)
    {

        List<List<FruitCell>> matchGroups = MatchChecker.FindMatches(fruitBoard.fruitCells);
        if (matchGroups.Count > 0) yield break;
        
        yield return new WaitForSeconds(0.3f);
        b.ChangeFruit(tempB);
        a.ChangeFruit(tempA);
    }
    IEnumerator WaitToFallAndSpawn()
    {
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
}
