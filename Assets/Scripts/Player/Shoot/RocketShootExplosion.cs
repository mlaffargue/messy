using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Messy
{
    public class RocketShootExplosion : Shoot
    {
        private float explosionRadius;

        public static Shoot Create(Vector2 position, float damage)
        {
            GameObject shootInstance = Instantiate<GameObject>(GameAssets.i.rocketShootExplosionPrefab, position, Quaternion.identity, ObjectRetriever.GetTreeFolderShoots().transform);
            Shoot shoot = shootInstance.GetComponent<Shoot>();
            shoot.AimVector = Vector2.zero;
            shoot.Recoil = 0;
            shoot.TraversableEnemy = 1;
            shoot.Damage = damage;
            shoot.IsCritical = false;

            Destroy(shoot, 3f);

            return shoot;
        }
        protected override void ChildStart()
        {
            explosionRadius = GetComponent<ParticleSystem>().main.startSize.constantMin / 2;
        }

        protected override void OnTriggerEnter2D(Collider2D collider)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                // Damage should be mitigated depending on the distance of the enemy
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                float factor =  Mathf.Min( explosionRadius / distance , 1f);


                damage *= factor;
                enemy.TakeDamage(this);
                // Put back normal damage
                damage /= factor;
            }
        }
    }
}
