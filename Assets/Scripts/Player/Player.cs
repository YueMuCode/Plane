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
    [SerializeField] private GameObject projectileOverDrive;
    [SerializeField] Transform muzzleMid;//生成的位置
    [SerializeField] Transform muzzleUp;//生成的位置
    [SerializeField] Transform muzzleDown;//生成的位置
    [SerializeField] float fireTime=0.2f;
    private WaitForSeconds waitForFireInterval;
    private WaitForSeconds waitForOverdriveFireInterval;
    private WaitForSeconds waitDecelerationTime;
    private Coroutine tempCoroutine;//中间变量
    [SerializeField,Range(0,2)] private int weaponPowre = 0;

    [Header("---Dodge---")] 
    [SerializeField,Range(0,100)] private int dodgeEnergyCost = 25;
    [SerializeField] private float maxRoll = 720f;
    [SerializeField] private float rollSpeed = 360f;
    [SerializeField] private Vector3 dodgeScale = new Vector3(0.5f, 0.5f, 0.5f);

    [Header("---Audio----")] [SerializeField]
    private AudioData[] projectileLauchSFX;
    [SerializeField] private AudioData[] dodgeSFX;

    [Header("---OverDrive---")] 
    [SerializeField] private int overdriveDodgeFactor = 2;
    [SerializeField] private float overdriveSpeedFactor = 1.2f;
    [SerializeField] private float overdriveFireFactor = 1.2f;
    private Collider2D myCollider;
    private float currentRoll;
    private bool isDodging=false;
    private float dodgeDuration;
    private bool isOverDriving = false;
    private Vector2 tempVelocity;
    private Quaternion temprotation;

    private readonly float slowMotionDuration=1f;

    private MIssilleSystem missile;
    private Vector2 moveDiretione;
    
    
    private void Awake()
    {
        myrigidbody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
        missile = GetComponent<MIssilleSystem>();
        dodgeDuration = maxRoll / rollSpeed;
        myrigidbody.gravityScale = 0f;
        waitForFireInterval=new WaitForSeconds(fireTime);
        waitHealthRegenerateTime = new WaitForSeconds(healthRegenerateTime);
        waitForOverdriveFireInterval = new WaitForSeconds(fireTime /= overdriveDodgeFactor);
        waitDecelerationTime = new WaitForSeconds(decelerationTIme);
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        input.onMove += Move;//���¼�
        input.onStopMove += StopMove;
        input.onFire += Fire;
        input.onStopFire += stopFire;
        input.onDodge += Dodge;
        input.onOverdrive += OverDrive;
        input.onLanuchMissle += LaunchMissile;

        PlayerOverDrive.on += OverDriveOn;
        PlayerOverDrive.off += OverDriveOff;

    }
    private void OnDisable()
    {
        input.onMove -= Move;
        input.onStopMove -= StopMove;
        input.onFire -= Fire;
        input.onStopFire -= stopFire;
        input.onDodge -= Dodge;
        input.onLanuchMissle -= LaunchMissile;
        PlayerOverDrive.on -= OverDriveOn;
        PlayerOverDrive.off -= OverDriveOff;
    }
    private void Start()
    {
        statsBar_HUD.Initialize(health,maxHealth);
     
        input.EnableGameplayInput();//���������ź�
       
       // TakeDamage(50f);
    }

  

    #region PlayerMove

    

   
    private void Move(Vector2 moveInput)//���յ�������źź�Ҫִ�е��ƶ��߼�
    {
        if (tempCoroutine != null)
        {
            StopCoroutine(tempCoroutine);
        }

        moveDiretione = moveInput.normalized;
        Quaternion moveRotation = Quaternion.AngleAxis(moveRotationAngle * moveInput.y, Vector3.right);
        StopCoroutine(nameof(DecelerationCoroutine));
        StartCoroutine(MovePositionLimitCoroutine());
        
        tempCoroutine= StartCoroutine(MoveCoroutine(accelerationTime,moveDiretione * moveSpeed,moveRotation)); 

    }
    private void StopMove()
    {
        if (tempCoroutine != null)
        {
            StopCoroutine(tempCoroutine);
        }
        
        
        StopCoroutine(nameof(MovePositionLimitCoroutine));
        tempCoroutine= StartCoroutine(MoveCoroutine(decelerationTIme, Vector2.zero,Quaternion.identity));
        StartCoroutine(nameof(DecelerationCoroutine));
    }

    IEnumerator MovePositionLimitCoroutine()//��������������ӿ������ڵ�Э��
    {
        while(true)
        { 
            transform.position = ViewPort.Instance.PlayerMoveablePosition(transform.position, PlayerPaddingX, PlayerPaddingY);//������ҵ�λ�����ӿ�֮��
            yield return null;
        }
    }

    IEnumerator DecelerationCoroutine()
    {
        yield return waitDecelerationTime;
        StopCoroutine(nameof(MovePositionLimitCoroutine));
        
    }
    IEnumerator MoveCoroutine(float time,Vector2 moveVelocity,Quaternion moveRotation)//ʵ�������ƶ����ٶȵ�Э��
    {
        float currentTime = 0f;
        tempVelocity = myrigidbody.velocity;
        temprotation = transform.rotation;
        while(currentTime<time)
        {
            currentTime += Time.fixedDeltaTime/accelerationTime;
            myrigidbody.velocity= Vector2.Lerp(tempVelocity, moveVelocity, currentTime / time);
            transform.rotation = Quaternion.Lerp(temprotation, moveRotation, currentTime / time);
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
    
            switch (weaponPowre)
            {
                case 0:
                    PoolManager.Release(isOverDriving? projectileOverDrive: projectile1, muzzleMid.position);
                    break;
                case 1:
                    PoolManager.Release(isOverDriving? projectileOverDrive: projectile1, muzzleMid.position);
                    PoolManager.Release(isOverDriving? projectileOverDrive: projectile2, muzzleUp.position);
                    break;
                case 2:
                    PoolManager.Release(isOverDriving? projectileOverDrive: projectile1, muzzleMid.position);
                    PoolManager.Release(isOverDriving? projectileOverDrive: projectile2, muzzleUp.position);
                    PoolManager.Release(isOverDriving? projectileOverDrive: projectile3, muzzleDown.position);
                    break;
                default:
                    break;
            }
            AudioManager.Instance.PlayRandomSFX(projectileLauchSFX);
            //yield return new WaitForSeconds(fireTime);//尽量不要在while里面new
           // yield return waitForFireInterval;
           if (isOverDriving)
           {
               yield return waitForOverdriveFireInterval;
           }
           else
           {
               yield return waitForFireInterval;
           }
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
            Move(moveDiretione);
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
        GameManager.onGameOver?.Invoke();
        GameManager.GameState = GameState.GameOver;
        statsBar_HUD.UpdateStats(0f,maxHealth);
        base.Die();
    }

    #endregion


    #region Dodge

    void Dodge()
    {
        if (isDodging||!PlayerEnergy.Instance.IsEnough(dodgeEnergyCost)) return;
        AudioManager.Instance.PlayRandomSFX(dodgeSFX);
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

    #region OverDrive

    void OverDrive()
    {
        if (!PlayerEnergy.Instance.IsEnough(PlayerEnergy.MAX)) return;
        
        PlayerOverDrive.on.Invoke();
    }

    void OverDriveOn()
    {
        isOverDriving = true;
        dodgeEnergyCost *= overdriveDodgeFactor;
        moveSpeed *= overdriveSpeedFactor;
        TimeController.Instance.BulletTime(slowMotionDuration,5f,slowMotionDuration);
    }

    void OverDriveOff()
    {
        isOverDriving = false;
        dodgeEnergyCost /= overdriveDodgeFactor;
        moveSpeed /= overdriveSpeedFactor;
    }

    #endregion


    void LaunchMissile()
    {
        missile.Launch(muzzleMid);
    }
}
