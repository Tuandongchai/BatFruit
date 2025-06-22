using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BaseButton : MonoBehaviour
{
    protected Button button;
    public static Action onClicked;
    
    protected virtual void OnEnable()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(()=> OnButtonClick());
    }
    protected virtual void OnButtonClick()
    {
        onClicked?.Invoke();
    }
}
