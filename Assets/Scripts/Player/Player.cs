using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] PlayerInput input;
    public float moveSpeed;//玩家移动速度
    Rigidbody2D myrigidbody;

    [SerializeField] float PlayerPaddingX=0.3f;//飞机的机身边距
    [SerializeField] float PlayerPaddingY=0.2f;

    //优化手感
    [SerializeField] float accelerationTime = 3f;//加速时间
    [SerializeField] float decelerationTIme = 3f;//减速时间
    private void Awake()
    {
        myrigidbody = GetComponent<Rigidbody2D>();
    }
    void OnEnable()
    {
        input.onMove += Move;//绑定事件
        input.onStopMove += StopMove;
    }
    private void OnDisable()
    {
        input.onMove -= Move;
        input.onStopMove -= StopMove;
    }
    private void Start()
    {
        myrigidbody.gravityScale = 0f;
        input.EnableGameplayInput();//激活输入信号
    }
    private void Update()
    {
        
    }
    private void Move(Vector2 moveInput)//接收道输入的信号后要执行的移动逻辑
    {
        myrigidbody.velocity = moveInput * moveSpeed;
        StartCoroutine(MovePositionLimitCoroutine());

    }
    private void StopMove()
    {
        myrigidbody.velocity = Vector2.zero;
        StopCoroutine(MovePositionLimitCoroutine());
    }

    IEnumerator MovePositionLimitCoroutine()//用于限制玩家在视口区域内的协程
    {
        while(true)
        {
            transform.position = ViewPort.Instance.PlayerMoveablePosition(transform.position, PlayerPaddingX, PlayerPaddingY);//限制玩家的位置在视口之内
            yield return null;
        }
    }
    IEnumerator MoveCoroutine(Vector2 moveVelocity)//实现物体移动加速度的协程
    {
        float currentTime = 0f;
        while(currentTime<accelerationTime)
        {
            currentTime += Time.fixedDeltaTime/accelerationTime;

            Vector2.Lerp(myrigidbody.velocity, moveVelocity, t / accelerationTime);
            yield return null;
        }

    }
}
