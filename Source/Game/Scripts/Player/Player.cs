
using FlaxEngine;

namespace Game
{
    public class Player : Script
    {
        [Header("Properties")]
        public RigidBody rigidBody;
        public Collider collider;
        public Actor pickActor;
        public Collider pickCollider;

        [Header("Movement")]
        public float speed = 5; // in centimeters

        float horizontal, vertical;
        Vector3 direction;

        public float Speed => speed * 100;

        public override void OnStart()
        {
            rigidBody = Actor.As<RigidBody>();
            collider = Actor.GetChild<Collider>();
        }

        public override void OnUpdate()
        {
            HandleMovement();
            HandleMousePick();
        }

        protected void HandleMovement()
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
            direction = new Vector3(horizontal, 0, vertical);

            rigidBody.LinearVelocity = direction * Speed;
        }

        void HandleMousePick()
        {
            pickActor.Position = Actor.Position;

            Ray ray = Camera.MainCamera.ConvertMouseToRay(Input.MousePosition);
            RayCastHit hit;

            if (pickCollider.RayCast(ray.Position, ray.Direction, out hit))
            {
                Vector3 lookDirection = hit.Point - Actor.Position;
                Quaternion rotation = Quaternion.LookRotation(lookDirection);
                Actor.Orientation = rotation;
            }
        }
    }
}
