
using FlaxEngine;

namespace Game
{
    public class Projectile : Script
    {
        [Header("Properties")]
        public float speed          = 15; // in centimeters
        public float lifetime       = 1;
        public int  damage          = 1;
        public Vector3 hitBoxSize   = Vector3.One * 10;

        [Header("Impact Layers")]
        public LayersMask impactLayers;     // layers projectile collides with
        public string[] damageTags;         // actors with these tags, will recieve damage

        float time;

        [HideInEditor] public readonly CustomEvent<Projectile> OnProjectileDestroyed = new CustomEvent<Projectile>();

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
                    ProcessCollision(colliders[i]);

                Destroy(Actor);
            }
        }

        void ProcessCollision(Collider collider)
        {
            Debug.Log("Hit Collider: " + collider);

            if (collider.HasAnyTag(damageTags) == true)
            {
                // find health component
                HealthComponent health = collider.FindScriptInParent<HealthComponent>();

                if (health == null)
                    health = collider.FindScript<HealthComponent>();

                if (health != null)
                    health.ApplyDamage(damage);
            }
        }

        public override void OnDestroy()
        {
            OnProjectileDestroyed?.Invoke(this);
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
