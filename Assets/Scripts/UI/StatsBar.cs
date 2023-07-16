using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StatsBar : MonoBehaviour
{
    [SerializeField] private Image fillImageBack;
    [SerializeField] private Image fillImageFront;
    [SerializeField] private float fillSpeed = 0.1f;
    [SerializeField] private bool delayFill = true;
    [SerializeField] private float fillDelay = 0.5f;
    private float currentFillAmount;
    protected float targetFillAmount;
    private float t;
    private WaitForSeconds waitFordelayFill;
    private Canvas canvas;
    private Coroutine bufferedFillCoroutine;
    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera=Camera.main;
        waitFordelayFill = new WaitForSeconds(fillDelay);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public virtual void Initialize(float currentValue, float maxValue)//初始化
    {
        currentFillAmount = currentValue / maxValue;
        targetFillAmount = currentFillAmount;

        fillImageBack.fillAmount = currentFillAmount;
        fillImageFront.fillAmount = currentFillAmount;
    }

    public void UpdateStats(float currentValue, float maxValue)
    {
        targetFillAmount = currentValue / maxValue;
        if (bufferedFillCoroutine != null)
        {
            StopCoroutine(bufferedFillCoroutine);//为什么要停用
        }
        //状态减少时
        if (currentFillAmount > targetFillAmount)
        {
            fillImageFront.fillAmount = targetFillAmount;
            bufferedFillCoroutine=StartCoroutine(BufferedFillingCoroutine(fillImageBack));
        }
        //状态增加时
        if (currentFillAmount < targetFillAmount)
        {
            fillImageBack.fillAmount = targetFillAmount;
            bufferedFillCoroutine=StartCoroutine(BufferedFillingCoroutine(fillImageFront));
        }
        
        
    }

     protected virtual IEnumerator BufferedFillingCoroutine(Image image)
    {
        if (delayFill)
        {
            yield return waitFordelayFill;
        }
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * fillSpeed;
            currentFillAmount = Mathf.Lerp(currentFillAmount, targetFillAmount, t);
            image.fillAmount = currentFillAmount;
            yield return null;
        }

        
    }
}
