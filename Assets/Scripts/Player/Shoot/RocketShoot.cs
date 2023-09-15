using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Messy
{

    public class RocketShoot : Shoot
    {
        // Components
        private TrailRenderer trailRenderer;

        private float timeBeforeAimingEnemy = 1f;
        private float timeBeforeExplosion = 3f;
        private float acceleration = 10f;
        private GameObject enemyGameObject = null;
        private bool forceTargetEnemy = false;

        public static Shoot Create(Vector2 position, Vector2 aimVector, float damage, bool isCritical, GameObject enemy = null)
        {
            GameObject shootInstance = Instantiate<GameObject>(GameAssets.i.rocketShootPrefab, position, Quaternion.identity, ObjectRetriever.GetTreeFolderShoots().transform);
            RocketShoot shoot = shootInstance.GetComponent<RocketShoot>();
            shoot.AimVector = aimVector;
            shoot.Recoil = 0f;
            shoot.TraversableEnemy = 1;
            shoot.Damage = damage;
            shoot.IsCritical = isCritical;
            shoot.ShootSpeed = 10f;
            shoot.forceTargetEnemy = (enemy != null);
            shoot.enemyGameObject = enemy;

            return shoot;
        }

        protected override void ChildStart()
        {
            trailRenderer = GetComponent<TrailRenderer>();
            if (enemyGameObject != null)
            {
                Instantiate<GameObject>(GameAssets.i.targetAcquiredAnimation, enemyGameObject.transform);
            }
        }

        protected override void ChildUpdate()
        {
            ProcessExplosionTime(Time.deltaTime);
            timeBeforeAimingEnemy -= Time.deltaTime;
            if (!forceTargetEnemy && enemyGameObject == null && timeBeforeAimingEnemy <= 0)
            {
                enemyGameObject = Enemy.FindClosestEnemy(transform.position);
                if (enemyGameObject != null) 
                { 
                    Instantiate<GameObject>(GameAssets.i.targetAcquiredAnimation, enemyGameObject.transform);
                }
            }

            if (enemyGameObject != null)
            {
                trailRenderer.emitting = true;
                Vector2 newVector = enemyGameObject.transform.position - transform.position;
                aimVector = Vector2.Lerp(aimVector, newVector, Time.deltaTime * shootSpeed / 10f);
                shootSpeed += acceleration * Time.deltaTime;
            }
        }

        private void ProcessExplosionTime(float deltaTime)
        {
            timeBeforeExplosion -= Time.deltaTime;
        }
        protected override bool ShouldBeDestroyed()
        {
            return timeBeforeExplosion <= 0;
        }

        protected override void Destroyed()
        {
            Shoot shoot = RocketShootExplosion.Create(transform.position, damage);
        }

        protected override bool ObjectInExpectedBounds()
        {
            Vector3 viewportView = Camera.main.WorldToViewportPoint(transform.position); 
            return viewportView.x >= -0.3 && viewportView.x <= 1.3 && viewportView.y >= -0.3 && viewportView.y <= 1.3;
        }
        protected override void OnTriggerEnter2D(Collider2D collider)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null && enemy.gameObject == this.enemyGameObject) // Is it our target ?
            { // Explode
                timeBeforeExplosion = 0f;
            }
        }
    }
}
