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
        public GameObject shootPrefab;
        public GameObject xpPrefab;
        public GameObject hudPrefab;
        public GameObject damagePopupPrefab;
        public GameObject upgradePopupPrefab;

        // Enemies
        public GameObject chaserPrefab;
        public GameObject swarmPrefab;

        public Material damagePopupFontMaterial;
        public Material damagePopupCriticalFontMaterial;

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