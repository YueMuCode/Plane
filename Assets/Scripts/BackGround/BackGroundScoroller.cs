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
        material = GetComponent<Renderer>().material;//先获取渲染器组件，然后就可以获取材质的值了
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
