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
    
    public event UnityAction onFire=delegate {  };
    public event UnityAction onStopFire=delegate {  }; 
    
    public event UnityAction onDodge=delegate {  };

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
        if(context.phase==InputActionPhase.Performed)//����������������ʱ
        {
            if(onMove!=null)
            {
                onMove.Invoke(context.ReadValue<Vector2>());
            }
        }
        if(context.phase==InputActionPhase.Canceled)//ֹͣ���°�����ʱ��
        {
            onStopMove.Invoke();
        } 
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if(context.phase==InputActionPhase.Performed)//����������������ʱ
        {
           onFire.Invoke();
           
        }
        if(context.phase==InputActionPhase.Canceled)//ֹͣ���°�����ʱ��
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
}
