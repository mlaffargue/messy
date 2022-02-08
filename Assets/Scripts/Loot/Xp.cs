using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Messy
{
    public class Xp : MonoBehaviour
    {
        // Components
        private Rigidbody2D rb;

        // Other objects
        private Player player;

        // Attributes
        private int amount;
        protected Vector2 nextDirection;
        private bool shouldBeGrabbed = false;

        public int Amount { get => amount; set => amount = value; }

        // Start is called before the first frame update
        void Start()
        {
            player = ObjectRetriever.GetPlayer();
            rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(0, 0, Time.deltaTime * 100, Space.Self);

            // Check distance to Player
            float dist = Vector3.Distance(transform.position, player.transform.position);
            
            if (dist < player.XpAbsorptionDist)
            {
                shouldBeGrabbed = true;
            }

            if (shouldBeGrabbed)
            {
                nextDirection = (player.transform.position - transform.position)*1.1f;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                player.GainXP(amount);
                Destroy(gameObject);
            }
        }

        private void FixedUpdate()
        {
            rb.velocity = nextDirection * 8;
        }
    }
}