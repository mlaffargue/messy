using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Messy
{
    public abstract class Enemy : MonoBehaviour
    {
        // Components
        protected Rigidbody2D rb;
        protected Player player;
        protected TMPro.TextMeshPro text;

        // Attributes
        protected int level = 1;
        protected float lifepoint = 100f;
        protected float moveSpeed = 5f;
        protected float timeBetweenDamage = 0.2f;

        // Behaviour
        protected Vector2 currentVelocity;
        protected Vector2 nextDirection;
        protected Vector2 recoilVector = new Vector2(0f, 0f);
        private float lastDamageGiven = -1;


        public static Enemy Create(Vector2 position, float level, EnemyEnum enemyType)
        {
            GameObject enemyInstance = Instantiate(EnemyEnumHelper.GetPrefab(enemyType),
                                position,
                                Quaternion.identity, Camera.main.transform);
            Enemy enemy = enemyInstance.GetComponent<Enemy>();
            enemy.Level = (int)level;
            enemy.Lifepoint *= level;

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
            currentVelocity = rb.velocity;
            player = ObjectRetriever.GetPlayer();
            text = gameObject.GetComponentInChildren<TMPro.TextMeshPro>();

            ChildStart();
        }

        public void TakeDamage(Shoot shoot)
        {
            lifepoint -= shoot.Damage;
            recoilVector += shoot.GetComponent<Rigidbody2D>().velocity * shoot.Recoil;
            DamagePopup.Create(transform.position, shoot.Damage, shoot.AimVector.normalized * 20f, shoot.IsCritical);
        }

        // Update is called once per frame
        protected void Update()
        {
            //text.text = "" + lifepoint;

            if (lifepoint <= 0)
            {
                if (Random.Range(1,10) > 5)
                {
                    GameObject xpInstance = Instantiate(GameAssets.i.xpPrefab, transform.position, Quaternion.identity, transform.parent);
                    xpInstance.GetComponent<Xp>().Amount = GetXPValue() * level;
                }

                Destroy(gameObject);
            }

            nextDirection = GetNextDirection();
        }

        protected void FixedUpdate()
        {
            rb.velocity = nextDirection + recoilVector;
            currentVelocity = rb.velocity;
            recoilVector = Vector2.SmoothDamp(recoilVector, new Vector2(0f, 0f), ref currentVelocity, .07f);
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



        protected int Level { get => level; set => level = value; }
        protected float Lifepoint { get => lifepoint; set => lifepoint = value; }
        protected float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
        protected float TimeBetweenDamage { get => timeBetweenDamage; set => timeBetweenDamage = value; }
    }
}