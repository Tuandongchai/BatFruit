using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileVerEffect : HandleEffect
{
    [SerializeField] private GameObject missileVer_VFX;
    [SerializeField] private Transform posStart;
    private GameObject go;

    protected override void Start()
    {
        board = FindObjectOfType<Board>();
        Destroy(gameObject, 0.9f);
    }
    public override IEnumerator Active(List<FruitCell> list, Transform trans = null)
    {
        List<FruitCell> cellList = list;
        if (RubikParticle != null)
            SpawnRubikParticle();
        if (cellList.Count == 0)
        {
            yield break;
        }

        cellList.Sort((a, b) => a.GetXY().y.CompareTo(b.GetXY().y));
        posStart = trans;
        yield return StartCoroutine(SpawnVFX(cellList[0].transform, cellList[cellList.Count - 1].transform));

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
        Destroy(go);
    }
    protected override IEnumerator EffectSequence(FruitCell cell, System.Action onComplete)
    {
        yield return StartCoroutine(WaitToDestroy(cell));

        onComplete?.Invoke();
    }
    private IEnumerator SpawnVFX(Transform targetPos1, Transform targetPos2)
    {
        go = Instantiate(missileVer_VFX, posStart.position, Quaternion.identity);
        LeanTween.move(go, targetPos1.position + new Vector3(0, -1.5f, 0), 0.5f)
                 .setEase(LeanTweenType.easeInOutQuad);
        yield return new WaitForSeconds(0.5f);
        LeanTween.move(go, targetPos2.position + new Vector3(0, 4, 0), 0.2f)
                 .setEase(LeanTweenType.easeInOutQuad);
        yield return new WaitForSeconds(0.2f);

    }
}
