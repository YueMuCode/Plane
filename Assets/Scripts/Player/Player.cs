using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] PlayerInput input;
    public float moveSpeed;//����ƶ��ٶ�
    Rigidbody2D myrigidbody;

    [SerializeField] float PlayerPaddingX=0.3f;//�ɻ��Ļ���߾�
    [SerializeField] float PlayerPaddingY=0.2f;

    //�Ż��ָ�
    [SerializeField] float accelerationTime = 3f;//����ʱ��
    [SerializeField] float decelerationTIme = 3f;//����ʱ��
    private void Awake()
    {
        myrigidbody = GetComponent<Rigidbody2D>();
    }
    void OnEnable()
    {
        input.onMove += Move;//���¼�
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
        input.EnableGameplayInput();//���������ź�
    }
    private void Update()
    {
        
    }
    private void Move(Vector2 moveInput)//���յ�������źź�Ҫִ�е��ƶ��߼�
    {
        myrigidbody.velocity = moveInput * moveSpeed;
        StartCoroutine(MovePositionLimitCoroutine());

    }
    private void StopMove()
    {
        myrigidbody.velocity = Vector2.zero;
        StopCoroutine(MovePositionLimitCoroutine());
    }

    IEnumerator MovePositionLimitCoroutine()//��������������ӿ������ڵ�Э��
    {
        while(true)
        {
            transform.position = ViewPort.Instance.PlayerMoveablePosition(transform.position, PlayerPaddingX, PlayerPaddingY);//������ҵ�λ�����ӿ�֮��
            yield return null;
        }
    }
    IEnumerator MoveCoroutine(Vector2 moveVelocity)//ʵ�������ƶ����ٶȵ�Э��
    {
        float currentTime = 0f;
        while(currentTime<accelerationTime)
        {
            currentTime += Time.fixedDeltaTime/accelerationTime;
            //Vector2.Lerp(myrigidbody.velocity, moveVelocity, t / accelerationTime);
            yield return null;
        }

    }
}
