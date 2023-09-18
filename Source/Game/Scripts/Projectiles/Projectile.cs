
using FlaxEngine;

namespace Game
{
    public class Projectile : Script
    {
        public float speed          = 15; // in centimeters
        public float lifetime       = 1;
        public int  damage          = 1;
        public Vector3 hitBoxSize   = Vector3.One * 10;

        float time;

        public override void OnUpdate()
        {
            HandleMovement();
            HandleCollisions();
        }

        void HandleMovement()
        {
            time += Time.DeltaTime;
            if (time > lifetime)
                Destroy(Actor);

            Actor.AddMovement(Actor.Direction * speed * 100 * Time.DeltaTime);
        }

        void HandleCollisions()
        {
            Collider[] colliders = new Collider[0];
            
            if (Physics.OverlapBox(Actor.Position, hitBoxSize * .5f, out colliders, Actor.Orientation))
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    Debug.Log("Hit Collider: " + colliders[i]);
                }

                Destroy(Actor);
            }
        }

        // same Unity's OnDrawGizmos()
        public override void OnDebugDraw()
        {
            BoundingBox box = new BoundingBox()
            {
                Center = Actor.Position,
                Size = hitBoxSize,
            };

            DebugDraw.DrawWireBox(box, Color.BlueViolet);
        }
    }
}
