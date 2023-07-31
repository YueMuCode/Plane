using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
  [Header("---Reward---")]
  [SerializeField]private int enemyDeathRewardEnergy=3;

  [SerializeField] private int enemyScorePoint = 100;
  [SerializeField] protected int healthFactor;

  private LootSpawner lootSpawner;

  protected virtual void Awake()
  {
    lootSpawner = GetComponent<LootSpawner>();
  }

  protected override void OnEnable()
  {
    SetHealth();
    base.OnEnable();
  }

  protected virtual void OnCollisionEnter2D(Collision2D other)
  {
    if (other.gameObject.TryGetComponent<Player>(out Player player))
    {
      player.Die();
      Die();
    }
  }

  public override void Die()
  {
    ScoreManager.Instance.AddScore(enemyScorePoint);
    PlayerEnergy.Instance.Obtain(enemyDeathRewardEnergy);
    EnemyManager.Instance.RemoveFromList(gameObject);
    lootSpawner.Spawn(transform.position);
    base.Die();
  }

  protected virtual void SetHealth()
  {
    maxHealth += (int)(EnemyManager.Instance.WaveNumber / healthFactor);
  }
}
