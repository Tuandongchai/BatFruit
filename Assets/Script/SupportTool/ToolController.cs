using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolController : MonoBehaviour
{
    [SerializeField] private BombSupportTool bombTool;
    [SerializeField] private RainbowBomb rainbowBomb;
    [SerializeField] private LightningTool lightning;

    public static ToolController instance;
    private void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        BombSupportButton.onClicked += bombTool.Active;
        RainBowBombButton.onClicked += rainbowBomb.Active;
        LightningButton.onClicked += lightning.Active;
    }
    private void OnDisable()
    {
        
        BombSupportButton.onClicked -= bombTool.Active;
        RainBowBombButton.onClicked -= rainbowBomb.Active;
        LightningButton.onClicked -= lightning.Active;
    }
}
