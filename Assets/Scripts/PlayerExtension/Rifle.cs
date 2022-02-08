using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Messy
{
    public class Rifle : PlayerExtension
    {

        // Timers
        private float nextShootTime;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Time.time > nextShootTime)
            {
                ShotgunShoot();
            }
        }

        private void ShotgunShoot()
        {
            GameObject closestEnemy = Enemy.FindClosestEnemy(transform.position);
            if (closestEnemy != null)
            {

            }
        }
    }
}