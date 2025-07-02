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
        yield return null; // ch? 1 frame ?? ??m b?o layout chính xác

        if (attachPoint == null)
        {
            Debug.LogWarning("AttachPoint ch?a ???c gán.");
            yield break;
        }

        // L?y v? trí màn hình t? attachPoint (dùng camera chính)
        Vector3 screenPos = Camera.main.WorldToScreenPoint(attachPoint.position);

        // Tính v? trí anchored trong canvas
        RectTransform canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        RectTransform rt = GetComponent<RectTransform>();

        Vector2 anchoredPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPos,
            null, // ? Overlay mode: KHÔNG dùng Camera
            out anchoredPos
        );

        rt.anchoredPosition = anchoredPos;

        // Di chuy?n lên
        LeanTween.moveY(rt, rt.anchoredPosition.y + 50f, 0.5f)
                 .setEase(LeanTweenType.easeOutCubic);
    }
}
