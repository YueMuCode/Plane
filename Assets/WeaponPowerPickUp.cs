using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPowerPickUp : LootItem
{
	[SerializeField] private int fullPowerScoreBonus = 200;
	
	[SerializeField] private AudioData fullPowerPickUpSFX;
	protected override void PickUp()
	{
		if (player.IsFullPower)
		{
			pickUpSFX = fullPowerPickUpSFX;
			lootMessage.text = $"·ÖÊý+{fullPowerScoreBonus}";
			ScoreManager.Instance.AddScore(fullPowerScoreBonus);
			
		}
		else
		{
			pickUpSFX = defaultPickUpSFX;
			lootMessage.text = $"ÎäÆ÷Éý¼¶";
			player.PowerUp();
		}
		base.PickUp();
	}
}
