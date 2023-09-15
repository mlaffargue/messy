using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Messy
{
    public abstract class Enemy : MonoBehaviour
    {
        // Components
        protected Rigidbody2D rb;
        protected Renderer rend;
        protected Player player;
        protected TMPro.TextMeshPro text;

        // Attributes
        protected float level = 1;
        protected float lifepoint = 100f;
        protected float moveSpeed = 5f;
        protected float timeBetweenDamage = 0.2f;

        // Behaviour
        protected Vector2 currentVelocity;
        protected Quaternion rotation = Quaternion.identity;
        protected Vector2 nextDirection;
        protected Vector2 recoilVector = new Vector2(0f, 0f);
        private float lastDamageGiven = -1;


        public static Enemy Create(Vector2 position, EnemyEnum enemyType)
        {
            GameObject enemyInstance = Instantiate(EnemyEnumHelper.GetPrefab(enemyType),
                                position,
                                EnemyEnumHelper.GetPrefab(enemyType).transform.rotation, ObjectRetriever.GetTreeFolderEnemies().transform);

            Enemy enemy = enemyInstance.GetComponent<Enemy>();
            float level = ObjectRetriever.GetGameManager().CurrentLevel;
            enemy.Level = level;
            enemy.Lifepoint = level * 2;

            return enemy;
        }

        public static GameObject FindClosestEnemy(Vector3 position)
        {
            GameObject[] gos;
            gos = GameObject.FindGameObjectsWithTag("Enemy");
            GameObject closest = null;
            float distance = Mathf.Infinity;
            foreach (GameObject go in gos)
            {
                if (go.GetComponent<Renderer>().isVisible)
                {
                    Vector3 diff = go.transform.position - position;
                    float curDistance = diff.sqrMagnitude;
                    if (curDistance < distance)
                    {
                        closest = go;
                        distance = curDistance;
                    }
                }
            }
            return closest;
        }

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            rend = GetComponent<Renderer>();
            currentVelocity = rb.velocity;
            player = ObjectRetriever.GetPlayer();
            text = gameObject.GetComponentInChildren<TMPro.TextMeshPro>();

            ChildStart();
        }

        public virtual void TakeDamage(Shoot shoot)
        {
            lifepoint -= shoot.Damage;
            Rigidbody2D shootRb = shoot.GetComponent<Rigidbody2D>();
            if (shootRb != null)
            {
                recoilVector += shootRb.velocity * shoot.Recoil;
            }
            DamagePopup.Create(transform.position, shoot.Damage, shoot.AimVector.normalized * 20f, shoot.IsCritical);
            TakeDamageAudio();
        }

        protected virtual void TakeDamageAudio()
        {
            AudioSource audioSource = AudioSourceHelper.PlayClipAt(GameAssets.i.soundBulletImpact, transform.position, 0.5f);
        }

        // Update is called once per frame
        protected void Update()
        {
            if (text != null)
            {
                text.text = "" + lifepoint;
            }

            if (lifepoint <= 0)
            {
                Destroyed();
                GameObject enemyExplosionInstance = Instantiate<GameObject>(GameAssets.i.enemyExplosion, transform.position, Quaternion.identity, ObjectRetriever.GetTreeFolderEnemies().transform);
                ParticleSystem.MainModule settings = enemyExplosionInstance.GetComponent<ParticleSystem>().main;
                if (rend is SpriteRenderer) {
                    SpriteRenderer sr = rend as SpriteRenderer;
                    settings.startColor = new ParticleSystem.MinMaxGradient(sr.color);
                }

                Destroy(gameObject);
            }

            nextDirection = GetNextDirection();
            //transform.localRotation = Quaternion.LookRotation(nextDirection);

            ChildUpdate();
        }

        protected void FixedUpdate()
        {
            
            rb.velocity = (nextDirection + recoilVector / (rb.mass/5));

            currentVelocity = rb.velocity;
            if (recoilVector != Vector2.zero)
            {
                recoilVector = Vector2.SmoothDamp(recoilVector, new Vector2(0f, 0f), ref currentVelocity, .07f);
            }

        }

        protected abstract Vector3 GetNextDirection();
        protected abstract int GetXPValue();

        public int GetDamage()
        {
            if (Time.time - lastDamageGiven > timeBetweenDamage)
            {
                lastDamageGiven = Time.time;
                return GetSpecificDamage();
            }

            return 0;
        }

        protected abstract int GetSpecificDamage();
        protected virtual void ChildStart() { }
        protected virtual void ChildUpdate() { }

        protected virtual void Destroyed()
        {
            if (Random.Range(1, 11) > 5)
            {
                Xp.Create(transform.position, GetXPValue());
            }
        }

        protected float Level { get => level; set => level = value; }
        protected float Lifepoint { get => lifepoint; set => lifepoint = value; }
        protected float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
        protected float TimeBetweenDamage { get => timeBetweenDamage; set => timeBetweenDamage = value; }
    }
}