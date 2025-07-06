using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextWaveAnimation : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> textList = new List<TextMeshProUGUI>();
    [SerializeField] private float scaleFactor = 1.5f;
    [SerializeField] private float waveSpeed = 0.1f;
    [SerializeField] private float tweenTime = 0.3f;
    [SerializeField] private float delayBetweenWaves = 5f;

    private void Start()
    {
        StartCoroutine(WaveLoop());
    }

    private IEnumerator WaveLoop()
    {
        while (true)
        {
            AnimateWave();
            yield return new WaitForSeconds(delayBetweenWaves);
        }
    }

    private void AnimateWave()
    {
        for (int i = 0; i < textList.Count; i++)
        {
            TextMeshProUGUI text = textList[i];
            float originalSize = text.fontSize;
            float targetSize = originalSize * scaleFactor;


            LeanTween.value(text.gameObject, originalSize, targetSize, tweenTime)
                .setEase(LeanTweenType.easeOutBack)
                .setDelay(i * waveSpeed)
                .setOnUpdate((float size) => text.fontSize = size)
                .setOnComplete(() =>
                {

                    LeanTween.value(text.gameObject, targetSize, originalSize, tweenTime)
                        .setEase(LeanTweenType.easeInOutBack)
                        .setOnUpdate((float size) => text.fontSize = size);
                });
        }
    }
}
