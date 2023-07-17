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
   [SerializeField] private int minEnemyAmount = 4;//��͵ĵ�������
   [SerializeField] private int maxEnemyAmount = 10;//���ĵ�������
   [SerializeField] private bool spawnEnemy = true;//�Ƿ��Զ����ɵ���
   [SerializeField] private GameObject waveUI;//���˵Ĳ�����ʾUI
   private int waveNumber = 1;//��ǰ����
   private int enemyAmount;//���˵�����
   private List<GameObject> enemyList;//��¼��ǰ���ĵ�������
   
   
   private WaitForSeconds waitTimeBetweenSpawns;//�������ɵļ��ʱ��
   private WaitForSeconds waitTimeBetweenWaves;//����UI��ʾ��ʱ��
   private WaitUntil waitUntilNoEnemy;//֪��û�е���
   
   protected override void Awake()//��ʼ���ø�����Ҫ���Ķ������
   {
      base.Awake();
      waitTimeBetweenSpawns = new WaitForSeconds(timeBetweenSpawns);
      enemyList = new List<GameObject>();
      waitUntilNoEnemy = new WaitUntil(NoEnemy);
      waitTimeBetweenWaves = new WaitForSeconds(timeBetweenWaves);
   }

   IEnumerator Start()//
   {
      while (spawnEnemy)
      {
         yield return waitUntilNoEnemy;//����ֱ����ǰ�����ĵ���Ϊ0
         waveUI.SetActive(true);//�����UI
         yield return waitTimeBetweenWaves;//���𣬼��ʱ��ΪUI�����Ĳ���ʱ��
         waveUI.SetActive(false);//������UI����Ϊ�Ǽ���״̬
         yield return StartCoroutine(nameof(RandomlySpawnCoroutine));
      }
      
   }

   IEnumerator RandomlySpawnCoroutine()//���ɵ���
   {
      enemyAmount = Mathf.Clamp(enemyAmount, minEnemyAmount + waveNumber / 3, maxEnemyAmount);
      for (int i = 0; i < enemyAmount; i++)
      {
         enemyList.Add( PoolManager.Release(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]));//�Ӷ�Ӧ�Ķ������ȡ������
         yield return waitTimeBetweenSpawns;
      }

      waveNumber++;
   }

   public void RemoveFromList(GameObject enemy) => enemyList.Remove(enemy);//����������������������Ƴ���list�����ڼ��㵱ǰ�������ĵ�������

   bool NoEnemy()//�鿴��ǰ���ĵ��������Ƿ�Ϊ��
   {
      return enemyList.Count == 0;
   }
}
