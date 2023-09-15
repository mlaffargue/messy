using System;
using UnityEngine;

namespace Messy
{
    internal class LineRendererUtil
    {
        internal static Vector3[] getStraightLinePositions(Vector3 source, Vector3 target, int vertices)
        {
            float step = 1 / (float)(vertices - 1);
            Vector3[] positions = new Vector3[vertices];
            for (int i = 0; i < vertices; i++)
            {
                positions[i] = source + target * i * step;
            }

            return positions;
        }
    }
}