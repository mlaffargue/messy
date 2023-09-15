using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Messy
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance = null;
        private UpgradePopup upgradePopup;
        private Vector2 viewportSize;

        // Attributes
        [System.NonSerialized]
        private float currentLevel = 1;
        private float passedTime = 0f;
        private float timeForClock = 0f;
        private int upgradeDialogToShow = 0;
        private bool upgradeWeapon = false;
        private bool shouldResume = false;

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
            viewportSize = Camera.main.ViewportToWorldPoint(new Vector3(Camera.main.rect.width, Camera.main.rect.height, 0));
        }

        // Start is called before the first frame update
        void Start()
        {
            upgradePopup = ObjectRetriever.GetUpgradePopupGameObject();
        }
       

        // Update is called once per frame
        void Update()
        {
            timeForClock += Time.deltaTime;
            // Don't count time during boss, else too hard to have a balanced increase of difficulty
            if (!ObjectRetriever.GetEnemyManager().IsBossAlive()) { 
                passedTime += Time.deltaTime;
            }
            currentLevel = passedTime + 1;

            if (shouldResume && Time.timeScale < 1)
            {
                float timeScale = Time.timeScale + Time.deltaTime / 2f;
                Time.timeScale = Math.Min(timeScale, 1f);
            } else
            {
                shouldResume = false;
            }
            ChangeCamera();

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                EnemyManager enemyManager = ObjectRetriever.GetEnemyManager();
                enemyManager.SpawnBoss1(5);
            } else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ObjectRetriever.GetPlayer().AddExtension(ExtensionEnum.Pistol);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ObjectRetriever.GetPlayer().AddExtension(ExtensionEnum.Rifle);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                ObjectRetriever.GetPlayer().AddExtension(ExtensionEnum.Shotgun);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                ObjectRetriever.GetPlayer().AddExtension(ExtensionEnum.RocketLauncher);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                ObjectRetriever.GetPlayer().AddExtension(ExtensionEnum.Laser);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                ObjectRetriever.GetPlayer().AddExtension(ExtensionEnum.KoopaHammer);
            }


            if (upgradeWeapon)
            {
                if (!upgradePopup.gameObject.activeSelf)
                {
                    upgradePopup.GetComponent<UpgradePopup>().RandomizeWeapon();
                }
                upgradePopup.gameObject.SetActive(true);
                PauseGame();
            }
            else if(upgradeDialogToShow > 0)
            {
                // Pause game
                if (!upgradePopup.gameObject.activeSelf) { 
                    upgradePopup.GetComponent<UpgradePopup>().RandomizeLoot();
                }
                upgradePopup.gameObject.SetActive(true);
                PauseGame();
            }
        }

        private void ChangeCamera()
        {
            if (!ObjectRetriever.GetEnemyManager().IsBossAlive())
            {
                Camera.main.orthographicSize += Time.deltaTime / 20f;
                viewportSize = Camera.main.ViewportToWorldPoint(new Vector3(Camera.main.rect.width, Camera.main.rect.height, 0));
            }
        }

        public void UpgradeChoiceDone(Enum upgrade)
        {
            switch (upgrade)
            {
                case WeaponEnum w:
                    upgradeWeapon = false;
                    break;
                case EnhancementEnum e:
                    upgradeDialogToShow--;
                    break;
                default:
                    break;
            }
            upgradePopup.gameObject.SetActive(false);

            ResumeGame();
        }

        private void PauseGame()
        {
            Time.timeScale = 0;
        }

        private void ResumeGame()
        {
            Time.timeScale = 0.4f;
            shouldResume = true;
        }

        public int UpgradeDialogToShow { get => upgradeDialogToShow; set => upgradeDialogToShow = value; }
        public float PassedTime { get => passedTime; }
        public float TimeForClock { get => timeForClock; }
        public bool UpgradeWeapon { get => upgradeWeapon; set => upgradeWeapon = value; }
        public Vector2 ViewportSize { get => viewportSize; }
        public float CurrentLevel { get => currentLevel; }
    }
}