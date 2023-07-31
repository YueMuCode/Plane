using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LootSpawner : MonoBehaviour
{
   [SerializeField] private LootSetting[] lootSettings;

   public void Spawn(Vector2 position)
   {
      foreach (var lootItem in lootSettings)
      {
         lootItem.Spawn(position+Random.insideUnitCircle);//²úÉúÎ»ÖÃÆ«ÒÆ
      }
   }
}
