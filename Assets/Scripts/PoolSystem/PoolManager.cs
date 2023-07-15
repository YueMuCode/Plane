using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
   [SerializeField] private Pool[] playerProjectilePools;
   [SerializeField] private Pool[] enemyProjectilePools;
   [SerializeField] private Pool[] VFXPools;
   
   static Dictionary<GameObject, Pool> dictionary;
   private void Start()
   {
      dictionary = new Dictionary<GameObject, Pool>();
      Initialize(playerProjectilePools);
      Initialize(enemyProjectilePools);
      Initialize(VFXPools);
   }

#if UNITY_EDITOR
   private void OnDestroy()
   {
      CheckPoolSize(playerProjectilePools);
      CheckPoolSize(enemyProjectilePools);
      CheckPoolSize(VFXPools);
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
   void Initialize(Pool[] pools)
   {
      foreach (var pool in pools)
      {
         #if UNITY_EDITOR
         if (dictionary.ContainsKey(pool.Prefab))
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

   public static GameObject Release(GameObject prefab)
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