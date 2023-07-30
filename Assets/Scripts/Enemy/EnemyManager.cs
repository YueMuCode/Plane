using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : Singleton<EnemyManager>
{
   public GameObject RandomEnemy => enemyList.Count == 0 ? null : enemyList[Random.Range(0, enemyList.Count)];
   public int WaveNumber => waveNumber;
   public float TimeBetweenWaves => timeBetweenWaves;
   [SerializeField] private GameObject[] enemyPrefabs;
   [SerializeField] private GameObject bossPrefab;
   [SerializeField] private float timeBetweenSpawns = 1f;
   [SerializeField] private float timeBetweenWaves = 1f;
   [SerializeField] private int minEnemyAmount = 4;//最低的敌人数量
   [SerializeField] private int maxEnemyAmount = 10;//最多的敌人数量
   [SerializeField] private bool spawnEnemy = true;//是否自动生成敌人
   [SerializeField] private GameObject waveUI;//敌人的波数显示UI
   [SerializeField] private int bossWaveNumber;
   private int waveNumber = 1;//当前波数
   private int enemyAmount;//敌人的数量
   private List<GameObject> enemyList;//记录当前存活的敌人数量
   
   
   private WaitForSeconds waitTimeBetweenSpawns;//敌人生成的间隔时间
   private WaitForSeconds waitTimeBetweenWaves;//波数UI显示的时间
   private WaitUntil waitUntilNoEnemy;//知道没有敌人
   
   protected override void Awake()//初始化好各种需要到的对象变量
   {
      base.Awake();
      waitTimeBetweenSpawns = new WaitForSeconds(timeBetweenSpawns);
      enemyList = new List<GameObject>();
      waitUntilNoEnemy = new WaitUntil(NoEnemy);
      waitTimeBetweenWaves = new WaitForSeconds(timeBetweenWaves);
   }

   IEnumerator Start()//
   {
      while (spawnEnemy&&GameManager.GameState!=GameState.GameOver)
      {
         
         waveUI.SetActive(true);//激活波数UI
         yield return waitTimeBetweenWaves;//挂起，间隔时间为UI动画的播放时间
         waveUI.SetActive(false);//将波数UI设置为非激活状态
         yield return StartCoroutine(nameof(RandomlySpawnCoroutine));
      }
      
   }

   IEnumerator RandomlySpawnCoroutine()//生成敌人
   {
      if (waveNumber%bossWaveNumber==0)
      {
         var boss= PoolManager.Release(bossPrefab);
         enemyList.Add(boss);
      }
      else
      {
         enemyAmount = Mathf.Clamp(enemyAmount, minEnemyAmount + waveNumber / bossWaveNumber, maxEnemyAmount);
         for (int i = 0; i < enemyAmount; i++)
         {
            enemyList.Add( PoolManager.Release(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]));//从对应的对象池中取出对象
            yield return waitTimeBetweenSpawns;
         }
       
      }
      yield return waitUntilNoEnemy;//挂起，直到当前波数的敌人为0
      waveNumber++;
   }

   public void RemoveFromList(GameObject enemy) => enemyList.Remove(enemy);//当敌人死亡，将这个对象移除出list，用于计算当前波数存活的敌人数量

   bool NoEnemy()//查看当前存活的敌人数量是否为零
   {
      return enemyList.Count == 0;
   }
}
