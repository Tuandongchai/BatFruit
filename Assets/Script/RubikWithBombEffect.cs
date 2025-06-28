using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubikWithBombEffect : HandleEffect
{
    protected override void Start()
    {
        board = FindObjectOfType<Board>();
        Destroy(gameObject, 1.6f);
    }
    protected override IEnumerator EffectSequence(FruitCell cell, System.Action onComplete)
    {
        yield return StartCoroutine(SpawnLine(this.transform, cell.transform));
        yield return StartCoroutine(SpawnerBomb(cell));
        yield return StartCoroutine(WaitToDestroy(cell));

        onComplete?.Invoke();
    }
    private IEnumerator SpawnerBomb(FruitCell cell)
    {
        /*cell?.GetFruit()?.GetComponent<Fruit>()?.DestroyThis();*/
        GameObject oldFruit = cell?.GetFruit();

        if (oldFruit != null)
        {
            Fruit fruitScript = oldFruit.GetComponent<Fruit>();
            if (fruitScript != null)
            {
                fruitScript.DestroyThis();
            }

            yield return null;
            cell.ChangeFruit(null);
        }
        Spawner.Instance.SpawnSpecialFruit(2, cell);

        yield return null;

    }
}