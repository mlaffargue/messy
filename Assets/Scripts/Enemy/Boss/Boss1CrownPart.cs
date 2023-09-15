using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Messy
{
    public class Boss1CrownPart : Enemy
    {
        private int idx;
        private int crownIdx;
        private Boss1 boss1;

        public static Boss1CrownPart Create(Vector2 position, int idx, int crownIdx, Boss1 parent)
        {
            GameObject crownPartInstance = Instantiate<GameObject>(GameAssets.i.boss1CrownPartPrefab, position, Quaternion.identity, parent.transform);
            crownPartInstance.transform.localScale = crownPartInstance.transform.localScale / parent.transform.localScale.x;

            Boss1CrownPart boss1CrownPart = crownPartInstance.GetComponent<Boss1CrownPart>();
            boss1CrownPart.idx = idx;
            boss1CrownPart.crownIdx = crownIdx;
            boss1CrownPart.boss1 = parent;

            return boss1CrownPart;
        }

        protected override void ChildUpdate()
        {
            if (boss1.AggressiveBehavior)
            {
                GetComponent<SpriteRenderer>().color = Color.red;
            } else
            {
                GetComponent<SpriteRenderer>().color = Color.white;
            }
        }

        public override void TakeDamage(Shoot shoot)
        {
            float damage = shoot.Damage;
            if (!boss1.isDamageableCrown(this.crownIdx))
            {
                damage = 0;
            }
            lifepoint -= damage;
            Rigidbody2D shootRb = shoot.GetComponent<Rigidbody2D>();
            if (shootRb != null)
            {
                recoilVector += shootRb.velocity * shoot.Recoil;
            }
            DamagePopup.Create(transform.position, damage, shoot.AimVector.normalized * 20f, shoot.IsCritical);
            TakeDamageAudio();
        }
        protected override void Destroyed()
        {
            boss1.Parts.Remove(this);
        }

        protected override Vector3 GetNextDirection()
        {
            Vector3 nextDirection = ((boss1.GetCrownPosition(idx, crownIdx) - transform.position) * 5)
                + (player.transform.position - transform.position).normalized * moveSpeed;

            if (boss1.AggressiveBehavior)
            {
                nextDirection *= Boss1.AGGRESSIVE_SPEED;
            }

            return nextDirection;
        }

        protected override int GetSpecificDamage()
        {
            return 2;
        }

        protected override int GetXPValue()
        {
            return 0;
        }
    }
}
