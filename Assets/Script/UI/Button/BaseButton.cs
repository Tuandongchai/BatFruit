using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseButton : MonoBehaviour
{
    protected Button button;
    
    protected virtual void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(()=> OnButtonClick());
    }
    protected abstract void OnButtonClick();
}
