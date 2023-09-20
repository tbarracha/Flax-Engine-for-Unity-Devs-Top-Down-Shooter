

using FlaxEngine;

namespace Game
{
    public class Agent : Script
    {
        [Header("Properties")]
        public RigidBody    rigidBody;
        public Collider     collider;

        [Header("Movement")]
        public float speed      = 5;             // in centimeters
        public float lookSpeed  = 5;             // in centimeters
        public int damage       = 1;

        protected float Speed => speed * 100;

        public override void OnStart()
        {
            rigidBody   = Actor.As<RigidBody>();
            collider    = Actor.GetChild<Collider>();
        }
    }
}
