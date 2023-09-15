using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Messy
{
    public class VectorUtil
    {
        public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, float angles)
        {
            Vector3 dir = point - pivot; // get point direction relative to pivot
            dir = Quaternion.Euler(0, 0, angles) * dir; // rotate it
            point = dir + pivot; // calculate rotated point
            return point; // return it
        }
    }
}