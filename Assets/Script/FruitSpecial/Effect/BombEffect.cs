using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEffect : HandleEffect
{
    [SerializeField] private GameObject bomb_VFX;
    [SerializeField] private GameObject bomb;
    private Transform transStart;
    private Vector3 posStart;
    private GameObject go;

    protected override void Start()
    {
        board = FindObjectOfType<Board>();
        Destroy(gameObject, 1.2f);
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
        Destroy(go);
    }
    protected override IEnumerator EffectSequence(FruitCell cell, System.Action onComplete)
    {
        yield return StartCoroutine(WaitToDestroy(cell));

        onComplete?.Invoke();
    }
    private IEnumerator SpawnVFX()
    {
        go = Instantiate(bomb, posStart, Quaternion.identity);

        int shakeTweenId = LeanTween.rotateZ(go, 50f, 0.1f)
                    .setEase(LeanTweenType.easeInOutSine)
                    .setLoopPingPong().uniqueId;
        LeanTween.scale(go, Vector3.one * 0.35f, 0.5f).setEase(LeanTweenType.easeOutBack);
        yield return new WaitForSeconds(0.5f);
        LeanTween.cancel(shakeTweenId);
        LeanTween.scale(go, Vector3.zero, 0.3f).setEase(LeanTweenType.easeOutBack);
        yield return new WaitForSeconds(0.6f);
        Instantiate(bomb_VFX, posStart, Quaternion.identity);

    }
}
