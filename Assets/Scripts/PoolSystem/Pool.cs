using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Pool
{
   public GameObject Prefab => prefab;
   public int Size=>size;
   public int RuntimeSize => queue.Count;
   
   
   
   [SerializeField] private GameObject prefab;
   [SerializeField] private int size = 1;
   private Queue<GameObject> queue;
   private Transform parent;//为生成出来的预制体指定父级,便于管理编辑器窗口
   public void Initialize(Transform parent)
   {
      queue = new Queue<GameObject>();
      this.parent = parent;
      for (var i = 0; i < size; i++)
      {
         queue.Enqueue(copy());
      }
   }

   GameObject copy()
   {
      var copy = GameObject.Instantiate(prefab,parent);
      copy.SetActive(false);
      return copy;
   }

   GameObject AvailablaObject()
   {
      GameObject availableObject = null;
      if (queue.Count > 0&&!queue.Peek().activeSelf)
      {
         availableObject = queue.Dequeue();
      }
      else
      {
         availableObject = copy();
      }
      queue.Enqueue(availableObject);//直接重新入队
      return availableObject;
   }
   public GameObject preparedObject()
   {
      GameObject preparedObject = AvailablaObject();
      preparedObject.SetActive(true);
      return preparedObject;
   }

   public GameObject preparedObject(Vector3 position)
   {
      GameObject preparedObject = AvailablaObject();
      preparedObject.SetActive(true);
      preparedObject.transform.position = position;
      return preparedObject;
   }

   public GameObject preparedObject(Vector3 position, Quaternion rotation)
   {
      GameObject preparedObject = AvailablaObject();
      preparedObject.SetActive(true);
      preparedObject.transform.position = position;
      preparedObject.transform.rotation = rotation;
      return preparedObject;
   }
   public GameObject preparedObject(Vector3 position, Quaternion rotation,Vector3 localScale)
   {
      GameObject preparedObject = AvailablaObject();
      preparedObject.SetActive(true);
      preparedObject.transform.position = position;
      preparedObject.transform.rotation = rotation;
      preparedObject.transform.localScale = localScale;
      return preparedObject;
   }
}
