using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubikWithMissileEffect : HandleEffect
{
    protected override void Start()
    {
        board = FindObjectOfType<Board>();
        Destroy(gameObject, 1.6f);
    }
    public override IEnumerator Active(List<FruitCell> list, Transform trans = null, FruitCell fc = null)
    {
        StartCoroutine(base.Active(list, trans, fc));
        AudioManager.Instance.Play2D(audioSO.RubikEffect);
        yield return null;
    }
    protected override IEnumerator EffectSequence(FruitCell cell, System.Action onComplete)
    {
        yield return StartCoroutine(SpawnLine(this.transform, cell.transform));
        yield return StartCoroutine(SpawnerMissile(cell));
        yield return StartCoroutine(WaitToDestroy(cell));

        onComplete?.Invoke();
    }
    private IEnumerator SpawnerMissile(FruitCell cell)
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
        Spawner.Instance.SpawnSpecialFruit(UnityEngine.Random.Range(0, 2), cell);
        
        yield return null;
        
    }
}
