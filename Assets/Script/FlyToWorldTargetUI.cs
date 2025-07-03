using UnityEngine;

public class FlyToWorldTargetUI : MonoBehaviour
{
    public RectTransform uiObject;    
    public Transform worldTarget;    
    public Camera cam;

    /*void Start()
    {
        if (cam == null) cam = Camera.main;
        FlyToWorldUI();
    }*/

    public void FlyToWorldUI()
    {
        // position of gameObject form world convert to UI position
        Vector2 screenPos = cam.WorldToScreenPoint(worldTarget.position);
        Vector2 startPos = uiObject.position;

        //
        Vector2 controlPoint = (startPos + screenPos) / 2 + new Vector2(Random.Range(-100f, 100f), Random.Range(50f, 150f));

        LeanTween.value(0f, 1f, 1f)
            .setOnUpdate((float t) =>
            {
                Vector2 p0 = startPos;
                Vector2 p1 = controlPoint;
                Vector2 p2 = screenPos;

                Vector2 curved = Mathf.Pow(1 - t, 2) * p0
                               + 2 * (1 - t) * t * p1
                               + Mathf.Pow(t, 2) * p2;

                uiObject.position = curved;
            })
            .setEase(LeanTweenType.easeInOutCubic)
            .setOnComplete(() =>
            {
                Debug.Log("complete!");
            });
    }
}
