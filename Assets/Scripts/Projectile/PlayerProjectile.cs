using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Projectile
{
     private TrailRenderer trail;

     private void Awake()
     {
          trail = GetComponentInChildren<TrailRenderer>();
          if (moveDirection != Vector2.right)
          {
               transform.GetChild(0).rotation = Quaternion.FromToRotation(Vector2.left, moveDirection);
          }
     }

     private void OnDisable()
     {
          trail.Clear();
     }
}
