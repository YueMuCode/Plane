using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayUIController : MonoBehaviour
{
   [Header("---Player Input---")]
   [SerializeField] private PlayerInput playerinput;
   [Header("---Canvas---")]
   [SerializeField] private Canvas HUDCanvas;
   [SerializeField] private Canvas menusCanvas;

   [Header("---Player Input---")] 
   [SerializeField] private Button resumeButton;
   [SerializeField] private Button optionsButton;
   [SerializeField] private Button mainMenuButton;

   [Header("---AudioData---")] 
   [SerializeField] private AudioData pauseSFX;

   [SerializeField] private AudioData unpauseSFX;


   private int buttonPressParameterID = Animator.StringToHash("Pressed");
   private void OnEnable()
   {
      playerinput.onPause += Pause;
      playerinput.onUnPause += Unpause;
      // resumeButton.onClick.AddListener(OnResumeButtonClick);
      // optionsButton.onClick.AddListener(OnOptionsButtonClick);
      // mainMenuButton.onClick.AddListener(OnMainMenuButtonClick);
      
      ButtonPressedBehaviour.buttonFunctionTable.Add(resumeButton.gameObject.name,OnResumeButtonClick);
      ButtonPressedBehaviour.buttonFunctionTable.Add(optionsButton.gameObject.name,OnOptionsButtonClick);
      ButtonPressedBehaviour.buttonFunctionTable.Add(mainMenuButton.gameObject.name,OnMainMenuButtonClick);
   }

   private void OnDisable()
   {
      playerinput.onPause -= Pause;
      playerinput.onUnPause -= Unpause;
      // resumeButton.onClick.RemoveAllListeners();
      // optionsButton.onClick.RemoveAllListeners();
      // mainMenuButton.onClick.RemoveAllListeners();
      ButtonPressedBehaviour.buttonFunctionTable.Clear();
   }
   public void Pause()
   {
      
     
      HUDCanvas.enabled = false;
      menusCanvas.enabled = true;
      GameManager.GameState = GameState.Paused;
      TimeController.Instance.Pause();
      playerinput.EnablePauseMenuInput();
      playerinput.SwitchToDynamicUpdateMode();
      UIInput.Instance.SelectUI(resumeButton);
      AudioManager.Instance.PlaySFX(pauseSFX);
   }
   public void Unpause()
   {
      resumeButton.Select();
      resumeButton.animator.SetTrigger(buttonPressParameterID);
      AudioManager.Instance.PlaySFX(unpauseSFX);
      //OnResumeButtonClick();
   }

   void OnResumeButtonClick()
   {
      
      HUDCanvas.enabled = true;
      menusCanvas.enabled = false;
      GameManager.GameState = GameState.Playing;
      TimeController.Instance.Unpause();
      playerinput.EnableGameplayInput();
      playerinput.SwitchToFixedUpdateMode();
   }

   void OnOptionsButtonClick()
   {
      //TODO
      UIInput.Instance.SelectUI(optionsButton);
      playerinput.EnablePauseMenuInput();
   }

   void OnMainMenuButtonClick()
   {
      menusCanvas.enabled = false;
      SceneLoader.Instance.LoadMainMenuScene();
   }

}
