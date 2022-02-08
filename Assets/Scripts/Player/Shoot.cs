using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Messy
{

    public class Shoot : MonoBehaviour
    {
        // Components
        private Rigidbody2D rb;

        // Parameters
        private Vector2 aimVector;
        private float shootSpeed = 30f;
        private float damage;
        private float recoil;
        private int traversableEnemy;
        private bool isCritical = false;


        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!InsideViewport() || traversableEnemy <= 0)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
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

        private void FixedUpdate()
        {
            // Move to nearest enemy
            rb.velocity = aimVector.normalized * shootSpeed;
        }

        private bool InsideViewport()
        {
            Vector3 viewportView = Camera.main.WorldToViewportPoint(transform.position);
            return viewportView.x >= 0 && viewportView.x <= 1 && viewportView.y >= 0 && viewportView.y <= 1;
        }

        public static Shoot Create(Vector2 position, Vector2 aimVector, float recoil, int traversableEnemyNb, float damage, bool isCritical)
        {
            GameObject shootInstance = Instantiate<GameObject>(GameAssets.i.shootPrefab, position, Quaternion.identity);
            Shoot shoot = shootInstance.GetComponent<Shoot>();
            shoot.AimVector = aimVector;
            shoot.Recoil = recoil;
            shoot.TraversableEnemy = traversableEnemyNb;
            shoot.Damage = damage;
            shoot.IsCritical = isCritical;

            return shoot;
        }

        public float Damage
        { get => (int)damage; set => damage = value; }
        public int TraversableEnemy { get => traversableEnemy; set => traversableEnemy = value; }
        public float ShootSpeed { get => shootSpeed; set => shootSpeed = value; }
        public float Recoil { get => recoil; set => recoil = value; }
        public Vector2 AimVector { get => aimVector; set => aimVector = value; }
        public bool IsCritical { get => isCritical; set => isCritical = value; }
    }
}
