using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject hitFVX;
    [SerializeField] private float damage;
    [SerializeField] protected float moveSpeed = 10f;
    [SerializeField] protected Vector2 moveDirection;

    [SerializeField] private AudioData []hitSFX;
    protected GameObject target;
    protected virtual void OnEnable()
    {
        StartCoroutine(MoveDirectly());
    }
 
    IEnumerator MoveDirectly()
    {
        while (gameObject.activeSelf) //当对象是激活状态时 
        {
           Move();
            yield return null;
        }
    }

    public void Move()
    {
        gameObject.transform.Translate(moveDirection*moveSpeed*Time.deltaTime);
    }
    #region 检测碰撞

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Character>(out Character character))
        {
            character.TakeDamage(damage);
            PoolManager.Release(hitFVX, collision.GetContact(0).point, Quaternion.LookRotation(collision.GetContact(0).normal));
            AudioManager.Instance.PlayRandomSFX(hitSFX);
            gameObject.SetActive(false);
            
        }
    }

    #endregion

    protected void SetTarget(GameObject target)
    {
        this.target = target;
    }
    
}
