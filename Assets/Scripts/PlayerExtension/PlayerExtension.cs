using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Messy
{
    public abstract class PlayerExtension : MonoBehaviour
    {
        protected Player player;

        [SerializeField]
        protected float baseDamage = 100f;
        [SerializeField]
        protected float criticalChance = .05f;
        [SerializeField]
        protected float criticalFactor = 1.4f;
        [SerializeField]
        protected float recoil = 1f;
        [SerializeField]
        private float shootFrequency = .6f;
        [SerializeField]
        protected int traversableEnemy = 1;

        // Timers
        protected float nextShootTime;
        
        private readonly float SHOOT_FREQ_REDUCTION = 0.1f;
        private readonly float SHOOT_CRIT_FREQ_FACTOR = 1.2f;
        private readonly float SHOOT_CRIT_DMG_FACTOR = .1f;
        private readonly float SHOOT_BASE_DMG_FACTOR = 1.1f;
        private readonly float SHOOT_RECOIL_INC = 1f;


        public static PlayerExtension Create(ExtensionEnum extensionEnum, Vector2 position)
        {
            GameObject extensionInstance = Instantiate<GameObject>(ExtensionEnumHelper.GetPrefab(extensionEnum));
            extensionInstance.transform.position = position;
            return extensionInstance.GetComponent<PlayerExtension>();
        }

        public static Vector2 GetExtensionPlacement(ExtensionPlacementEnum extensionPlacementEnum)
        {
            Player player = ObjectRetriever.GetPlayer();

            return player.transform.Find(extensionPlacementEnum.ToString()).transform.position;
        }

        // Start is called before the first frame update
        void Start()
        {
            player = ObjectRetriever.GetPlayer();

            ChildStart();
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.time > nextShootTime)
            {
                Shoot();
                nextShootTime = Time.time + ShootFrequency;
            }

        }

        protected abstract void Shoot();

        public virtual void ApplyUpgrade(Enum upgradesEnum)
        {
            ApplyDefaultUpgrade(upgradesEnum);
        }

        public void ApplyDefaultUpgrade(Enum upgradesEnum)
        {
            switch (upgradesEnum)
            {
                case EnhancementEnum.ShootDamage:
                    baseDamage *= SHOOT_BASE_DMG_FACTOR;
                    return;
                case EnhancementEnum.ShootCriticalDamage:
                    criticalFactor += SHOOT_CRIT_DMG_FACTOR;
                    return;
                case EnhancementEnum.ShootCriticalFrequency:
                    criticalChance *= SHOOT_CRIT_FREQ_FACTOR;
                    return;
                case EnhancementEnum.ShootRecoil:
                    recoil += SHOOT_RECOIL_INC;
                    return;
                case EnhancementEnum.ShootSpeed:
                    ShootFrequency -= ShootFrequency * SHOOT_FREQ_REDUCTION;
                    return;
                case EnhancementEnum.ShootTraversal:
                    traversableEnemy += 1;
                    return;
            }
        }

        protected virtual void ChildStart() {}


        public float ShootFrequency { get => shootFrequency; set => shootFrequency = value; }
    }
}