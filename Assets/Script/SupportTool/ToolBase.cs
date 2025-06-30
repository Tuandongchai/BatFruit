using System.Collections;
using UnityEngine;

public abstract class ToolBase : MonoBehaviour
{
    [SerializeField] protected Camera fruitCamera;
    [SerializeField] protected FruitCell cellChoose;
    [SerializeField] protected Board board;
    [SerializeField] protected Spawner spawner;
    [SerializeField] protected SupportToolUI toolUI;
    [SerializeField] protected bool isActive = false;
    private bool hasActivatedEffect = false;

    protected virtual void Start()
    {
        board = GameObject.FindObjectOfType<Board>();
        spawner = GameObject.FindObjectOfType<Spawner>();
        toolUI = GameObject.FindObjectOfType<SupportToolUI>();
    }

    public virtual void Active()
    {
        if (FruitController.instance.isMatching)
            return;

        FruitController.instance.isMatching = true;
        isActive = true;
        hasActivatedEffect = false;
    }

    protected virtual void Update()
    {
        if (!isActive)
            return;

        MouseDown(); 

        if (cellChoose != null && !hasActivatedEffect)
        {
            hasActivatedEffect = true;
            StartCoroutine(ActiveEffect());
        }
    }

    public virtual IEnumerator ActiveEffect()
    {
        yield break;
    }

    protected virtual void MouseDown()
    {
        cellChoose = GetCellUnderMouse();
    }

    protected FruitCell GetCellUnderMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = fruitCamera.ScreenToWorldPoint(mousePos);
        worldPos.z = 0f;

        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

        if (hit.collider != null)
            return hit.collider.GetComponent<FruitCell>();
        return null;
    }
}
