using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject hitFVX;
    [SerializeField] private float damage;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] protected Vector2 moveDirection; 
    protected GameObject target;
    protected virtual void OnEnable()
    {
        StartCoroutine(MoveDirectly());
    }
 
    IEnumerator MoveDirectly()
    {
        while (gameObject.activeSelf) //�������Ǽ���״̬ʱ 
        {
            gameObject.transform.Translate(moveDirection*moveSpeed*Time.deltaTime);
            yield return null;
        }
    }

    #region �����ײ

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Character>(out Character character))
        {
            character.TakeDamage(damage);
            PoolManager.Release(hitFVX, collision.GetContact(0).point, Quaternion.LookRotation(collision.GetContact(0).normal));
            gameObject.SetActive(false);
        }
    }

    #endregion
}
