using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchChecker : MonoBehaviour
{
    public static List<List<FruitCell>> FindMatches(List<FruitCell> cells)
    {
        Dictionary<Vector2Int, FruitCell> cellMap = new Dictionary<Vector2Int, FruitCell>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        List<List<FruitCell>> matches = new List<List<FruitCell>>();

        // B1: T?o b?n ?? t? t?a ?? -> cell
        foreach (var cell in cells)
        {
            Vector2Int pos = Vector2Int.RoundToInt(cell.GetXY()); // gi? s? GetXY() tr? Vector2
            cellMap[pos] = cell;
        }

        // B2: Duy?t t?t c? cell
        foreach (var kvp in cellMap)
        {
            Vector2Int pos = kvp.Key;

            if (visited.Contains(pos)) continue;

            FruitType type = kvp.Value.GetFruit().GetComponent<Fruit>().type;
            List<FruitCell> group = new List<FruitCell>();

            DFS(pos, type, cellMap, visited, group);

            if (group.Count >= 3)
                matches.Add(group);
        }

        return matches;
    }

    private static void DFS(Vector2Int pos, FruitType type, Dictionary<Vector2Int, FruitCell> map, HashSet<Vector2Int> visited, List<FruitCell> group)
    {
        if (!map.ContainsKey(pos)) return;
        if (visited.Contains(pos)) return;

        var cell = map[pos];
        var fruit = cell.GetFruit().GetComponent<Fruit>();
        if (fruit.type != type) return;

        visited.Add(pos);
        group.Add(cell);

        // 4 h??ng
        DFS(pos + Vector2Int.up, type, map, visited, group);
        DFS(pos + Vector2Int.down, type, map, visited, group);
        DFS(pos + Vector2Int.left, type, map, visited, group);
        DFS(pos + Vector2Int.right, type, map, visited, group);
    }

}
