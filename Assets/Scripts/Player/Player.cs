using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Messy
{
    public class Player : MonoBehaviour
    {
        // Game Manager
        private GameManager gameManager;

        // Components
        private Rigidbody2D rb;

        // Movement
        private float horizontal;
        private float vertical;
        private float diagonalSpeedLimit = 0.83f;

        // Calculated Attributes
        private int currentLevel = 1;
        private int currentXP = 0;
        private float currentLife;

        private List<PlayerExtension> extensions = new List<PlayerExtension>();

        // Enhanceable Attributes
        [SerializeField]
        private float moveSpeed = 20f;
        [SerializeField]
        private float maxLife = 100f;
        [SerializeField]
        private float xpAbsorptionDist = 5f;
        [SerializeField]
        private float rotationSpeed = 1f;


        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            gameManager = Camera.main.GetComponentInChildren<GameManager>();
            currentLife = maxLife;

            AddExtension(ExtensionEnum.Pistol);
        }


        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.tag == "Enemy" && collision.GetComponent<Enemy>() != null)
            {
                currentLife -= collision.GetComponent<Enemy>().GetDamage();
            }
        }

        public int GetNextLevelXP()
        {
            // Formula from Runescape simplified
            //return (int) (100 * (Math.Pow(2, (double)(currentLevel) / 7) - 1));
            return (int)(60 * (Math.Pow(2, (double)(currentLevel) / 8) - 1)) + 5;
        }

        // Update is called once per frame
        void Update()
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");

            // Check borders
            float heightSeen = Camera.main.orthographicSize * 2.0f;
            float widthSeen = heightSeen * Camera.main.aspect;

            if (
                (transform.position.x - transform.localScale.x < -(widthSeen / 2) && horizontal < 0)
                || (transform.position.x + transform.localScale.x > widthSeen / 2) && horizontal > 0)
            {
                horizontal = 0;
            }

            if (
                (transform.position.y - transform.localScale.y < -(heightSeen / 2) && vertical < 0)
                || (transform.position.y + transform.localScale.y > heightSeen / 2) && vertical > 0)
            {
                vertical = 0;
            }
        }

        void FixedUpdate()
        {
            if (horizontal != 0 && vertical != 0)
            { // Diagonal
                horizontal *= diagonalSpeedLimit;
                vertical *= diagonalSpeedLimit;
            }

            rb.velocity = new Vector2(horizontal * moveSpeed, vertical * moveSpeed);
            rb.rotation += rotationSpeed;
        }

        public void AddExtension(ExtensionEnum extensionEnum)
        {
            // Create the extension
            ExtensionPlacementEnum placement = (ExtensionPlacementEnum)Enum.ToObject(typeof(ExtensionPlacementEnum), extensions.Count);

            PlayerExtension playerExtension = PlayerExtension.Create(extensionEnum, PlayerExtension.GetExtensionPlacement(placement));
            playerExtension.transform.parent = transform;

            extensions.Add(playerExtension);
        }

        public void GainXP(int amount)
        {
            int nextLevelXP = GetNextLevelXP();
            currentXP += amount;
            while (currentXP >= nextLevelXP)
            {
                currentXP -= nextLevelXP;
                currentLevel++;
                nextLevelXP = GetNextLevelXP();
                gameManager.UpgradeDialogToShow += 1;
            }
        }

        public void GainWeapon()
        {
            gameManager.UpgradeWeapon = true;
        }


        public void ApplyUpgrade(Enum upgradesEnum)
        {
            switch (upgradesEnum)
            {
                case EnhancementEnum.PlayerAbsorbDistance:
                    XpAbsorptionDist += 1;
                    break;
                case EnhancementEnum.PlayerMaxLife:
                    maxLife += 10;
                    break;
                case EnhancementEnum.PlayerMoveSpeed:
                    moveSpeed += moveSpeed * 0.05f;
                    break;
                case WeaponEnum.Pistol:
                    AddExtension(ExtensionEnum.Pistol);
                    break;
                case WeaponEnum.Rifle:
                    AddExtension(ExtensionEnum.Rifle);
                    break;
                case WeaponEnum.RocketLauncher:
                    AddExtension(ExtensionEnum.RocketLauncher);
                    break;
                case WeaponEnum.Shotgun:
                    AddExtension(ExtensionEnum.Shotgun);
                    break;
                default:
                    ApplyUpgradeToExtensions(upgradesEnum);
                    break;
            }
        }

        private void ApplyUpgradeToExtensions(Enum upgradesEnum)
        {
            foreach (PlayerExtension extension in extensions)
            {
                extension.ApplyUpgrade(upgradesEnum);
            }
        }


        public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
        public float XpAbsorptionDist { get => xpAbsorptionDist; set => xpAbsorptionDist = value; }
        public float CurrentLife { get => currentLife; set => currentLife = value; }
        public float MaxLife { get => maxLife; set => maxLife = value; }
        public int CurrentLevel { get => currentLevel; set => currentLevel = value; }
        public int CurrentXP { get => currentXP; set => currentXP = value; }
        public float CurrentLife1 { get => currentLife; set => currentLife = value; }
    }
}