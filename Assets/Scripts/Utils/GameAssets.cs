using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Messy
{
    public class GameAssets : MonoBehaviour
    {
        public static GameAssets i = null;

        public GameObject enemyManagerPrefab;
        public GameObject gameManagerPrefab;
        public GameObject playerPrefab;
        public GameObject xpPrefab;
        public GameObject weaponLootPrefab;
        public GameObject hudPrefab;
        public GameObject damagePopupPrefab;
        public GameObject upgradePopupPrefab;

        // Ammo
        public GameObject koopaHammerShootPrefab;
        public GameObject laserShootPrefab;
        public GameObject pistolShootPrefab;
        public GameObject rocketShootPrefab;
        public GameObject rocketShootExplosionPrefab;

        // Animation
        public GameObject targetAcquiredAnimation;

        // Enemies
        public GameObject chaserPrefab;
        public GameObject swarmPrefab;

        public GameObject enemyExplosion;

        // Bosses
        public GameObject boss1Prefab;
        public GameObject boss1CrownPartPrefab;

        // Misc
        public GameObject laserTargetting;

        // Extensions
        public GameObject koopaHammerPrefab;
        public GameObject laserPrefab;
        public GameObject pistolPrefab;
        public GameObject riflePrefab;
        public GameObject rocketLauncherPrefab;
        public GameObject shotgunPrefab;

        public Material damagePopupFontMaterial;
        public Material damagePopupCriticalFontMaterial;

        // Sounds
        public AudioClip soundPistol;
        public AudioClip soundRifle;
        public AudioClip soundShotgun;
        public AudioClip soundBulletImpact;
        public AudioClip soundXP;


        void Awake()
        {
            //Check if instance already exists
            if (i == null)
                //if not, set instance to this
                i = this;

            //If instance already exists and it's not this:
            else if (i != this)
                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);

        }
    }
}