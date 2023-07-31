using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBonusPickUp :LootItem
{
   [SerializeField] private int scoreBonus;
   protected override void PickUp()
   {
      ScoreManager.Instance.AddScore(scoreBonus);
      base.PickUp();
   }
}
