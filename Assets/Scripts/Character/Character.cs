using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private GameObject deathVFX;
    [SerializeField] protected float maxHealth;
    [Header("---HealthBar---")] 
    [SerializeField] private StatsBar onHeadHealthBar;
    [SerializeField] private bool showOnHeadHealthBar = true;

    [Header("---Audio---")] 
    [SerializeField] private AudioData[] deathSFX;
 
    protected float health;

    protected virtual void OnEnable()
    {
        health =maxHealth;//��ʼ������ֵΪ���ֵ
        if (showOnHeadHealthBar)
        {
            ShowOnHeadHealthBar();
        }
        else
        {
            HideOnHeadHealthBar();
        }
    }

    public virtual void TakeDamage(float damage)
    {
        if (health == 0) return;
        health -= damage;
        if (showOnHeadHealthBar&&gameObject.activeSelf)
        {
            onHeadHealthBar.UpdateStats(health,maxHealth);
        }
        
        
        if (health <= 0f)
        {
            Die();
        }
    }

    public virtual void Die()//����
    {
        health = 0f;
        PoolManager.Release(deathVFX, transform.position);
        AudioManager.Instance.PlayRandomSFX(deathSFX);
        gameObject.SetActive(false);
    }

    public virtual void RestoreHealth(float value)
    {
        if (health == maxHealth) return;
        health = Mathf.Clamp(health + value, 0, maxHealth);
        if (showOnHeadHealthBar)
        {
            onHeadHealthBar.UpdateStats(health,maxHealth);
        }
    }

    public IEnumerator HealthRegenerateCoroutine(WaitForSeconds waitTime, float percent)//����������Ѫ
    {
        while (health < maxHealth)
        {
            yield return waitTime;
            RestoreHealth(maxHealth*percent);
        }
    }
    
    public IEnumerator DamageOverTimeCoroutine(WaitForSeconds waitTime, float percent)//����������Ѫ
    {
        while (health>0f)
        {
            yield return waitTime;
            TakeDamage(maxHealth*percent);
        }
    }
    #region Ѫ��

    public void ShowOnHeadHealthBar()
    {
        onHeadHealthBar.gameObject.SetActive(true);
        onHeadHealthBar.Initialize(health,maxHealth);
    }

    public void HideOnHeadHealthBar()
    {
        onHeadHealthBar.gameObject.SetActive(false);
    }

    #endregion
}
