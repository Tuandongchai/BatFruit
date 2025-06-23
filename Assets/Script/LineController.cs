using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    private LineRenderer lineRenderer;

    [SerializeField]
    private Texture[] texttures;
    private int animationStep;

    [SerializeField]
    private float fps = 30f;
    private float fpsCounter;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    private void Update()
    {
        fpsCounter += Time.deltaTime;
        if (fpsCounter >= 1f/fps) {
            animationStep++;
            if(animationStep >= texttures.Length) {
                animationStep = 0;
            }

            lineRenderer.material.SetTexture("_MainTex", texttures[animationStep]);

            fpsCounter = 0f;
        }
    }
}
