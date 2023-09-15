using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Messy
{
    public class Laser : PlayerExtension
    {
        protected override void ChildStart()
        {
            LaserShoot.Create(this, recoil, traversableEnemy, baseDamage);
        }

        protected override void Shoot()
        {
        }
    }
}