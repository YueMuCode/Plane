using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProjectileGuidanceSystem : MonoBehaviour
{
    [SerializeField] private Projectile projectile;
    [SerializeField] private float minBallisticAngle = 50f;//用于修饰子弹追踪时的轨迹
    [SerializeField] private float maxBallistcAngle=75f;
    private float ballisticAngle;
   public  IEnumerator HomingCoroutine(GameObject target)
   {
       ballisticAngle = Random.Range(minBallisticAngle, maxBallistcAngle);
        while (gameObject.activeSelf)
        {
            if (target.activeSelf)
            {
                //移向目标
                Vector3 targetDirection = target.transform.position - transform.position;
                //修改子弹的朝向
                var angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;//将弧度转换成角度值
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation *= Quaternion.Euler(0f, 0f, ballisticAngle);
                //开始移动
                projectile.Move();
            }
            else
            {
                projectile.Move();
            }

            yield return null;
        }
    }
}
