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
   public void Initialize(Transform parent)//将生成的对象存入队列中
   {
      queue = new Queue<GameObject>();
      this.parent = parent;
      for (var i = 0; i < size; i++)
      {
         queue.Enqueue(copy());
      }
   }

   GameObject copy()//生成对象
   {
      var copy = GameObject.Instantiate(prefab,parent);
      copy.SetActive(false);
      return copy;
   }

   GameObject AvailablaObject()//从池中（队列queue）取出对象
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
      queue.Enqueue(availableObject);//直接重新入队这里实际上已经完成了对象的回收操作。
      return availableObject;//相当于只是返回了对象的引用
   }
   public GameObject preparedObject()//从池中取出对应的对象之后，将对象进行必要的初始化，这里只是做了对象的激活操作。
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
