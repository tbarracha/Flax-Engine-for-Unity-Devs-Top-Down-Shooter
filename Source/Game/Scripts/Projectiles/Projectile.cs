
using FlaxEngine;

namespace Game
{
    public class Projectile : Script
    {
        public float speed          = 15; // in centimeters
        public float lifetime       = 1;
        public int  damage          = 1;
        public Vector3 hitBoxSize   = Vector3.One * 10;

        [Header("Impact Layers")]
        public LayersMask   impactLayers;
        public string[]     impactTags;

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
            
            if (Physics.OverlapBox(Actor.Position, hitBoxSize * .5f, out colliders, Actor.Orientation, impactLayers))
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    Collider collider = colliders[i];
                    Debug.Log("Hit Collider: " + collider);

                    DamageCollider(collider);
                }

                Destroy(Actor);
            }
        }

        void DamageCollider(Collider collider)
        {
            if (collider.HasAnyTag(impactTags) == false)
                return;

            HealthComponent health = collider.FindScript<HealthComponent>();

            if (health == null)
                health = collider.FindScriptInParent<HealthComponent>();

            if (health != null)
                health.ApplyDamage(damage);
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
