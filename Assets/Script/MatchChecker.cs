using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchChecker : MonoBehaviour
{
   public static List<List<FruitCell>> FindMatches(List<FruitCell> cells)
    {
        Dictionary<Vector2Int, FruitCell> cellMap = new Dictionary<Vector2Int, FruitCell>();
        HashSet<FruitCell> matchedCells = new HashSet<FruitCell>();
        List<List<FruitCell>> matches = new List<List<FruitCell>>();

        foreach (var cell in cells)
        {
            Vector2Int pos = Vector2Int.RoundToInt(cell.GetXY());
            cellMap[pos] = cell;
        }


        foreach (var kvp in cellMap)
        {
            Vector2Int pos = kvp.Key;
            FruitCell startCell = kvp.Value;

            

            Fruit fruit = startCell?.GetFruit()?.GetComponent<Fruit>();
            if (fruit == null) continue;

            List<FruitCell> horizontalMatch = new List<FruitCell> { startCell };

            for (int i = 1; i < 6; i++)
            {
                Vector2Int nextPos = pos + new Vector2Int(i, 0);
                if (!cellMap.TryGetValue(nextPos, out var nextCell)) break;

                var nextFruit = nextCell?.GetFruit()?.GetComponent<Fruit>();
                
                if (nextFruit == null || nextFruit.type != fruit.type) break;

                horizontalMatch.Add(nextCell);
            }

            if (horizontalMatch.Count >= 3)
            {
                foreach (var c in horizontalMatch)
                    matchedCells.Add(c);
                matches.Add(horizontalMatch);
            }
        }


        foreach (var kvp in cellMap)
        {
            Vector2Int pos = kvp.Key;
            FruitCell startCell = kvp.Value;
            Fruit fruit = startCell.GetFruit()?.GetComponent<Fruit>();
            if (fruit == null) continue;

            List<FruitCell> verticalMatch = new List<FruitCell> { startCell };

            for (int i = 1; i < 6; i++)
            {
                Vector2Int nextPos = pos + new Vector2Int(0, i);
                if (!cellMap.TryGetValue(nextPos, out var nextCell)) break;

                var nextFruit = nextCell.GetFruit()?.GetComponent<Fruit>();
                if (nextFruit == null || nextFruit.type != fruit.type) break;

                verticalMatch.Add(nextCell);
            }

            if (verticalMatch.Count >= 3)
            {
                foreach (var c in verticalMatch)
                    matchedCells.Add(c);
                matches.Add(verticalMatch);
            }
        }

       
        List<List<FruitCell>> finalGroups = MergeOverlappingMatches(matches);

        return finalGroups;
    }

    private static List<List<FruitCell>> MergeOverlappingMatches(List<List<FruitCell>> rawGroups)
    {
        List<List<FruitCell>> merged = new List<List<FruitCell>>();
        HashSet<FruitCell> visited = new HashSet<FruitCell>();

        foreach (var group in rawGroups)
        {
            if (group.Exists(cell => visited.Contains(cell)))
            {
                foreach (var existingGroup in merged)
                {
                    if (group.Exists(cell => existingGroup.Contains(cell)))
                    {
                        foreach (var cell in group)
                        {
                            if (!existingGroup.Contains(cell))
                                existingGroup.Add(cell);
                            visited.Add(cell);
                        }
                        break;
                    }
                }
            }
            else
            {
                merged.Add(new List<FruitCell>(group));
                foreach (var cell in group)
                    visited.Add(cell);
            }
        }

        return merged;
    }
}
