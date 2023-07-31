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
               string.Format("当前对象池:{0}运行时的大小{1}小于初始化大小{2}",
                  pool.Prefab.name,
                  pool.RuntimeSize,
                  pool.Size
               ));
         }
      }
   }
   #endif
   void Initialize(Pool[] pools)//将对象池存入字典中，以便使用的时候可以直接传入对象的类型直接调用出相应的对象
   {
      foreach (var pool in pools)
      {
         #if UNITY_EDITOR
         if (dictionary.ContainsKey(pool.Prefab))//防止存入多相同的对象池
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

   public static GameObject Release(GameObject prefab)//根据传入的预制体，经过字典查找返回一个可以使用的对象
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
