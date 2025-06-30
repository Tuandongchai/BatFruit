using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainBowBombButton : BaseButton
{
    public static Action onClicked;
    protected override void OnButtonClick()
    {
        onClicked?.Invoke();
    }

}
