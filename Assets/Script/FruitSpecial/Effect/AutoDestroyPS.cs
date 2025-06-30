using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyPS : MonoBehaviour
{
    private ParticleSystem ps;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        StartCoroutine(WaitAndDestroy());
    }
    IEnumerator WaitAndDestroy()
    {
        yield return new WaitUntil(() => !ps.IsAlive(true));
        Destroy(gameObject);
    }
}
