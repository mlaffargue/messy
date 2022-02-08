using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Messy
{

    public class Swarm : Enemy
    {
        // Attributes
        [SerializeField]
        private float playerChaseDistance = 15f;
        private float defaultMoveSpeed;


        // Mvt
        private float nextMvtChange;
        private float nextDirectionFactor = 1f;
        private Vector2 perpendicularVector = Vector2.zero;

        protected override void ChildStart()
        {
            defaultMoveSpeed = moveSpeed;
            calcNextMvtChange();
        }

        private void calcNextMvtChange()
        {
            nextMvtChange = Random.Range(1f, 4f);
        }

        protected override int GetSpecificDamage()
        {
            return 1;
        }

        protected override Vector3 GetNextDirection()
        {
            Vector2 direction = player.transform.position - transform.position;

            nextMvtChange -= Time.deltaTime;

            if (nextMvtChange < 0)
            {
                perpendicularVector = Vector2.Perpendicular(direction) * nextDirectionFactor;
                nextDirectionFactor *= -1;

                calcNextMvtChange();
            }

            moveSpeed = defaultMoveSpeed;
            if (direction.magnitude < playerChaseDistance)
            {
                perpendicularVector = Vector2.zero;
                moveSpeed *= 1.6f;
            } 
            return (direction + perpendicularVector).normalized * moveSpeed;
        }

        protected override int GetXPValue()
        {
            return 1;
        }


    }
}
