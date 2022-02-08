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
            return GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }

        public static GameObject GetUpgradePopupGameObject()
        {
            return GameObject.FindGameObjectWithTag("UpgradePopup");
        }
    }
}