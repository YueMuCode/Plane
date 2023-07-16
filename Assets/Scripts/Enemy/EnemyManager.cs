using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : Singleton<EnemyManager>
{
   public int WaveNumber => waveNumber;
   public float TimeBetweenWaves => timeBetweenWaves;
   [SerializeField] private GameObject[] enemyPrefabs;
   [SerializeField] private float timeBetweenSpawns = 1f;
   [SerializeField] private float timeBetweenWaves = 1f;
   [SerializeField] private int minEnemyAmount = 4;
   [SerializeField] private int maxEnemyAmount = 10;
   [SerializeField] private bool spawnEnemy = true;
   [SerializeField] private GameObject waveUI;
   private int waveNumber = 1;
   private int enemyAmount;
   private List<GameObject> enemyList;
   
   
   private WaitForSeconds waitTimeBetweenSpawns;
   private WaitForSeconds waitTimeBetweenWaves;
   private WaitUntil waitUntilNoEnemy;
   
   protected override void Awake()
   {
      base.Awake();
      waitTimeBetweenSpawns = new WaitForSeconds(timeBetweenSpawns);
      enemyList = new List<GameObject>();
      waitUntilNoEnemy = new WaitUntil(NoEnemy);
      waitTimeBetweenWaves = new WaitForSeconds(timeBetweenWaves);
   }

   IEnumerator Start()
   {
      while (spawnEnemy)
      {
         yield return waitUntilNoEnemy;
         waveUI.SetActive(true);
         yield return waitTimeBetweenWaves;
         waveUI.SetActive(false);
         yield return StartCoroutine(nameof(RandomlySpawnCoroutine));
      }
      
   }

   IEnumerator RandomlySpawnCoroutine()
   {
      enemyAmount = Mathf.Clamp(enemyAmount, minEnemyAmount + waveNumber / 3, maxEnemyAmount);
      for (int i = 0; i < enemyAmount; i++)
      {
         enemyList.Add( PoolManager.Release(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]));
         yield return waitTimeBetweenSpawns;
      }

      waveNumber++;
   }

   public void RemoveFromList(GameObject enemy) => enemyList.Remove(enemy);

   bool NoEnemy()
   {
      return enemyList.Count == 0;
   }
}
