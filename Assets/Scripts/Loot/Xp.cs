using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Messy
{
    public class Xp : MonoBehaviour
    {
        // Components
        private Rigidbody2D rb;
        private SpriteRenderer sr;
        private AudioSource audioSource;

        // Other objects
        private Player player;

        // Attributes
        [SerializeField]
        private int amount;
        protected Vector2 nextDirection;
        private bool shouldBeGrabbed = false;

        public int Amount { get => amount; set => amount = value; }

        public static void Create(Vector3 position, int amount)
        {
            GameObject xpInstance = Instantiate(GameAssets.i.xpPrefab, position, Quaternion.identity, ObjectRetriever.GetTreeFolderXPs().transform);
            xpInstance.GetComponent<Xp>().Amount = amount;

        }

        // Start is called before the first frame update
        void Start()
        {
            player = ObjectRetriever.GetPlayer();
            rb = GetComponent<Rigidbody2D>();
            sr = GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            if (amount > 2)
            {
                sr.color = new Color(0.2f,0.7f,0.2f);
            }

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
                AudioSourceHelper.PlayClipAt(GameAssets.i.soundXP, transform.position, 0.5f);
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