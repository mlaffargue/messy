using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Messy
{

    public class Chaser : Enemy
    {
       
        protected override int GetSpecificDamage()
        {
            return 10;
        }

        protected override Vector3 GetNextDirection()
        {
            return (player.transform.position - transform.position).normalized * moveSpeed;
        }

        protected override int GetXPValue()
        {
            return 5;
        }


    }
}
