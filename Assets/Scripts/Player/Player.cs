using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : Character
{
    [Header("----HP----")]
    [SerializeField] private bool regenerateHealth = true;//是否能够持续回血

    [SerializeField] private float healthRegenerateTime;
    [SerializeField,Range(0f,1f)] private float healthRegeneratePercent;
    [SerializeField] private WaitForSeconds waitHealthRegenerateTime;
    Coroutine healthRegenerateCoroutine;
    
    [Header("---MOVE---")]
    [SerializeField] PlayerInput input;
    public float moveSpeed;//����ƶ��ٶ�
    Rigidbody2D myrigidbody;

    [SerializeField] float PlayerPaddingX=0.3f;//�ɻ��Ļ���߾�
    [SerializeField] float PlayerPaddingY=0.2f;

    [Header("---Fire---")]
    //�Ż��ָ�
    [SerializeField] float accelerationTime = 3f;//加速度
    [SerializeField] float decelerationTIme = 3f;//减速度  
    [SerializeField] float moveRotationAngle = 50f;
    
    //子弹
    [SerializeField] GameObject projectile1;
    [SerializeField] GameObject projectile2;
    [SerializeField] GameObject projectile3;
    [SerializeField] Transform muzzleMid;//生成的位置
    [SerializeField] Transform muzzleUp;//生成的位置
    [SerializeField] Transform muzzleDown;//生成的位置
    [SerializeField] float fireTime=0.2f;
    private WaitForSeconds waitForFireInterval;
    private Coroutine tempCoroutine;//中间变量

    [SerializeField,Range(0,2)] private int weaponPowre = 0;
    
    
    
    
    
    
    
    private void Awake()
    {
        myrigidbody = GetComponent<Rigidbody2D>();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        input.onMove += Move;//���¼�
        input.onStopMove += StopMove;
        input.onFire += Fire;
        input.onStopFire += stopFire;
    }
    private void OnDisable()
    {
        input.onMove -= Move;
        input.onStopMove -= StopMove;
        input.onFire -= Fire;
        input.onStopFire -= stopFire;
    }
    private void Start()
    {
        myrigidbody.gravityScale = 0f;
        input.EnableGameplayInput();//���������ź�
        waitForFireInterval=new WaitForSeconds(fireTime);
        waitHealthRegenerateTime = new WaitForSeconds(healthRegenerateTime);
        TakeDamage(50f);
    }

  

    #region PlayerMove

    

   
    private void Move(Vector2 moveInput)//���յ�������źź�Ҫִ�е��ƶ��߼�
    {
        if (tempCoroutine != null)
        {
            StopCoroutine(tempCoroutine);
        }

        Quaternion moveRotation = Quaternion.AngleAxis(moveRotationAngle * moveInput.y, Vector3.right);
        StartCoroutine(MovePositionLimitCoroutine());
        tempCoroutine= StartCoroutine(MoveCoroutine(accelerationTime,moveInput.normalized * moveSpeed,moveRotation)); 

    }
    private void StopMove()
    {
        if (tempCoroutine != null)
        {
            StopCoroutine(tempCoroutine);
        }
        
        
        StopCoroutine(MovePositionLimitCoroutine());
       tempCoroutine= StartCoroutine(MoveCoroutine(decelerationTIme, Vector2.zero,Quaternion.identity));
    }

    IEnumerator MovePositionLimitCoroutine()//��������������ӿ������ڵ�Э��
    {
        while(true)
        { 
            transform.position = ViewPort.Instance.PlayerMoveablePosition(transform.position, PlayerPaddingX, PlayerPaddingY);//������ҵ�λ�����ӿ�֮��
            yield return null;
        }
    }
    IEnumerator MoveCoroutine(float time,Vector2 moveVelocity,Quaternion moveRotation)//ʵ�������ƶ����ٶȵ�Э��
    {
        float currentTime = 0f;
        while(currentTime<time)
        {
            currentTime += Time.fixedDeltaTime/accelerationTime;
            myrigidbody.velocity= Vector2.Lerp(myrigidbody.velocity, moveVelocity, currentTime / time);
            transform.rotation = Quaternion.Lerp(transform.rotation, moveRotation, currentTime / time);
            yield return null;
        }

    }
    #endregion
 

    #region PlayerFire

    void Fire()
    {
        StartCoroutine("FireCoroutine");
    }

    void stopFire()
    {
         StopCoroutine("FireCoroutine");
        // StopCoroutine(FireCoroutine());//这不会工作,unity引擎的老问题???
    }

    IEnumerator FireCoroutine()
    {
       
        while (true)
        {
            // switch (weaponPowre)
            // {
            //     case 0:
            //         Instantiate(projectile1, muzzleMid.position, quaternion.identity);
            //         break;
            //     case 1:
            //         Instantiate(projectile2, muzzleUp.position, quaternion.identity);
            //         Instantiate(projectile1, muzzleMid.position, quaternion.identity);
            //         break;
            //     case 2:
            //         Instantiate(projectile2, muzzleUp.position, quaternion.identity);
            //         Instantiate(projectile1, muzzleMid.position, quaternion.identity);
            //         Instantiate(projectile3, muzzleDown.position, quaternion.identity);
            //         break;
            //     default:
            //         break;
            // }

            switch (weaponPowre)
            {
                case 0:
                    PoolManager.Release(projectile1, muzzleMid.position);
                    break;
                case 1:
                    PoolManager.Release(projectile1, muzzleMid.position);
                    PoolManager.Release(projectile2, muzzleUp.position);
                    break;
                case 2:
                    PoolManager.Release(projectile1, muzzleMid.position);
                    PoolManager.Release(projectile2, muzzleUp.position);
                    PoolManager.Release(projectile3, muzzleDown.position);
                    break;
                default:
                    break;
            }
            //yield return new WaitForSeconds(fireTime);//尽量不要在while里面new
            yield return waitForFireInterval;
        }
    }
    #endregion


    #region PlayerHPSystem

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (gameObject.activeSelf)
        {
            if (regenerateHealth)
            {
                if (healthRegenerateCoroutine != null)
                {
                    StopCoroutine(healthRegenerateCoroutine);
                }
                healthRegenerateCoroutine = StartCoroutine(HealthRegenerateCoroutine(waitHealthRegenerateTime, healthRegeneratePercent));
            }
        }
    }

    #endregion
}
