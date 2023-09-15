using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Messy
{
    public class Pistol : PlayerExtension
    {
        protected override void ChildStart()
        {
        }

        protected override void Shoot()
        {

            GameObject closestEnemy = Enemy.FindClosestEnemy(transform.position);
            if (closestEnemy != null)
            {
                Vector2 shootAimVector = closestEnemy.transform.position - transform.position;
                bool shootIsCritical = false;
                float shootDamage = (int)baseDamage * 2;

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

                // Generate Audio
                AudioSource audioSource = AudioSourceHelper.PlayClipAt(GameAssets.i.soundPistol, transform.position, 0.5f);
            }
        }
    }
}