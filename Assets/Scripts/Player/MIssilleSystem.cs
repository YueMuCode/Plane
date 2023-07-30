using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MIssilleSystem : MonoBehaviour
{
  [SerializeField] private GameObject missilePrefab = null;
  [SerializeField] private AudioData launchSFX = null;
  [SerializeField] private int defaultAmount = 5;
  [SerializeField] private float cooldownTime = 1f;
  private int amount;
  private bool isReady = true;
  private void Awake()
  {
    amount = defaultAmount;
  }

  private void Start()
  {
    MissileDisplay.UpdateAmountText(amount);
  }

  public void Launch(Transform muzzleTransform)
  {
    if (amount == 0||!isReady)
    {
      return;
    }

    isReady = false;
    PoolManager.Release(missilePrefab, muzzleTransform.position);
    AudioManager.Instance.PlayRandomSFX(launchSFX);
    amount--;
    MissileDisplay.UpdateAmountText(amount);
    if (amount == 0)
    {
      MissileDisplay.UpdateCooldownImage(1f);
    }
    else
    {
      StartCoroutine(CooldownCoroutine());
    }
  }

  IEnumerator CooldownCoroutine()
  {
    var cooldownValue = cooldownTime;
    while (cooldownValue > 0f)
    {
      MissileDisplay.UpdateCooldownImage(cooldownValue/cooldownTime);
      cooldownValue = Mathf.Max(cooldownValue - Time.deltaTime,0f);
      yield return null;
    }
    isReady = true;
  }
}