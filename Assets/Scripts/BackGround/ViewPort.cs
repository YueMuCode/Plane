using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  ViewPort : Singleton<ViewPort>
{
    float minX;//视口左
    float maxX;//视口右
    float minY;//视口下
    float maxY;//视口上
    private float middleX;
    private void Start()
    {
        Camera mainCamera = Camera.main;
        Vector2 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f));//将视口坐标转换为世界坐标
        Vector2 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f));
        minX = bottomLeft.x;
        minY = bottomLeft.y;
        maxX = topRight.x;
        maxY = topRight.y;
        middleX = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0f, 0f)).x;
    }

    public Vector3 PlayerMoveablePosition(Vector3 playerPosition,float paddingX,float paddingY)
    {
        Vector3 position = Vector3.zero;
        position.x = Mathf.Clamp(playerPosition.x, minX+paddingX, maxX-paddingX);//如果玩家当前的位置超出视口，就使用边界的坐标，如果没有就保持原本的坐标不变
        position.y = Mathf.Clamp(playerPosition.y, minY+paddingY, maxY-paddingY);

        return position;
    }


    #region 敌人随机出生和移动的位置

    public Vector3 RandomEnemySpawnPosition(float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;
        position.x = maxX + paddingX;
        position.y = Random.Range(minY + paddingY, maxY - paddingY);
        return position;
    }

    public Vector3 RandomRightHalfPosition(float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;
        position.x = Random.Range(middleX, maxX - paddingX);
        position.y = Random.Range(minY + paddingY, maxY - paddingY);
        return position;
    }
    public Vector3 RandomEnemyMovePosition(float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;
        position.x = Random.Range(minX+paddingX,maxX-paddingX);
        position.y = Random.Range(minY + paddingY, maxY - paddingY);
        return position;
    }
    #endregion


}
