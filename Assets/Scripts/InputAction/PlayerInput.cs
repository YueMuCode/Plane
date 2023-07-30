using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Player Input")]
public class PlayerInput : ScriptableObject, InputActions.IGamePlayActions, InputActions.IPauseMenuActions,
    InputActions.IGameOverScreenActions
{
    InputActions inputActions;
    public event UnityAction<Vector2> onMove = delegate { };
    public event UnityAction onStopMove = delegate { };

    public event UnityAction onFire = delegate { };
    public event UnityAction onStopFire = delegate { };

    public event UnityAction onDodge = delegate { };

    public event UnityAction onOverdrive = delegate { };

    public event UnityAction onPause = delegate { };

    public event UnityAction onUnPause = delegate { };
    public event UnityAction onLanuchMissle = delegate { };
    public event UnityAction onConfirmGameOver = delegate { };

    void OnEnable()
    {
        inputActions = new InputActions();
        inputActions.GamePlay.SetCallbacks(this);
        inputActions.PauseMenu.SetCallbacks(this);
        inputActions.GameOverScreen.SetCallbacks(this);

    }

    void OnDisable()
    {
        DisableAllInputs();
    }

    void SwitchActionMap(InputActionMap actionMap, bool isUIInput)
    {
        inputActions.Disable();
        actionMap.Enable();
        if (isUIInput)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void SwitchToDynamicUpdateMode()
    {
        InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;

    }

    public void SwitchToFixedUpdateMode()
    {
        InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;
    }


    public void DisableAllInputs()
    {
        inputActions.GamePlay.Disable();
        inputActions.PauseMenu.Disable();
    }

    public void EnableGameplayInput()
    {
        SwitchActionMap(inputActions.GamePlay, false);
    }

    public void EnablePauseMenuInput()
    {
        SwitchActionMap(inputActions.PauseMenu, true);
    }

    public void EnableGameOverScreenInput()
    {
        SwitchActionMap(inputActions.GameOverScreen,false);
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

    public void OnFire(InputAction.CallbackContext context)
    {
        if(context.phase==InputActionPhase.Performed)//当按键被持续按下时
        {
           onFire.Invoke();
           
        }
        if(context.phase==InputActionPhase.Canceled)//停止按下按键的时候
        {
            onStopFire.Invoke();
        } 
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onDodge.Invoke();
        }
    }

    public void OnOverDrive(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onOverdrive.Invoke();
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onPause.Invoke();
        }
    }

    public void OnLanuchMissile(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onLanuchMissle.Invoke();
        }
    }

    public void OnUnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onUnPause.Invoke();
        }
    }


    public void OnConfirmGameOver(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onConfirmGameOver.Invoke();
        }
    }
}
