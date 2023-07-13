using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScoroller : MonoBehaviour
{
    Material material;
    [SerializeField] Vector2 scrollVelocity; 

    void Start()
    {
        material = GetComponent<Renderer>().material;//先获取渲染器组件，然后就可以获取材质的值了
    }

    // Update is called once per frame
    void Update()
    {
        material.mainTextureOffset += scrollVelocity * Time.deltaTime;
    }
}
