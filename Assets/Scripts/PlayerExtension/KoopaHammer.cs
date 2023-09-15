using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Messy
{
    public class KoopaHammer : PlayerExtension
    {

        protected override void ChildStart()
        {
            ShootFrequency *= 3;
        }
        

        protected override void Shoot()
        {
            float shootDamage = baseDamage;
            bool shootIsCritical = false;
            // Critical ?
            if (UnityEngine.Random.Range(0f, 1f) < criticalChance)
            {
                shootDamage = (int)(baseDamage * criticalFactor);
                shootIsCritical = true;
            }
            // Generate Hammer
            Shoot shoot = KoopaHammerShoot.Create(transform.position, Vector2.up, recoil, shootDamage, shootIsCritical);
            
        }
    }
}