using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
  [Header("---Reward---")]
  [SerializeField]private int enemyDeathRewardEnergy=3;

  public override void Die()
  {
    PlayerEnergy.Instance.Obtain(enemyDeathRewardEnergy);
    EnemyManager.Instance.RemoveFromList(gameObject);
    base.Die();
  }
}
