using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    [Header("------MOVE------")]
    [SerializeField] protected float paddingX;
    [SerializeField] protected float paddingY;
    [SerializeField] protected float moveSpeed = 2f;
    [SerializeField] protected float moveRotationAngle = 25f;
    
    [Header("-----Fire------")]
    [SerializeField] protected float minFireInterval;
    [SerializeField] protected float maxFireInterval;
    [SerializeField] protected GameObject[] projectiles;
    [SerializeField] protected Transform muzzle;
    [SerializeField] protected ParticleSystem muzzleVFX;

    [Header("---Audio---")]
    [SerializeField] protected AudioData[] enemyProjectileSFX;

    protected Vector3 targetposition;
    
    
    protected virtual void OnEnable()
    {
        StartCoroutine(nameof(RandomlyMovingCoroutine));
        StartCoroutine(nameof(RandomlyFireCoroutine));
        
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator RandomlyMovingCoroutine()
    {
        
            transform.position = ViewPort.Instance.RandomEnemySpawnPosition(paddingX, paddingY);
             targetposition = ViewPort.Instance.RandomRightHalfPosition(paddingX, paddingY);
            while (gameObject.activeSelf)
            {
                //未到达目标位置
                if (Vector3.Distance(transform.position, targetposition) > Mathf.Epsilon) //Mathf.Epsilon为无限接近0的数
                {
                    transform.position =Vector3.MoveTowards(transform.position, targetposition, moveSpeed * Time.deltaTime);
                    transform.rotation = Quaternion.AngleAxis((targetposition - transform.position).normalized.y * moveRotationAngle, Vector3.right);
                }
                else
                {
                    targetposition = ViewPort.Instance.RandomRightHalfPosition(paddingX, paddingY);
                }
            
                yield return null;
            }
        
    }

    #region 敌人开火

  protected  virtual IEnumerator RandomlyFireCoroutine()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(Random.Range(minFireInterval, maxFireInterval));
            if(GameManager.GameState==GameState.GameOver)yield break;
            foreach (var projectile in projectiles)
            {
                PoolManager.Release(projectile, muzzle.position);
            }
            AudioManager.Instance.PlayRandomSFX(enemyProjectileSFX);
            muzzleVFX.Play();
        }
    }

    #endregion
    
}
