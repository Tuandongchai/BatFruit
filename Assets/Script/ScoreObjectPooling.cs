using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreObjectPooling : MonoBehaviour
{
    public GameObject scorePrefab;     
    public int initialSize = 10;

    private Queue<GameObject> pool = new Queue<GameObject>();

    void Start()
    {
        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = Instantiate(scorePrefab, transform);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject Spawn(Vector3 position, out GameObject obj)
    {
        //GameObject obj;

        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
        }
        else
        {
            obj = Instantiate(scorePrefab, transform);
        }

        obj.SetActive(true);
        obj.transform.position = position;

        StartCoroutine(ReleaseAfterSeconds(obj, 0.5f));

        return obj;
    }

    private System.Collections.IEnumerator ReleaseAfterSeconds(GameObject obj, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
