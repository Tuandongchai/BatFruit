using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportToolUI : MonoBehaviour
{
    [Header("ButtonUI")]
    [SerializeField] private RectTransform bombTool, rainbowTool, lightning;

    private void Start()
    {
        BombSupportButton.onClicked += BombBtnUIStartAnimation;
        RainBowBombButton.onClicked += RainbowBtnUIStartAnimation;
        LightningButton.onClicked += lightningBtnUIStartAnimation;
    }
    private void BombBtnUIStartAnimation()
    {
        /*LeanTween.value(bombTool.gameObject, bombTool.sizeDelta, bombTool.sizeDelta * 1.8f, 0.5f)
                     .setEase(LeanTweenType.easeOutBack)
                     .setOnUpdate((Vector2 val) => bombTool.sizeDelta = val);*/
        LeanTween.scale(bombTool, new Vector3(1.8f, 1.8f, 1.8f), 0.5f).setEase(LeanTweenType.easeOutBack);
    }
    public void BombBtnUIEndAnimation()
    {

        LeanTween.scale(bombTool, new Vector3(1f, 1f, 1f), 0.5f).setEase(LeanTweenType.easeOutBack);
    }
    private void RainbowBtnUIStartAnimation()
    {
        LeanTween.scale(rainbowTool, new Vector3(1.8f, 1.8f, 1.8f), 0.5f).setEase(LeanTweenType.easeOutBack);

    }
    public void RainbowBtnUIEndAnimation()
    {
        LeanTween.scale(rainbowTool, new Vector3(1f, 1f, 1f), 0.5f).setEase(LeanTweenType.easeOutBack);

    }
    private void lightningBtnUIStartAnimation()
    {
        LeanTween.scale(lightning, new Vector3(1.8f, 1.8f, 1.8f), 0.5f).setEase(LeanTweenType.easeOutBack);
    }
    public void lightningBtnUIEndAnimation()
    {
        LeanTween.scale(lightning, new Vector3(1f, 1f, 1f), 0.5f).setEase(LeanTweenType.easeOutBack);
    }
}
