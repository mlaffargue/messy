using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Messy
{

    public class PistolShoot : Shoot
    {
        public static Shoot Create(Vector2 position, Vector2 aimVector, float recoil, int traversableEnemyNb, float damage, bool isCritical)
        {
            GameObject shootInstance = Instantiate<GameObject>(GameAssets.i.pistolShootPrefab, position, Quaternion.identity, ObjectRetriever.GetTreeFolderShoots().transform);
            Shoot shoot = shootInstance.GetComponent<Shoot>();
            shoot.AimVector = aimVector;
            shoot.Recoil = recoil;
            shoot.TraversableEnemy = traversableEnemyNb;
            shoot.Damage = damage;
            shoot.IsCritical = isCritical;

            return shoot;
        }
    }
}
