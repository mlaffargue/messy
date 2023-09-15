using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Messy
{

    public class KoopaHammerShoot : Shoot
    {
        private float throwForce = 500f;

        public static Shoot Create(Vector2 position, Vector2 aimVector, float recoil, float damage, bool isCritical)
        {
            GameObject shootInstance = Instantiate<GameObject>(GameAssets.i.koopaHammerShootPrefab, position, Quaternion.identity, ObjectRetriever.GetTreeFolderShoots().transform);
            KoopaHammerShoot shoot = shootInstance.GetComponent<KoopaHammerShoot>();
            shoot.AimVector = aimVector;
            shoot.Recoil = recoil;
            shoot.TraversableEnemy = int.MaxValue;
            shoot.Damage = damage;
            shoot.IsCritical = isCritical;

            return shoot;
        }

        protected override void ChildStart()
        {
            // Move to nearest enemy
            float newX = AimVector.x + Random.Range(-1f, 1f);
            AimVector = new Vector2(newX, AimVector.y);
            AimVector.Normalize();
            rb.AddForce(AimVector * throwForce);
            rb.AddTorque(400f * Mathf.Sign(-AimVector.x));
        }

        protected override void FixedUpdate()
        {
        }

        protected virtual bool ObjectInExpectedBounds()
        {
            Vector3 viewportView = Camera.main.WorldToViewportPoint(transform.position);
            return viewportView.x >= 0 && viewportView.x <= 1 && viewportView.y >= 0 && viewportView.y <= 1;
        }
    }
}
