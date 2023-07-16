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
    [SerializeField] private StatsBar_HUD statsBar_HUD;
  
    
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

    [Header("---Dodge---")] 
    [SerializeField,Range(0,100)] private int dodgeEnergyCost = 25;
    [SerializeField] private float maxRoll = 720f;
    [SerializeField] private float rollSpeed = 360f;
    [SerializeField] private Vector3 dodgeScale = new Vector3(0.5f, 0.5f, 0.5f);
    private Collider2D myCollider;
    private float currentRoll;
    private bool isDodging=false;
    private float dodgeDuration;
    
    private void Awake()
    {
        myrigidbody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
        dodgeDuration = maxRoll / rollSpeed;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        input.onMove += Move;//���¼�
        input.onStopMove += StopMove;
        input.onFire += Fire;
        input.onStopFire += stopFire;
        input.onDodge += Dodge;

    }
    private void OnDisable()
    {
        input.onMove -= Move;
        input.onStopMove -= StopMove;
        input.onFire -= Fire;
        input.onStopFire -= stopFire;
        input.onDodge -= Dodge;
    }
    private void Start()
    {
        statsBar_HUD.Initialize(health,maxHealth);
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
        
        
        StopCoroutine(nameof(MovePositionLimitCoroutine));
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
        statsBar_HUD.UpdateStats(health,maxHealth);
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

    public override void RestoreHealth(float value)
    {
        base.RestoreHealth(value);
        statsBar_HUD.UpdateStats(health,maxHealth);
    }

    public override void Die()
    {
        statsBar_HUD.UpdateStats(0f,maxHealth);
        base.Die();
    }

    #endregion


    #region Dodge

    void Dodge()
    {
        if (isDodging||!PlayerEnergy.Instance.IsEnough(dodgeEnergyCost)) return;
        
        StartCoroutine(nameof(DodgeCoroutine));
    }

    IEnumerator DodgeCoroutine()
    {
        isDodging = true;
        //消耗能量
        PlayerEnergy.Instance.Use(dodgeEnergyCost);
        //闪避
        myCollider.isTrigger = true;
        //翻滚
        currentRoll = 0f;
        var scale = transform.localScale;
        while (currentRoll < maxRoll)
        {
            currentRoll += rollSpeed * Time.deltaTime;
            transform.rotation = Quaternion.AngleAxis(currentRoll, Vector3.right);
            if (currentRoll < maxRoll / 2f)
            {
                scale -= (Time.deltaTime / dodgeDuration) * Vector3.one;
            }
            else
            {
                scale += (Time.deltaTime / dodgeDuration) * Vector3.one;
            }

            transform.localScale = scale;
            yield return null;
        }

        myCollider.isTrigger = false;
        isDodging = false;
    }
    #endregion
}
