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
   static Dictionary<GameObject, Pool> dictionary;
   private void Awake()
   {
      dictionary = new Dictionary<GameObject, Pool>();
      Initialize(playerProjectilePools);
      Initialize(enemyProjectilePools);
      Initialize(VFXPools);
      Initialize(enemyPools);
   }

#if UNITY_EDITOR
   private void OnDestroy()
   {
      CheckPoolSize(playerProjectilePools);
      CheckPoolSize(enemyProjectilePools);
      CheckPoolSize(VFXPools);
      CheckPoolSize(enemyPools);
   }
   
   void CheckPoolSize(Pool[] pools)
   {
      foreach (var pool in pools)
      {
         if (pool.RuntimeSize > pool.Size)
         {
            Debug.LogWarning(
               string.Format("当前对象池:{0}运行时的大小{1}小于初始化大小{2}",
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
            Debug.LogError("相同的池被初始化"+pool.Prefab.name);
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
         Debug.LogError("没有找到对应的对象");
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
         Debug.LogError("没有找到对应的对象");
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
         Debug.LogError("没有找到对应的对象");
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
         Debug.LogError("没有找到对应的对象");
         return null;
      }
#endif
      return dictionary[prefab].preparedObject(position,rotation,locaScale);
   }
}
