using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Messy
{
    public class DamagePopup : MonoBehaviour
    {
        private const float DISAPPEAR_TIMER_MAX = .5f;

        private TMPro.TextMeshPro text;
        private Color color;
        private float disappearTimer;
        private Vector3 moveVector;
        private bool isCritical;

        public static DamagePopup Create(Vector3 position, float amount, Vector3 shootDirection, bool isCritical)
        {
            GameObject damagePopupInstance = Instantiate(GameAssets.i.damagePopupPrefab, position, Quaternion.identity);
            DamagePopup damagePopup = damagePopupInstance.GetComponent<DamagePopup>();

            damagePopup.Setup(amount, shootDirection, isCritical);

            return damagePopup;
        }
        private void Awake()
        {
            text = GetComponent<TMPro.TextMeshPro>();
        }
        // Start is called before the first frame update
        public void Setup(float amount, Vector3 shootDirection, bool isCritical)
        {
            this.isCritical = isCritical;
            text.fontMaterial = GameAssets.i.damagePopupFontMaterial;
            if (isCritical)
            {
                text.fontMaterial = GameAssets.i.damagePopupCriticalFontMaterial;
            }
            text.SetText("" + amount);
            color = text.color;
            disappearTimer = DISAPPEAR_TIMER_MAX;

            moveVector = shootDirection;
        }

        private void Update()
        {
            transform.position += moveVector * Time.deltaTime;

            moveVector -= moveVector * 8f * Time.deltaTime;

            float scale = 2f;
            if (isCritical)
            {
                scale = 4f;
            }

            if (disappearTimer > DISAPPEAR_TIMER_MAX * .5f)
            {
                // First half
                transform.localScale += Vector3.one * scale * Time.deltaTime;
            } else
            {
                // Second half
                transform.localScale -= Vector3.one * scale * Time.deltaTime;
            }

            disappearTimer -= Time.deltaTime;
            if (disappearTimer < 0)
            {
                color.a -= 5f * Time.deltaTime;
                text.color = color;

                if (color.a < 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}