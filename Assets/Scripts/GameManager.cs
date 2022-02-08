using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Messy
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance = null;
        private GameObject upgradePopup;
        
        // Attributes
        [System.NonSerialized]
        public float currentLevel = 1;
        private float startTime;
        private int upgradeDialogToShow = 0;

        //Awake is always called before any Start functions
        void Awake()
        {
            //Check if instance already exists
            if (instance == null)

                //if not, set instance to this
                instance = this;

            //If instance already exists and it's not this:
            else if (instance != this)

                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);

            Initialize();
        }

        void Initialize()
        {
            Instantiate<GameObject>(GameAssets.i.playerPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity, Camera.main.transform);
            Instantiate(GameAssets.i.enemyManagerPrefab, Camera.main.transform).GetComponent<EnemyManager>();
            startTime = Time.time;
        }

        // Start is called before the first frame update
        void Start()
        {
            upgradePopup = ObjectRetriever.GetUpgradePopupGameObject();
            upgradePopup.SetActive(false);
        }
       

        // Update is called once per frame
        void Update()
        {
            currentLevel = Mathf.Max(1, (Time.time - startTime) / 60);

            if (upgradeDialogToShow > 0)
            {
                // Pause game
                if (!upgradePopup.activeSelf) { 
                    upgradePopup.GetComponent<UpgradePopup>().RandomizeLoot();
                }
                upgradePopup.SetActive(true);
                PauseGame();
            } else
            {
                upgradePopup.SetActive(false);
                ResumeGame();
            }

        }

        private void PauseGame()
        {
            Time.timeScale = 0;
        }

        private void ResumeGame()
        {
            Time.timeScale = 1;
        }

        public int UpgradeDialogToShow { get => upgradeDialogToShow; set => upgradeDialogToShow = value; }
        public float StartTime { get => startTime; set => startTime = value; }
    }
}