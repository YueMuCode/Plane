using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerProjectileOverdrive :PlayerProjectile
{
    [SerializeField] private ProjectileGuidanceSystem guidanceSystem;
    protected override void OnEnable()
    {
        SetTarget(EnemyManager.Instance.RandomEnemy);
        transform.rotation=quaternion.identity;
        ;
        if (target == null)
        {
            base.OnEnable();
        }
        else
        {
            //×·×ÙÍæ¼Ò
            StartCoroutine(guidanceSystem.HomingCoroutine(target));
        }
    }
}
