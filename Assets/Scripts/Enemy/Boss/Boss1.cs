using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Messy
{
    public class Boss1 : Boss
    {
        // Components
        private SpriteRenderer spriteRenderer;
        private ParticleSystem particleSettings;

        [SerializeField]
        private Sprite killableSprite = null;
        [SerializeField]
        private Sprite damagedSprite = null;
        [SerializeField]
        private float baseCrownRotation = 50f;
        [SerializeField]
        private int baseCrownElementNbr = 15;
        
        private int crownNbr;

        public static float AGGRESSIVE_SPEED = 3f;
        private static float AGGRESSIVE_BEHAVIOU_PERIOD = 10f;
        private static float DEFAULT_AGGRESSIVE_DURATION = 5f;

        private bool aggressiveBehavior = false;
        private float nextAggressiveBehavior = AGGRESSIVE_BEHAVIOU_PERIOD;
        private float aggressiveBehaviorDuration = DEFAULT_AGGRESSIVE_DURATION;

        private List<Boss1CrownPart> parts = new List<Boss1CrownPart>();
        private bool initialized = false;

        public static Boss1 Create(Vector3 position, int crownNbr)
        {
            GameObject boss1Instance = Instantiate(GameAssets.i.boss1Prefab, position, Quaternion.identity, ObjectRetriever.GetTreeFolderEnemies().transform);
            Boss1 boss1 = boss1Instance.GetComponent<Boss1>();
            float level = ObjectRetriever.GetGameManager().CurrentLevel;
            boss1.level = level;
            boss1.lifepoint *= level;
            boss1.crownNbr = crownNbr;

            return boss1;
        }

        internal bool isDamageableCrown(int crownIdx)
        {
            return (parts.Count <= baseCrownElementNbr * ((crownIdx * crownIdx) + crownIdx)*0.5f);
        }

        protected override void ChildStart()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            particleSettings = GetComponent<ParticleSystem>();

            lifepoint = 5000;

            for (int crownIdx = 1; crownIdx <= crownNbr; crownIdx++) { 
                // Generate crown
                for (int i = 0; i < baseCrownElementNbr * crownIdx; i++)
                {
                    parts.Add(Boss1CrownPart.Create(GetCrownPosition(i, crownIdx), i, crownIdx, this));
                }
            }
            
            initialized = true;
        }

        protected override void ChildUpdate()
        {
            if (initialized) { 
                HandleAgressivityStatus();
                HandleSprite();
            }
        }

        private void HandleSprite()
        {
            ParticleSystem.ColorOverLifetimeModule module = particleSettings.colorOverLifetime;
            if (parts.Count == 0)
            {
                spriteRenderer.sprite = killableSprite;
                module.color = new ParticleSystem.MinMaxGradient(new Color(1f, 1f, 1f, 0.1f));
            } else
            {
                spriteRenderer.sprite = damagedSprite;
                module.color = new ParticleSystem.MinMaxGradient(new Color(1f, 1f, 1f, crownNbr/10f));
            }
        }

        private void HandleAgressivityStatus()
        {
            if (!aggressiveBehavior && parts.Count > 0)
            {
                nextAggressiveBehavior -= Time.deltaTime;
                if (nextAggressiveBehavior < 0)
                {
                    aggressiveBehavior = true;

                    nextAggressiveBehavior = AGGRESSIVE_BEHAVIOU_PERIOD;
                }
            }
            else
            {
                aggressiveBehaviorDuration -= Time.deltaTime;
                if (aggressiveBehaviorDuration < 0)
                {
                    aggressiveBehaviorDuration = DEFAULT_AGGRESSIVE_DURATION;
                    aggressiveBehavior = false;
                }
            }
        }

        public Vector3 GetCrownPosition(int elementIdx, int crownIdx)
        {
            int totalElements = crownIdx * baseCrownElementNbr;
            Vector3 position = CircleUtil.GetPointOnCircle(Vector2.zero, elementIdx * (360f / totalElements));
            // Put them a bit further
            position *=  2 * totalElements / 7;
            position += transform.position;

            // Rotate depending on current time
            float rotationValue = Time.time * baseCrownRotation * Mathf.Pow(-1, crownIdx + 1);
            if (aggressiveBehavior)
            {
                rotationValue *= 4;
            }
            position = VectorUtil.RotatePointAroundPivot(position, transform.position, rotationValue);

            return position;
        }

        public override void TakeDamage(Shoot shoot)
        {
            if (parts.Count > 0) {
                shoot.Damage = 0;
                shoot.Recoil = 0;
                shoot.TraversableEnemy = 0;
            }
            base.TakeDamage(shoot);
        }

        protected override Vector3 GetNextDirection()
        {
            Vector3 nextDirection = (player.transform.position - transform.position).normalized * moveSpeed;
            if (aggressiveBehavior)
            {
                nextDirection *= AGGRESSIVE_SPEED;
            }

            return nextDirection;
        }

        protected override int GetSpecificDamage()
        {
            return 10;
        }

        protected override int GetXPValue()
        {
            return 200;
        }

        public List<Boss1CrownPart> Parts { get => parts; set => parts = value; }
        public bool AggressiveBehavior { get => aggressiveBehavior; set => aggressiveBehavior = value; }
    }
}
