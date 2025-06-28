using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyLine : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject,0.7f);
    }

}
