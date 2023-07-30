using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : Singleton<PlayerEnergy>
{
   [SerializeField] private StatsBar_HUDEnegy energyBar;
   [SerializeField] private float overdriveInterval = 0.1f;
   private WaitForSeconds waitForOverdriveInterval;
   public const int MAX = 100;
   public const int PERCENT = 1;
   private int energy;
   private bool availableToGetEnergy=true;
   protected override void Awake()
   {
      base.Awake();
      waitForOverdriveInterval = new WaitForSeconds(overdriveInterval);
   }

   private void OnEnable()
   {
      PlayerOverDrive.on += PlayerOverDriveOn;
      PlayerOverDrive.off += PlayerOverDriveOff;
   }

   private void OnDisable()
   {
      PlayerOverDrive.on -= PlayerOverDriveOn;
      PlayerOverDrive.off -= PlayerOverDriveOff;
   }

   private void Start()
   {
      energyBar.Initialize(energy,MAX);
      Obtain(MAX);
   }

   public void Obtain(int value)
   {
      if (energy == MAX||!availableToGetEnergy) return;
      energy = Mathf.Clamp(energy + value, 0, MAX);
      energyBar.UpdateStats(energy,MAX);
   }

   public void Use(int value)
   {
      energy -= value;
      energyBar.UpdateStats(energy,MAX);
      if (energy == 0 && !availableToGetEnergy)
      {
         PlayerOverDrive.off.Invoke();
      }
   }

   public bool IsEnough(int value) => value <=energy;

   void PlayerOverDriveOn()
   {
      availableToGetEnergy = false;
      StartCoroutine(nameof(KeepUsingCoroutine));
   }

   void PlayerOverDriveOff()
   {
      availableToGetEnergy = true;
      StopCoroutine(nameof(KeepUsingCoroutine));
   }

   IEnumerator KeepUsingCoroutine()
   {
      while (gameObject.activeSelf && energy > 0)
      {
         yield return waitForOverdriveInterval;
         
         Use(PERCENT);
      }
   }
}
