using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Messy
{

    public abstract class Shoot : MonoBehaviour
    {
        // Components
        protected Rigidbody2D rb;

        // Parameters
        protected PlayerExtension playerExtension;
        protected Vector2 aimVector;
        protected float shootSpeed = 30f;
        protected float damage;
        protected float recoil;
        protected int traversableEnemy;
        protected bool isCritical = false;
        protected int level = 1;


        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            ChildStart();
        }

        // Update is called once per frame
        void Update()
        {
            if (!ObjectInExpectedBounds() || ShouldBeDestroyed())
            {
                Destroyed();
                Destroy(gameObject);
            }

            ChildUpdate();
        }

        protected virtual void OnTriggerEnter2D(Collider2D collider)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                traversableEnemy -= 1;
                enemy.TakeDamage(this);

                isCritical = false;
                damage *= Random.Range(0.5f, 0.7f);
            }
        }

        protected virtual void FixedUpdate()
        {
            // Move to nearest enemy
            if (rb != null)
            {
                rb.velocity = aimVector.normalized * shootSpeed;
            }
        }

        protected virtual bool ObjectInExpectedBounds()
        {
            Vector3 viewportView = Camera.main.WorldToViewportPoint(transform.position);
            return viewportView.x >= 0 && viewportView.x <= 1 && viewportView.y >= 0 && viewportView.y <= 1;
        }

        protected virtual void ChildStart() { }
        protected virtual void ChildUpdate() { }
        protected virtual bool ShouldBeDestroyed()
        {
            return traversableEnemy <= 0;
        }
        protected virtual void Destroyed() { }

        public float Damage { get => (int)damage; set => damage = value; }
        public int TraversableEnemy { get => traversableEnemy; set => traversableEnemy = value; }
        public float ShootSpeed { get => shootSpeed; set => shootSpeed = value; }
        public float Recoil { get => recoil; set => recoil = value; }
        public Vector2 AimVector { get => aimVector; set => aimVector = value; }
        public bool IsCritical { get => isCritical; set => isCritical = value; }
        public PlayerExtension PlayerExtension { get => playerExtension; set => playerExtension = value; }
    }
}
