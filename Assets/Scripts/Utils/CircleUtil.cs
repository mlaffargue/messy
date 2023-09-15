using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Messy
{
    public class CircleUtil
    {
        public static Vector2 GetPointOnCircle(Vector2 center, float angleInDegree)
        {
            Vector2 pos = new Vector2(center.x, center.y);
            while (pos.x == center.x && pos.y == center.y)
            {
                pos.x = center.x + Mathf.Sin(angleInDegree * Mathf.Deg2Rad);
                pos.y = center.y + Mathf.Cos(angleInDegree * Mathf.Deg2Rad);
            }
            return pos;
        }
    }
}