using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;
public class LootItem : MonoBehaviour
{
   [SerializeField] private float minSpeed = 5f;
   [SerializeField] private float maxSpeed = 15f;
   [SerializeField] protected AudioData defaultPickUpSFX;
   
   protected Player player;

   private Animator animator;
   private int pickUpStateID = Animator.StringToHash("PickUp");
   protected AudioData pickUpSFX;
   protected Text lootMessage;
   private void Awake()
   {
      player = FindObjectOfType<Player>();
      animator = GetComponent<Animator>();
      pickUpSFX = defaultPickUpSFX;
      lootMessage = GetComponentInChildren<Text>(true);
   }

   private void OnEnable()
   {
      StartCoroutine(nameof(MoveCoroutine));
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      PickUp();
   }

   protected virtual void PickUp()
   {
      StopAllCoroutines();
      animator.Play(pickUpStateID);
      AudioManager.Instance.PlayRandomSFX(pickUpSFX);
   }

   IEnumerator MoveCoroutine() //自动飞向玩家
   {
      float speed = Random.Range(minSpeed, maxSpeed);
      Vector3 direction=Vector3.left;
      while (true)
      {
         if (player.isActiveAndEnabled)
         {
            direction = (player.transform.position - transform.position).normalized;
         }
         transform.Translate(direction*speed*Time.deltaTime);
         yield return null;
      }
   }
}
