using System;
using System.Collections;
using System.Collections.Generic;
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
            TrySwap(firstSelectedCell, secondSelectedCell);

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
    private void TrySwap(FruitCell a, FruitCell b)
    {
        GameObject tempA = a.GetFruit();
        GameObject tempB = b.GetFruit();

        a.ChangeFruit(tempB);
        b.ChangeFruit(tempA);

        Debug.Log($"Swapped: {a.GetXY()} <-> {b.GetXY()}");

        HandleMatches();

    }
    IEnumerator WaitToFallAndSpawn()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(spawner.Falling());
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

            List<List<FruitCell>> matchGroups = MatchChecker.FindMatches(fruitBoard.fruitCells);
            if (matchGroups.Count == 0) break;

            isMatching = true;
            foreach (var group in matchGroups)
            {
                foreach (var cell in group)
                {
                    Destroy(cell.GetFruit());
                    cell.ChangeFruit(null);
                }
            }

            yield return new WaitForSeconds(0.2f); 
            yield return StartCoroutine(WaitToFallAndSpawn());
            Debug.Log("Da roi xong check de de quy");
            yield return new WaitForSeconds(5f); 
        }
    }
}
