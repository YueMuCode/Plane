using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerOverDrive : MonoBehaviour
{
  public static UnityAction on=delegate {  };
  public static UnityAction off=delegate {  };
  [SerializeField] private GameObject triggerVFX;
  [SerializeField] private GameObject engineVFXNormal;
  [SerializeField] private GameObject engineVFXOverdrive;
  [SerializeField] private AudioData onSFX;
  [SerializeField] private AudioData offSFX;


  private void Awake()
  {
    on += On;
    off += Off;
  }

  void On()
  {
    triggerVFX.SetActive(true);
    engineVFXNormal.SetActive(false);
    engineVFXOverdrive.SetActive(true);
    AudioManager.Instance.PlayRandomSFX(onSFX);
  }

  void Off()
  {
    engineVFXNormal.SetActive(false);
    engineVFXOverdrive.SetActive(true);
    AudioManager.Instance.PlayRandomSFX(offSFX);
  }
}
