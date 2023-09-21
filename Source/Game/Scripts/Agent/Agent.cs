

using FlaxEngine;

namespace Game
{
    public class Agent : Script
    {
        [Header("Properties")]
        public RigidBody        rigidBody;
        public Collider         collider;
        public HealthComponent  healthComponent;
        public ShootComponent   shootComponent;

        [Header("Agent Properties")]
        public int damage       = 1;
        public float speed      = 5;             // in centimeters
        public float lookSpeed  = 5;             // in centimeters

        [Header("Impact Layers")]
        public LayersMask impactLayers;
        public string[] damageTags;

        protected float Speed => speed * 100;

        public override void OnStart()
        {
            if (rigidBody == null)
                rigidBody   = Actor.As<RigidBody>();

            if (collider == null)
                collider    = Actor.GetChild<Collider>();

            healthComponent.OnDeath.AddListener(Death);
            shootComponent.OnProjectileSpawned.AddListener(SetProjectileProperties);
        }

        void SetProjectileProperties(Projectile projectile)
        {
            projectile.impactLayers = impactLayers;
            projectile.damageTags   = damageTags;
        }

        protected virtual void Death()
        {
            
        }
    }
}
