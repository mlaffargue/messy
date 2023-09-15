using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Messy
{
    public class RocketLauncher : PlayerExtension
    {
        protected override void ChildStart()
        {
            ShootFrequency *= 4;
        }

        protected override void Shoot()
        {

            Vector2 shootAimVector = transform.position - player.transform.position;
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
            RocketShoot.Create(transform.position, shootAimVector, shootDamage, shootIsCritical);

            // Generate Audio
            AudioSource audioSource = AudioSourceHelper.PlayClipAt(GameAssets.i.soundPistol, transform.position, 0.5f);
        }

    }
}