
using FlaxEngine;

namespace Game
{
    public class Agent : Script
    {
        [Header("Properties")]
        public RigidBody        rigidBody;
        public Collider         collider;
        public ShootComponent   shootComponent;
        public HealthComponent  healthComponent;

        [Header("Movement")]
        public float speed      = 5;             // in centimeters
        public float lookSpeed  = 5;             // in centimeters
        public int damage       = 1;

        [Header("Shooting")]
        public LayersMask impactLayers;
        public string[] impactTags;

        protected float Speed => speed * 100;

        public override void OnStart()
        {
            rigidBody   = Actor.As<RigidBody>();
            collider    = Actor.GetChild<Collider>();

            shootComponent.OnProjectileSpawned.AddListener(OnProjectileSpawned);
            healthComponent.OnDeath.AddListener(Death);
        }

        void OnProjectileSpawned(Projectile projectile)
        {
            projectile.impactLayers = impactLayers;
            projectile.impactTags   = impactTags;
        }

        protected virtual void Death()
        {
            Debug.Log(Actor.Name + ", has died!");
            Destroy(Actor);
        }
    }
}
