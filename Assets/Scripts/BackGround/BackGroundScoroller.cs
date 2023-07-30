using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScoroller : MonoBehaviour
{
    Material material;
    [SerializeField] Vector2 scrollVelocity;


    private void Awake()
    {
        material = GetComponent<Renderer>().material;//�Ȼ�ȡ��Ⱦ�������Ȼ��Ϳ��Ի�ȡ���ʵ�ֵ��
    }

    private IEnumerator Start()
    {
        while (GameManager.GameState != GameState.GameOver)
        {
            material.mainTextureOffset += scrollVelocity * Time.deltaTime;
            yield return null;
        }
    }

}
