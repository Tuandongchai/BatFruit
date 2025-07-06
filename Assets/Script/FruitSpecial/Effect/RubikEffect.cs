using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubikEffect : HandleEffect
{
/*    [SerializeField] private AudioSO aaudioSO;*/
    public override IEnumerator Active(List<FruitCell> list, Transform trans = null, FruitCell fc = null)
    {
        StartCoroutine(base.Active(list, trans, fc));
        AudioManager.Instance.Play2D(this.audioSO.RubikEffect);
        yield return null;
    }
}