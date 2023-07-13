using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
[CreateAssetMenu(menuName ="Player Input")]
public class PlayerInput : ScriptableObject,InputActions.IGamePlayActions
{
    InputActions inputActions;
    public event UnityAction<Vector2> onMove=delegate { };
    public event UnityAction onStopMove = delegate { };
    void OnEnable()
    {
        inputActions = new InputActions();
        inputActions.GamePlay.SetCallbacks(this);

    }

    void OnDisable()
    {
        DisableAllInputs();
    }

    public void DisableAllInputs()
    {
        inputActions.GamePlay.Disable();
    }
    
    public void EnableGameplayInput()
    {
        inputActions.GamePlay.Enable();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if(context.phase==InputActionPhase.Performed)//当按键被持续按下时
        {
            if(onMove!=null)
            {
                onMove.Invoke(context.ReadValue<Vector2>());
            }
        }
        if(context.phase==InputActionPhase.Canceled)//停止按下按键的时候
        {
            onStopMove.Invoke();
        } 
    }

  
}
