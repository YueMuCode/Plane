using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScoroller : MonoBehaviour
{
    Material material;
    [SerializeField] Vector2 scrollVelocity; 

    void Start()
    {
        material = GetComponent<Renderer>().material;//�Ȼ�ȡ��Ⱦ�������Ȼ��Ϳ��Ի�ȡ���ʵ�ֵ��
    }

    // Update is called once per frame
    void Update()
    {
        material.mainTextureOffset += scrollVelocity * Time.deltaTime;
    }
}
