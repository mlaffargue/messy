using UnityEditor;
using UnityEngine;

namespace Messy
{
    public static class CameraExtension
    {
        public static Bounds OrthographicBounds(this Camera camera)
        {
            float screenAspect = (float)Screen.width / (float)Screen.height;
            float cameraHeight = camera.orthographicSize * 2;
            Bounds bounds = new Bounds(
                camera.transform.position,
                new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
            return bounds;
        }

        public static Vector3 ClosestBoundPoint(this Camera camera, Vector3 point)
        {
            Bounds cameraBounds = Camera.main.OrthographicBounds();
            cameraBounds.size *= 1.1f;

            return cameraBounds.ClosestPoint(point);
        }
    }
}