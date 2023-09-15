using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Messy
{

    public class LaserShoot : Shoot
    {
        // Components
        private LineRenderer lineRenderer;
        private BoxCollider2D boxCollider2D;

        // Attributes
        [SerializeField]
        private float laserSize = 15f;
        [SerializeField]
        private float laserSpeedFactor = 5f;
        [SerializeField]
        private float defaultMaxRocketLaunchTimer = .8f;

        private float maxRocketLaunchTimer = 1f;

        private int lineVertices = 4;
        private float detectorSizeFactor = .2f;

        protected override void ChildStart()
        {
            transform.localPosition = Vector2.zero;

            lineRenderer = GetComponent<LineRenderer>();
            boxCollider2D = GetComponent<BoxCollider2D>();

            lineRenderer.positionCount = lineVertices;
            lineRenderer.SetPositions(LineRendererUtil.getStraightLinePositions(Vector3.zero, Vector2.up * laserSize, lineVertices));

            boxCollider2D.transform.position = Vector3.up;
            boxCollider2D.size = new Vector2(lineRenderer.endWidth, laserSize * detectorSizeFactor);
            boxCollider2D.offset = new Vector2(lineRenderer.endWidth/2, laserSize * (1-detectorSizeFactor/2));
        }
        protected override bool ShouldBeDestroyed()
        {
            return false;
        }

        public static Shoot Create(PlayerExtension playerExtension, float recoil, int traversableEnemyNb, float damage)
        {
            GameObject shootInstance = Instantiate<GameObject>(GameAssets.i.laserShootPrefab, Vector2.zero, Quaternion.identity, playerExtension.transform);
            
            LaserShoot shoot = shootInstance.GetComponent<LaserShoot>();
            shoot.PlayerExtension = playerExtension;
            shoot.AimVector = playerExtension.transform.position - ObjectRetriever.GetPlayer().transform.position;
            shoot.Recoil = recoil;
            shoot.TraversableEnemy = traversableEnemyNb;
            shoot.Damage = damage;
            shoot.IsCritical = false;

            return shoot;
        }


        protected override void ChildUpdate()
        {
            transform.localPosition = Vector2.zero;

            // Update values
            defaultMaxRocketLaunchTimer = playerExtension.ShootFrequency;

            // Rotate
            transform.Rotate(Vector3.forward, -Time.deltaTime * shootSpeed * laserSpeedFactor);

            /// Double size every 5 minutes (300s)
            float computedLaserSize = laserSize * (1 + Time.time / 300f);

            // Update line size
            //lineRenderer.SetPositions(LineRendererUtil.getStraightLinePositions(Vector2.zero, Vector2.up * computedLaserSize, lineVertices));
            // Update collider size
            boxCollider2D.size = new Vector2(lineRenderer.endWidth, computedLaserSize * detectorSizeFactor);
            boxCollider2D.offset = new Vector2(lineRenderer.endWidth / 2, computedLaserSize * (1 - detectorSizeFactor / 2));

            maxRocketLaunchTimer -= Time.deltaTime;
        }

        protected override bool ObjectInExpectedBounds()
        {
            return true;
        }

        protected override void OnTriggerEnter2D(Collider2D collider)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (maxRocketLaunchTimer < 0)
                {
                    // Focus on the enemy
                    Instantiate<GameObject>(GameAssets.i.laserTargetting, enemy.gameObject.transform);

                    // Create missiles with this enemy as target
                    StartCoroutine(LaunchSalvo(enemy));
                    maxRocketLaunchTimer = defaultMaxRocketLaunchTimer;
                }
            }
        }

        private IEnumerator LaunchSalvo(Enemy enemy)
        {
            Debug.Log(level);
            for (int i=0; i<level + 2; i++)
            {
                if (enemy != null)
                {
                    RocketShoot.Create(playerExtension.transform.position, enemy.transform.position, damage, false, enemy.gameObject);
                    yield return new WaitForSeconds(.3f);
                }
            }
            yield return null;
        }
    }
}
