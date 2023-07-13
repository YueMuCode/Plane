using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewPort : Singleton<ViewPort>
{
    float minX;//�ӿ���
    float maxX;//�ӿ���
    float minY;//�ӿ���
    float maxY;//�ӿ���
    private void Start()
    {
        Camera mainCamera = Camera.main;
        Vector2 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f));//���ӿ�����ת��Ϊ��������
        Vector2 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f));
        minX = bottomLeft.x;
        minY = bottomLeft.y;
        maxX = topRight.x;
        maxY = topRight.y;
    }

    public Vector3 PlayerMoveablePosition(Vector3 playerPosition,float paddingX,float paddingY)
    {
        Vector3 position = Vector3.zero;
        position.x = Mathf.Clamp(playerPosition.x, minX+paddingX, maxX-paddingX);//�����ҵ�ǰ��λ�ó����ӿڣ���ʹ�ñ߽�����꣬���û�оͱ���ԭ�������겻��
        position.y = Mathf.Clamp(playerPosition.y, minY+paddingY, maxY-paddingY);

        return position;
    }



}
