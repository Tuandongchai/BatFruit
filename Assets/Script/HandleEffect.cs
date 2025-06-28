using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleEffect : MonoBehaviour
{
    [SerializeField] protected Board board;
    [SerializeField] protected GameObject lineRenPrefab;
    [SerializeField] protected GameObject RubikParticle;
    [SerializeField] protected GameObject RubikWithRubikParticle;
    [SerializeField] protected List<FruitCell> cellList=new List<FruitCell>();

    protected virtual void Start()
    {
        board = FindObjectOfType<Board>();
        Destroy(gameObject, 1.1f);
    }

    public virtual IEnumerator Active(List<FruitCell> list)
    {


        List<FruitCell> cellList = list;
        if(RubikParticle != null) 
            SpawnRubikParticle();
        if (cellList.Count == 0)
        {

            yield break;
        }

        int completed = 0;

        foreach (FruitCell cell in cellList)
        {
            StartCoroutine(EffectSequence(cell, () => completed++));
        }
        while (completed < cellList.Count)
        {
            yield return null; 
        }

    }
   /* public IEnumerator ActiveRubikWithMissile(List<FruitCell> cells)
    {
        if (cells.Count == 0)
            yield break;
        foreach (FruitCell cell in cells)
        {
            cell?.GetFruit()?.GetComponent<Fruit>()?.DestroyThis();
            Spawner.Instance.SpawnSpecialFruit(UnityEngine.Random.Range(0, 2), cell);
        }
        yield return new WaitForSeconds(1);
        foreach (FruitCell cell in cells)
            cell?.GetFruit()?.GetComponent<Fruit>()?.DestroyThis();

    }*/
    protected virtual IEnumerator SpawnLine(Transform startPos, Transform endPos)
    {
        GameObject line = Instantiate(lineRenPrefab, Vector2.zero, Quaternion.identity);
        line.GetComponent<LineRenderer>().SetPosition(0, startPos.position);
        line.GetComponent<LineRenderer>().SetPosition(1, endPos.position);

        yield break;
    }
    protected virtual IEnumerator WaitToDestroy(FruitCell c)
    {
        yield return new WaitForSeconds(1f);

        if (c == null) yield break;

        GameObject fruit = c.GetFruit();
        if (fruit == null) yield break;

        Fruit fruitScript = fruit.GetComponent<Fruit>();
        if (fruitScript == null) yield break;

        fruitScript.DestroyThis();
    }
    protected virtual IEnumerator EffectSequence(FruitCell cell, System.Action onComplete)
    {
        
        yield return StartCoroutine(SpawnLine(this.transform, cell.transform));
        yield return StartCoroutine(WaitToDestroy(cell));

        onComplete?.Invoke();
    }
    protected virtual void SpawnRubikParticle()
    {
        GameObject rp = Instantiate(RubikParticle, Vector3.zero, Quaternion.identity);
        rp.transform.SetParent(this.transform);
        rp.transform.localPosition = Vector3.zero;
    }
    
}
