using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubikWithRubikEffect : HandleEffect
{
    protected override void Start()
    {
        board = FindObjectOfType<Board>();
        SpawnRubikWithRubikParticle();
        Destroy(gameObject, 1.6f);
    }
    protected override IEnumerator EffectSequence(FruitCell cell, System.Action onComplete)
    {

        yield return StartCoroutine(WaitToDestroy(cell));

        onComplete?.Invoke();
    }
    
    protected virtual void SpawnRubikWithRubikParticle()
    {
        GameObject rp = Instantiate(RubikWithRubikParticle, Vector3.zero, Quaternion.identity);
        rp.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        rp.transform.position = new Vector3(0, -1.23f, -2.11f);

        rp.transform.SetParent(this.transform);
    }

}
