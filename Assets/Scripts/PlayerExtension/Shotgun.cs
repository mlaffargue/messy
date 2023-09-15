using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Messy
{
    public class Shotgun : PlayerExtension
    {
        private int shootNbr = 5;

        protected override void ChildStart()
        {
            ShootFrequency *= (shootNbr - 1);
        }

        protected override void Shoot()
        {
             // Generate X shoots
            StartCoroutine(GenerateShoot());

            AudioSource audioSource = AudioSourceHelper.PlayClipAt(GameAssets.i.soundShotgun, transform.position, 0.5f);
            
        }

        private IEnumerator GenerateShoot()
        {
            Vector2 shootCenterVector = transform.position - player.transform.position;
            if (shootCenterVector == Vector2.zero)
            {
                shootCenterVector = UnityEngine.Random.insideUnitCircle;
            }

            for (int row = 0; row < 5; row++)
            {
                for (int shootOnLine = 0; shootOnLine < shootNbr; shootOnLine++)
                {

                    float shootDamage = 2*baseDamage / (shootNbr - 1);
                    // Randomize a bit shoot damage
                    shootDamage *= UnityEngine.Random.Range(0.8f, 1.2f);

                    // Generate shoot
                    // Random angle
                    float angle = UnityEngine.Random.Range(-20f, 20f);
                    Vector2 shootAimVector = Quaternion.Euler(0f, 0f, angle) * shootCenterVector;

                    // First line only can be critical.
                    bool shootIsCritical = false;
                    if (row == 0)
                    {
                        if (UnityEngine.Random.Range(0f, 1f) < criticalChance)
                        {
                            Debug.Log("Critical shotgun");
                            shootDamage = (int)(baseDamage * criticalFactor);
                            shootIsCritical = true;
                        }
                    }

                    PistolShoot.Create(transform.position, shootAimVector, recoil, 1, shootDamage, shootIsCritical);
                }
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}