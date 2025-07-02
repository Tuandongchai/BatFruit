using System.Collections;
using UnityEngine;

public class CoroutineRunner : MonoBehaviour
{
    private static CoroutineRunner _instance;

    public static CoroutineRunner Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject runnerObject = new GameObject("CoroutineRunner");
                DontDestroyOnLoad(runnerObject);
                _instance = runnerObject.AddComponent<CoroutineRunner>();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
    public void RunCoroutine(IEnumerator routine)
    {
        StartCoroutine(routine);
    }
}
