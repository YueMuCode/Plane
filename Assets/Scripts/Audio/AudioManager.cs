using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : PersistentSingleton<AudioManager>
{
   [SerializeField] private AudioSource sFxPlayer;
   [SerializeField] private float minPitch = 0.9f;
   [SerializeField] private float maxpitch = 1.1f;
   public void PlaySFX(AudioData audioData)//�ʺ�����UI
   {
      sFxPlayer.PlayOneShot(audioData.audioClip,audioData.volume);
   }

   public void PlayRandomSFX(AudioData audioData)//�ʺ������ӵ�
   {
      sFxPlayer.pitch = Random.Range(minPitch, maxpitch);
      PlaySFX(audioData);
   }
   public void PlayRandomSFX(AudioData []audioData)//�ʺ������ӵ�
   {
      PlayRandomSFX(audioData[Random.Range(0,audioData.Length)]);
   }
}
