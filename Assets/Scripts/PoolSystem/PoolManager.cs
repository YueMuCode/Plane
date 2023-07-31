using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
   [SerializeField] private Pool[] playerProjectilePools;
   [SerializeField] private Pool[] enemyProjectilePools;
   [SerializeField] private Pool[] VFXPools;
   [SerializeField] private Pool[] enemyPools;
   [SerializeField] private Pool[] lootItemPools;
   static Dictionary<GameObject, Pool> dictionary;
   private void Awake()
   {
      dictionary = new Dictionary<GameObject, Pool>();
      Initialize(playerProjectilePools);
      Initialize(enemyProjectilePools);
      Initialize(VFXPools);
      Initialize(enemyPools);
      Initialize(lootItemPools);
   }

#if UNITY_EDITOR
   private void OnDestroy()
   {
      CheckPoolSize(playerProjectilePools);
      CheckPoolSize(enemyProjectilePools);
      CheckPoolSize(VFXPools);
      CheckPoolSize(enemyPools);
      CheckPoolSize(lootItemPools);
   }
   
   void CheckPoolSize(Pool[] pools)
   {
      foreach (var pool in pools)
      {
         if (pool.RuntimeSize > pool.Size)
         {
            Debug.LogWarning(
               string.Format("��ǰ�����:{0}����ʱ�Ĵ�С{1}С�ڳ�ʼ����С{2}",
                  pool.Prefab.name,
                  pool.RuntimeSize,
                  pool.Size
               ));
         }
      }
   }
   #endif
   void Initialize(Pool[] pools)//������ش����ֵ��У��Ա�ʹ�õ�ʱ�����ֱ�Ӵ�����������ֱ�ӵ��ó���Ӧ�Ķ���
   {
      foreach (var pool in pools)
      {
         #if UNITY_EDITOR
         if (dictionary.ContainsKey(pool.Prefab))//��ֹ�������ͬ�Ķ����
         {
            Debug.LogError("��ͬ�ĳر���ʼ��"+pool.Prefab.name);
            continue;
         }
         #endif
         dictionary.Add(pool.Prefab,pool);
         Transform poolParent = new GameObject("Pool: " + pool.Prefab.name).transform;
         poolParent.parent = transform;
         pool.Initialize(poolParent);
      }
   }

   public static GameObject Release(GameObject prefab)//���ݴ����Ԥ���壬�����ֵ���ҷ���һ������ʹ�õĶ���
   {
      #if UNITY_EDITOR
      if (!dictionary.ContainsKey(prefab))
      {
         Debug.LogError("û���ҵ���Ӧ�Ķ���");
         return null;
      }
      #endif
      return dictionary[prefab].preparedObject();
   }
   
   
   public static GameObject Release(GameObject prefab,Vector3 position)
   {
#if UNITY_EDITOR
      if (!dictionary.ContainsKey(prefab))
      {
         Debug.LogError("û���ҵ���Ӧ�Ķ���");
         return null;
      }
#endif
      return dictionary[prefab].preparedObject(position);
   }
   
   public static GameObject Release(GameObject prefab,Vector3 position,Quaternion  rotation)
   {
#if UNITY_EDITOR
      if (!dictionary.ContainsKey(prefab))
      {
         Debug.LogError("û���ҵ���Ӧ�Ķ���");
         return null;
      }
#endif
      return dictionary[prefab].preparedObject(position,rotation);
   }

   public static GameObject Release(GameObject prefab,Vector3 position,Quaternion  rotation,Vector3 locaScale)
   {
#if UNITY_EDITOR
      if (!dictionary.ContainsKey(prefab))
      {
         Debug.LogError("û���ҵ���Ӧ�Ķ���");
         return null;
      }
#endif
      return dictionary[prefab].preparedObject(position,rotation,locaScale);
   }
}
