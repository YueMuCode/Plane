using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class PlayerMissile : PlayerProjectileOverdrive
{
    [SerializeField] private AudioData targetAcquireVoice = null;
    [SerializeField]private float lowSpeed = 8f;
    [SerializeField]private float highSpeed = 25f;
    [SerializeField]private float variableSpeedDylay = 0.5f;
    private WaitForSeconds waitVariableSpeedDelay;

    [Header("---Explosion---")] 
    [SerializeField] private GameObject explosionVFX = null;

    [SerializeField] private AudioData explosionSFX = null;
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private LayerMask enemyLayerMask=default;
    [SerializeField] private float explosionDamage = 100f;//±¨’®…À∫¶
    private protected override void Awake()
    {
        base.Awake();
        waitVariableSpeedDelay = new WaitForSeconds(variableSpeedDylay);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(nameof(VariableSpeedCoroutine));
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        //»√◊”µØ±¨’®
        //“Ù–ß
        //Ãÿ–ß
        //…À∫¶
        PoolManager.Release(explosionVFX, transform.position);
        AudioManager.Instance.PlayRandomSFX(explosionSFX);
       var colliders= Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayerMask);
       foreach (var collider in colliders)
       {
           if (collider.TryGetComponent<Enemy>(out Enemy enemy))
           {
               enemy.TakeDamage(explosionDamage);
           }
       }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,explosionRadius);
    }

    IEnumerator VariableSpeedCoroutine()
    {
        moveSpeed = lowSpeed;
        yield return waitVariableSpeedDelay;
        moveSpeed = highSpeed;
        if (target != null)
        {
            AudioManager.Instance.PlayRandomSFX(targetAcquireVoice);
        }
    }
}
