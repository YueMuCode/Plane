using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProjectileGuidanceSystem : MonoBehaviour
{
    [SerializeField] private Projectile projectile;
    [SerializeField] private float minBallisticAngle = 50f;//���������ӵ�׷��ʱ�Ĺ켣
    [SerializeField] private float maxBallistcAngle=75f;
    private float ballisticAngle;
   public  IEnumerator HomingCoroutine(GameObject target)
   {
       ballisticAngle = Random.Range(minBallisticAngle, maxBallistcAngle);
        while (gameObject.activeSelf)
        {
            if (target.activeSelf)
            {
                //����Ŀ��
                Vector3 targetDirection = target.transform.position - transform.position;
                //�޸��ӵ��ĳ���
                var angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;//������ת���ɽǶ�ֵ
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation *= Quaternion.Euler(0f, 0f, ballisticAngle);
                //��ʼ�ƶ�
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
