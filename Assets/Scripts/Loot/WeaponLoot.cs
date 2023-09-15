using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Messy
{
    public class WeaponLoot : MonoBehaviour
    {
        // Components
        private Rigidbody2D rb;
        private SpriteRenderer spriteRenderer;

        // Other objects
        private Player player;

        // Attributes
        private static List<Color> colors = new List<Color>();
        private float startTime;
        private Color currentColor;
        private Color nextColor;
        protected Vector2 nextDirection;
        private bool shouldBeGrabbed = false;

        // Start is called before the first frame update
        void Start()
        {
            player = ObjectRetriever.GetPlayer();
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            startTime = Time.time;
            currentColor = spriteRenderer.color;
            nextColor = Random.ColorHSV(0.4f, 0.8f, 0.4f, 0.8f, 0.5f, 0.7f, 1f, 1f);
        }

        // Update is called once per frame
        void Update()
        {
            // Rainbow
            spriteRenderer.color = Color.Lerp(currentColor, nextColor, (Time.time - startTime) / 2);
            
            if (spriteRenderer.color == nextColor)
            {
                startTime = Time.time;
                currentColor = nextColor;
                nextColor = Random.ColorHSV(0.4f, 0.8f, 0.4f, 0.8f, 0.5f, 0.7f, 1f, 1f);
            }

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
                player.GainWeapon();

                Destroy(gameObject);
            }
        }

        private void FixedUpdate()
        {
            rb.velocity = nextDirection * 6;
        }
    }
}