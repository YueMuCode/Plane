using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickUp : LootItem
{
	[SerializeField] private int fullHealthScoreBonus = 200;
	[SerializeField] private float shieldBonus = 20f;
	[SerializeField] private AudioData fullHealthPickUpSFX;
	protected override void PickUp()
	{
		if (player.IsFullHealth)
		{
			pickUpSFX = fullHealthPickUpSFX;
			lootMessage.text = $"·ÖÊý+{fullHealthScoreBonus}";
			ScoreManager.Instance.AddScore(fullHealthScoreBonus);
			
		}
		else
		{
			pickUpSFX = defaultPickUpSFX;
			lootMessage.text = $"»¤¶Ü+{shieldBonus}";
			player.RestoreHealth(shieldBonus);
		}
		base.PickUp();
	}
}
