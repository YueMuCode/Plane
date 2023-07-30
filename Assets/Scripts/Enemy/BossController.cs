using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossController :EnemyController
{
  [Header("---PlayerDetection---")]
  [SerializeField] private Transform playerDetectionTransform;
  [SerializeField] private Vector3 playerDetectionSize;
  [SerializeField] private LayerMask playerLayer;
  [SerializeField] private float continueFireDuration = 1.5f;
  [Header("---Bean---")] 
  [SerializeField]private float beamCoolDownTime = 12f;

  [SerializeField] private AudioData beamChargingSFX;
  [SerializeField] private AudioData beamLaunchSFX;
  private bool isBeamReady;
  private Animator animator;
  private int launchBeamID = Animator.StringToHash("launchBeam");
  
  
  private WaitForSeconds waitForcoutinuousFireInterval;
  private WaitForSeconds waitForFireInterval;
  private WaitForSeconds WaitBeamCooldownTime;//武器的冷却时间
  private List<GameObject> magazine;
  private AudioData launchSFX;
  private Transform playerTransform;
  private void Awake()
  {
    animator = GetComponent<Animator>();
    waitForcoutinuousFireInterval = new WaitForSeconds(minFireInterval);
    waitForFireInterval = new WaitForSeconds(maxFireInterval);
    magazine = new List<GameObject>(projectiles.Length);
    WaitBeamCooldownTime = new WaitForSeconds(beamCoolDownTime);
    playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
  }

  protected override void OnEnable()
  {
    isBeamReady = false;
    muzzleVFX.Stop();
    StartCoroutine(nameof(BeamCooldownCoroutine));
    base.OnEnable();
  }

  private void OnDrawGizmosSelected()
  {
    Gizmos.color=Color.blue;
    Gizmos.DrawWireCube(playerDetectionTransform.position,playerDetectionSize);
  }

  void LoadProjectiles() //装填不同的子弹
  {
    magazine.Clear();
    if (Physics2D.OverlapBox(playerDetectionTransform.position,playerDetectionSize,0f,playerLayer))
    {
      //发射1号子弹
      magazine.Add(projectiles[0]);
      launchSFX = enemyProjectileSFX[0];
    }
    else
    {
      //发射二号子弹或者追踪子弹
      if (Random.Range(0,1f)< 0.5f)
      {
      
        magazine.Add(projectiles[1]);
        launchSFX = enemyProjectileSFX[1];
      }
      else
      {
        for (int i = 2; i < projectiles.Length; i++)
        {
          magazine.Add(projectiles[i]);
        }

        launchSFX = enemyProjectileSFX[1];
      }
    }
  }
  
  //发射
  void ActivateBeamWeapon()
  {
    isBeamReady = false;
    animator.SetTrigger(launchBeamID);
    AudioManager.Instance.PlayRandomSFX(beamChargingSFX);
  }

  void AnimationEventLaunchBeam()
  {
    AudioManager.Instance.PlayRandomSFX(beamLaunchSFX);
  }

  void AnimationEventStopBeam()
  {
    StopCoroutine(nameof(ChasingPlayerCouroutine));
    StartCoroutine(nameof(BeamCooldownCoroutine));
    StartCoroutine(nameof(RandomlyFireCoroutine));
  }
  
  protected override IEnumerator RandomlyFireCoroutine()
  {
    
    while (isActiveAndEnabled)
    {
      if(GameManager.GameState==GameState.GameOver)yield break;
      if (isBeamReady)
      {
        ActivateBeamWeapon();
        StartCoroutine(nameof(ChasingPlayerCouroutine));
        yield break;
      }
      yield return waitForFireInterval;
      yield return StartCoroutine(nameof(ContinuouFireCoroutine));
    }
  }


  IEnumerator ContinuouFireCoroutine()
  {
    LoadProjectiles();
    muzzleVFX.Play();
    float continuousFireTimer = 0f;
    while (continuousFireTimer < continueFireDuration)
    {
      foreach (var projectile in magazine)
      {
        PoolManager.Release(projectile, muzzle.position);
      }

      continuousFireTimer += minFireInterval;
      AudioManager.Instance.PlayRandomSFX(launchSFX);
      yield return waitForcoutinuousFireInterval;
    }
    muzzleVFX.Stop();
  }


  IEnumerator BeamCooldownCoroutine()
  {
    yield return WaitBeamCooldownTime;
    isBeamReady = true;
  }

  IEnumerator ChasingPlayerCouroutine()
  {
    while (isActiveAndEnabled)
    {
      targetposition.x = ViewPort.Instance.MaxX - paddingX;
      targetposition.y = playerTransform.position.y;
      yield return null;
    }
  }
  

}
