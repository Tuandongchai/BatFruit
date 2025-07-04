using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileWithBombEffect : HandleEffect
{
    [SerializeField] private GameObject missileVer_VFX;
    [SerializeField] private GameObject missileHor_VFX;
    [SerializeField] private Transform posStart;
    [SerializeField] private Vector3 posVStart;
    private GameObject go;
    private GameObject go1;

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

        //cellList.Sort((a, b) => a.GetXY().y.CompareTo(b.GetXY().y));
        // Coordinates to spawn missile
        Vector2? rightMost = null, leftMost = null, topMost = null, bottomMost = null;
        FruitCell rightFruitMost = null, leftFruitMost = null, topFruitMost = null, bottomFruitMost = null;

        foreach (FruitCell v in list)
        {
            if (v.GetXY().y == fc.GetXY().y)
            {
                if (rightMost == null || v.GetXY().x > rightMost.Value.x) rightMost = v.GetXY();
                if (leftMost == null || v.GetXY().x < leftMost.Value.x) leftMost = v.GetXY();
            }
            if (v.GetXY().x == fc.GetXY().x)
            {
                if (topMost == null || v.GetXY().y > topMost.Value.y) topMost = v.GetXY();
                if (bottomMost == null || v.GetXY().y < bottomMost.Value.y) bottomMost = v.GetXY();
            }
        }

        foreach (FruitCell f in cellList)
        {
            if(f.GetXY() == rightMost)
                rightFruitMost= f;
            else if (f.GetXY() == leftMost)
                leftFruitMost= f;
            else if (f.GetXY() == bottomMost)
                bottomFruitMost= f;
            else if(f.GetXY() == topMost)
                topFruitMost= f;
        }
        //
        posStart = trans;
        posVStart = posStart.position;
        yield return StartCoroutine(SpawnVFX(bottomFruitMost.transform, topFruitMost.transform, leftFruitMost.transform, rightFruitMost.transform));

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
        Destroy(go1);
    }
    protected override IEnumerator EffectSequence(FruitCell cell, System.Action onComplete)
    {
        yield return StartCoroutine(WaitToDestroy(cell));

        onComplete?.Invoke();
    }
    private IEnumerator SpawnVFX(Transform bottom, Transform top, Transform left, Transform right)
    {
        go = Instantiate(missileVer_VFX, posVStart, Quaternion.identity);
        LeanTween.move(go, bottom.position + new Vector3(0, -1.5f, 0), 0.5f)
                 .setEase(LeanTweenType.easeInOutQuad);

        go1 = Instantiate(missileHor_VFX, posVStart, Quaternion.identity);
        LeanTween.move(go1, left.position + new Vector3(-1.5f,0, 0), 0.5f)
                 .setEase(LeanTweenType.easeInOutQuad);
        yield return new WaitForSeconds(0.5f);

        LeanTween.move(go, top.position + new Vector3(0, 4, 0), 0.2f)
                 .setEase(LeanTweenType.easeInOutQuad);
        LeanTween.move(go1, right.position + new Vector3(4, 0, 0), 0.2f)
                 .setEase(LeanTweenType.easeInOutQuad);
        yield return new WaitForSeconds(0.2f);
    }
}