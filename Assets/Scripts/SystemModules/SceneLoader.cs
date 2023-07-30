using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneLoader : PersistentSingleton<SceneLoader>
{
    private const string GAMEPLAY = "Gameplay";
    private const string MAIN_MENU = "MainMenu";
    private const string SCORING = "Scoring";
    [SerializeField] private Image transitionImage;
    [SerializeField] private float fadeTime;
    private Color color;
    
    void Load(string sceneName)
    {
        SceneManager.LoadScene((sceneName));
    }

    IEnumerator LoadingCoroutine(string sceneName)
    {
        transitionImage.gameObject.SetActive(true);
        while (color.a < 1f)
        {
            color.a = Mathf.Clamp01(color.a + Time.unscaledDeltaTime / fadeTime);
            transitionImage.color = color;
            yield return null;
        }

        Load(sceneName);

        while (color.a > 0f)
        {
            color.a = Mathf.Clamp01(color.a - Time.unscaledDeltaTime / fadeTime);
            transitionImage.color = color;
            yield return null;
        }
        transitionImage.gameObject.SetActive(false);
    }





    public void LoadGameplayScene()
    {
        StopAllCoroutines();
        StartCoroutine(LoadingCoroutine(GAMEPLAY));
    }

    public void LoadMainMenuScene()
    {
        StopAllCoroutines();
        StartCoroutine(LoadingCoroutine(MAIN_MENU));
    }

    public void LoadScoringScene()
    {
        StopAllCoroutines();
        StartCoroutine(LoadingCoroutine(SCORING));
    }
}
