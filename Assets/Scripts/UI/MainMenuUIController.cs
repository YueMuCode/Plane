using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    [Header("---Canvas---")]
    [SerializeField] private Canvas mainMenuCanvas;

    [Header("---Buttons---")] 
    [SerializeField] private Button buttonStart;

    [SerializeField] private Button buttonOptions;

    [SerializeField] private Button buttonQuit;

    private void OnEnable()
    {
       ButtonPressedBehaviour.buttonFunctionTable.Add(buttonStart.gameObject.name,OnButtonStartClicked);
       ButtonPressedBehaviour.buttonFunctionTable.Add(buttonOptions.gameObject.name,OnButtonOptionClicked);
       ButtonPressedBehaviour.buttonFunctionTable.Add(buttonQuit.gameObject.name,OnButtonQuitClicked);
    }

    private void OnDisable()
    {
        //buttonStart.onClick.RemoveAllListeners();
        ButtonPressedBehaviour.buttonFunctionTable.Clear();
    }

    private void Start()
    {
        Time.timeScale = 1f;
        GameManager.GameState = GameState.Playing;
        UIInput.Instance.SelectUI(buttonStart);
    }

    void OnButtonStartClicked()
    {
        mainMenuCanvas.enabled = false;
        SceneLoader.Instance.LoadGameplayScene();
      //   Debug.Log("11111");
    }

    void OnButtonOptionClicked()
    {
        UIInput.Instance.SelectUI(buttonOptions);
    }

    void OnButtonQuitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
