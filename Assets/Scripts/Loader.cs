using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Messy
{
    public class Loader : MonoBehaviour
    {
		void Start()
		{
			//Check if a GameManager has already been assigned to static variable GameManager.instance or if it's still null
			if (GameManager.instance == null)
			{
				//Instantiate gameManager prefab
				Instantiate(GameAssets.i.gameManagerPrefab, Camera.main.transform);
			}


			if (HUD.instance == null)
			{
				//Instantiate gameManager prefab
				Instantiate(GameAssets.i.hudPrefab, Camera.main.transform).SetActive(true);
			}

			// UpgradePopup
			if (UpgradePopup.instance == null)
			{
				//Instantiate gameManager prefab
				Instantiate(GameAssets.i.upgradePopupPrefab, Camera.main.transform).SetActive(true);
			}
		}


	}
}