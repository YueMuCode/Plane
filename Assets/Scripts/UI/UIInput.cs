using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
public class UIInput : Singleton<UIInput>
{
    [SerializeField] private PlayerInput playerInput;
    private InputSystemUIInputModule UiInputModule;
    protected override void Awake()
    {
        base.Awake();
        UiInputModule = GetComponent<InputSystemUIInputModule>();
        UiInputModule.enabled = false;
    }

    public void SelectUI(Selectable UIObject)
    {
        UIObject.Select();
        UIObject.OnSelect(null);
        UiInputModule.enabled = true;
    }

    public void DisableAllInputs()
    {
        playerInput.DisableAllInputs();
        UiInputModule.enabled = false;
    }
}
