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
        private SpriteRenderer spriteRenderer;

        // Movement
        private float horizontal;
        private float vertical;
        private float diagonalSpeedLimit = 0.83f;

        // Timers
        private float nextShootTime;

        [System.NonSerialized]
        public int currentLevel = 1;
        [System.NonSerialized]
        public int currentXP = 0;
        [System.NonSerialized]
        private float currentLife;



        // Enhanceable
        [SerializeField]
        private float moveSpeed = 20f;
        [SerializeField]
        private float shootFrequency = .4f;
        [SerializeField]
        private float maxLife = 100f;
        [SerializeField]
        private float baseDamage = 100f;
        [SerializeField]
        private float recoil = 1f;
        [SerializeField]
        private int traversableEnemy = 1;
        [SerializeField]
        private float xpAbsorptionDist = 5f;
        [SerializeField]
        private float criticalChance = .05f;
        [SerializeField]
        private float criticalFactor = 1.4f;


        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            gameManager = Camera.main.GetComponentInChildren<GameManager>();
            currentLife = maxLife;
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

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.tag == "Enemy")
            {
                currentLife -= collision.GetComponent<Enemy>().GetDamage();
            }
        }

        public int GetNextLevelXP()
        {
            // Formula from Runescape simplified
            return (int) (100 * (Math.Pow(2, (double)(currentLevel) / 7) - 1));
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
                (transform.position.x - spriteRenderer.bounds.size.x < -(widthSeen / 2) && horizontal < 0)
                || (transform.position.x + spriteRenderer.bounds.size.x > widthSeen / 2) && horizontal > 0)
            {
                horizontal = 0;
            }

            if (
                (transform.position.y - spriteRenderer.bounds.size.y < -(heightSeen / 2) && vertical < 0)
                || (transform.position.y + spriteRenderer.bounds.size.y > heightSeen / 2) && vertical > 0)
            {
                vertical = 0;
            }

            if (Time.time > nextShootTime) {
                PlayerShoot();
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

        }


        private void PlayerShoot()
        {

            GameObject closestEnemy = Enemy.FindClosestEnemy(transform.position);
            if (closestEnemy != null)
            {
                Vector2 shootAimVector = closestEnemy.transform.position - transform.position;
                bool shootIsCritical = false;
                float shootDamage = (int)baseDamage;

                // Critical ?
                if (UnityEngine.Random.Range(0f, 1f) < criticalChance)
                {
                    shootDamage = (int)(baseDamage * criticalFactor);
                    shootIsCritical = true;
                }

                // Randomize a bit shoot damage
                shootDamage *= UnityEngine.Random.Range(0.8f, 1.2f);

                // Generate shoot
                Shoot.Create(transform.position, shootAimVector, recoil, traversableEnemy, shootDamage, shootIsCritical);
            }
            nextShootTime = Time.time + shootFrequency;
        }

        public void ApplyUpgrade(UpgradesEnum upgradesEnum)
        {
            switch (upgradesEnum)
            {
                case UpgradesEnum.PlayerAbsorbDistance:
                    XpAbsorptionDist += 1;
                    break;
                case UpgradesEnum.PlayerMaxLife:
                    maxLife += 10;
                    break;
                case UpgradesEnum.PlayerMoveSpeed:
                    moveSpeed += moveSpeed * 0.2f;
                    break;
                case UpgradesEnum.ShootDamage:
                    baseDamage *= 1.1f;
                    break;
                case UpgradesEnum.ShootCriticalDamage:
                    criticalFactor += .1f;
                    break;
                case UpgradesEnum.ShootCriticalFrequency:
                    criticalChance *= 1.2f;
                    break;
                case UpgradesEnum.ShootRecoil:
                    recoil += 1f;
                    break;
                case UpgradesEnum.ShootSpeed:
                    shootFrequency -= shootFrequency*0.2f;
                    break;
                case UpgradesEnum.ShootTraversal:
                    traversableEnemy += 1;
                    break;
                default:
                    throw new NotImplementedException("Unknown upgrade : " + upgradesEnum);
            }
        }


        public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
        public float XpAbsorptionDist { get => xpAbsorptionDist; set => xpAbsorptionDist = value; }
        public float CurrentLife { get => currentLife; set => currentLife = value; }
        public float MaxLife { get => maxLife; set => maxLife = value; }
        public float Recoil { get => recoil; set => recoil = value; }
    }
}