using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Messy
{
    public class Rifle : PlayerExtension
    {
        protected override void ChildStart()
        {
            ShootFrequency /= 3;
            baseDamage /= 2;
        }

        protected override void Shoot()
        {

            Vector2 shootAimVector = transform.position - player.transform.position;
            bool shootIsCritical = false;
            float shootDamage = (int)baseDamage;

            // Critical ?
            if (UnityEngine.Random.Range(0f, 1f) < criticalChance)
            {
                shootDamage = (int)(baseDamage * criticalFactor);
                shootIsCritical = true;
            }

            // Randomize a bit shoot damage
            shootDamage *= UnityEngine.Random.Range(0.8f, 1.2f);

            // Generate shoot
            PistolShoot.Create(transform.position, shootAimVector, recoil, traversableEnemy, shootDamage, shootIsCritical);
            AudioSource audioSource = AudioSourceHelper.PlayClipAt(GameAssets.i.soundRifle, transform.position, 0.3f);
        }
    }
}