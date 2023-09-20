

using FlaxEngine;

namespace Game
{
    public static class Gizmos
    {
        public static void DrawCube(Vector3 center, Vector3 size, Quaternion rotation, Color color)
        {
            BoundingBox boundingBox = new BoundingBox();
            boundingBox.Size = size;

            OrientedBoundingBox orientatedboundingBox = new OrientedBoundingBox(boundingBox);

            Transform transform = new Transform(center, rotation);
            orientatedboundingBox.Transform(ref transform);

            DebugDraw.DrawBox(orientatedboundingBox, color);
        }

        public static void DrawWireCube(Vector3 center, Vector3 size, Quaternion rotation, Color color)
        {
            BoundingBox boundingBox = new BoundingBox();
            boundingBox.Size = size;

            OrientedBoundingBox orientatedboundingBox = new OrientedBoundingBox(boundingBox);

            Transform transform = new Transform(center, rotation);
            orientatedboundingBox.Transform(ref transform);

            DebugDraw.DrawWireBox(orientatedboundingBox, color);
        }

        public static void DrawSphere(Vector3 center, float radius, Color color)
        {
            BoundingSphere boundingSphere = new BoundingSphere(center, radius);
            DebugDraw.DrawSphere(boundingSphere, color);
        }

        public static void DrawWireSphere(Vector3 center, float radius, Color color)
        {
            BoundingSphere boundingSphere = new BoundingSphere(center, radius);
            DebugDraw.DrawWireSphere(boundingSphere, color);
        }
    }
}
