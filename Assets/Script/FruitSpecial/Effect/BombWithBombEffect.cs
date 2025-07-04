using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombWithBombEffect : HandleEffect
{
    [SerializeField] private GameObject bombWithBomb_VFX;
    [SerializeField] private Transform transStart;
    [SerializeField] private Vector3 posStart;
    private GameObject go;

    protected override void Start()
    {
        board = FindObjectOfType<Board>();
        Destroy(gameObject, 0.9f);
    }
    public override IEnumerator Active(List<FruitCell> list, Transform trans = null, FruitCell fc = null)
    {
        List<FruitCell> cellList = list;
        if (RubikParticle != null)
            SpawnRubikParticle();
        if (cellList.Count == 0)
        {
            yield break;
        }

        transStart = trans;
        posStart = transStart.position;
        yield return StartCoroutine(SpawnVFX());
       
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

    }
    protected override IEnumerator EffectSequence(FruitCell cell, System.Action onComplete)
    {
        yield return StartCoroutine(WaitToDestroy(cell));

        onComplete?.Invoke();
    }
    private IEnumerator SpawnVFX()
    {
        go = Instantiate(bombWithBomb_VFX, posStart, Quaternion.identity);
        yield return new WaitForSeconds(0.3f);

    }
}