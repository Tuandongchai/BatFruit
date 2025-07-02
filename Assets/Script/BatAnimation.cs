using UnityEngine;

public class BatAnimation : MonoBehaviour
{
    public RectTransform bat;

    void Start()
    {
        if (bat != null)
        {
            LeanTween.moveY(bat, bat.anchoredPosition.y + 50f, 1.5f)
                     .setEase(LeanTweenType.easeInOutQuad)
                     .setLoopPingPong();
            LeanTween.moveX(bat, bat.anchoredPosition.x - 50f, 1.5f)
                    .setEase(LeanTweenType.easeInOutQuad)
                    .setLoopPingPong();


            LeanTween.scale(bat.gameObject, new Vector3(1.05f, 0.95f, 1f), 0.6f)
                     .setEase(LeanTweenType.easeInOutSine)
                     .setLoopPingPong();


            LeanTween.rotateZ(bat.gameObject, 5f, 1f)
                     .setEase(LeanTweenType.easeInOutSine)
                     .setLoopPingPong();
        }
        
    }
}
