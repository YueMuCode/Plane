using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
   [SerializeField] private PlayerInput input;
   [SerializeField] private Canvas HUDCanvas;
   [SerializeField] private AudioData confirmGameOverSound;
   private Canvas canvas;
   private Animator animator;
   private int exitStateID = Animator.StringToHash("GameOverScreenExit");
   private void Awake()
   {
      canvas = GetComponent<Canvas>();
      animator = GetComponent<Animator>();
      canvas.enabled = false;
      animator.enabled = false;
   }

   private void OnEnable()
   {
      GameManager.onGameOver += OnGameOver;
      input.onConfirmGameOver += OnConfirmGameOver;
   }

   private void OnDisable()
   {
      GameManager.onGameOver -= OnGameOver;
      input.onConfirmGameOver -= OnConfirmGameOver;
   }

   void OnGameOver()
   {
      HUDCanvas.enabled = false;
      canvas.enabled = true;
      animator.enabled = true;
      input.DisableAllInputs();
   }

   void OnConfirmGameOver()
   {
      AudioManager.Instance.PlaySFX(confirmGameOverSound);
      input.DisableAllInputs();
      animator.Play(exitStateID);
      SceneLoader.Instance.LoadScoringScene();
   }

   void EnableGameOverScreenInput()
   {
      input.EnableGameOverScreenInput();
   }
}
