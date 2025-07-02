using UnityEngine;
using System.Collections;

public class ScorePlusUI : MonoBehaviour
{
    private Transform attachPoint;

    public void Init(Transform _attachPoint)
    {
        attachPoint = _attachPoint;
    }

    public void Effect()
    {
        StartCoroutine(DelayedEffect());
    }

    private IEnumerator DelayedEffect()
    {
        yield return null; // ch? 1 frame ?? ??m b?o layout ch�nh x�c

        if (attachPoint == null)
        {
            Debug.LogWarning("AttachPoint ch?a ???c g�n.");
            yield break;
        }

        // L?y v? tr� m�n h�nh t? attachPoint (d�ng camera ch�nh)
        Vector3 screenPos = Camera.main.WorldToScreenPoint(attachPoint.position);

        // T�nh v? tr� anchored trong canvas
        RectTransform canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        RectTransform rt = GetComponent<RectTransform>();

        Vector2 anchoredPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPos,
            null, // ? Overlay mode: KH�NG d�ng Camera
            out anchoredPos
        );

        rt.anchoredPosition = anchoredPos;

        // Di chuy?n l�n
        LeanTween.moveY(rt, rt.anchoredPosition.y + 50f, 0.5f)
                 .setEase(LeanTweenType.easeOutCubic);
    }
}
