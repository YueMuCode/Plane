using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    [Header("------MOVE------")]
    [SerializeField] private float paddingX;
    [SerializeField] private float paddingY;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float moveRotationAngle = 25f;
    
    [Header("-----Fire------")]
    [SerializeField] private float minFireInterval;
    [SerializeField] private float maxFireInterval;
    [SerializeField] private GameObject[] projectiles;
    [SerializeField] private Transform muzzle;
    
    private void OnEnable()
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
            Vector3 targetposition = ViewPort.Instance.RandomRightHalfPosition(paddingX, paddingY);
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

    #region 敌人开发

    IEnumerator RandomlyFireCoroutine()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(Random.Range(minFireInterval, maxFireInterval));
            foreach (var projectile in projectiles)
            {
                PoolManager.Release(projectile, muzzle.position);
            }
        }
    }

    #endregion
    
}
