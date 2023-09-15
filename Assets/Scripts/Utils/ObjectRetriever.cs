using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Messy
{
    public class ObjectRetriever
    {
        public static Player GetPlayer()
        {
            return Camera.main.GetComponentInChildren<Player>();
        }

        public static GameManager GetGameManager()
        {
            return GameManager.instance;
        }

        public static EnemyManager GetEnemyManager()
        {
            return GameObject.FindGameObjectWithTag("EnemyManager").GetComponent<EnemyManager>();
        }

        public static UpgradePopup GetUpgradePopupGameObject()
        {
            return UpgradePopup.instance;
        }

        public static GameObject GetTreeFolderEnemies()
        {
            return GameObject.FindGameObjectWithTag("EnemiesFolder");
        }

        public static GameObject GetTreeFolderShoots()
        {
            return GameObject.FindGameObjectWithTag("ShootsFolder");
        }
        public static GameObject GetTreeFolderNotifications()
        {
            return GameObject.FindGameObjectWithTag("NotificationsFolder");
        }
        public static GameObject GetTreeFolderSounds()
        {
            return GameObject.FindGameObjectWithTag("SoundsFolder");
        }

        public static GameObject GetTreeFolderXPs()
        {
            return GameObject.FindGameObjectWithTag("XPsFolder");
        }
    }
}