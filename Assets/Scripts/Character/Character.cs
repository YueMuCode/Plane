using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private GameObject deathVFX;
    [SerializeField] protected float maxHealth;

    protected float health;

    protected virtual void OnEnable()
    {
        health = maxHealth;//初始化生命值为最大值
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            Die();
        }
    }

    public virtual void Die()//死亡
    {
        health = 0f;
        PoolManager.Release(deathVFX, transform.position);
        gameObject.SetActive(false);
    }

    public virtual void RestoreHealth(float value)
    {
        if (health == maxHealth) return;
        health = Mathf.Clamp(health + value, 0, maxHealth);
    }

    public IEnumerator HealthRegenerateCoroutine(WaitForSeconds waitTime, float percent)//缓慢持续回血
    {
        while (health < maxHealth)
        {
            yield return waitTime;
            RestoreHealth(maxHealth*percent);
        }
    }
    
    public IEnumerator DamageOverTimeCoroutine(WaitForSeconds waitTime, float percent)//缓慢持续回血
    {
        while (health>0f)
        {
            yield return waitTime;
            TakeDamage(maxHealth*percent);
        }
    }
}
