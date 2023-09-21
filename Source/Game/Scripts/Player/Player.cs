
using FlaxEngine;

namespace Game
{
    public class Player : Agent
    {
        [Header("Mouse Pick")]   
        public Actor pickActor;
        public Collider pickCollider;

        float horizontal, vertical;
        Vector3 direction;

        public override void OnStart()
        {
            base.OnStart();
        }

        public override void OnUpdate()
        {
            HandleMovement();
            HandleMousePick();
            HandleShooting();
        }

        protected void HandleMovement()
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
            direction = new Vector3(horizontal, 0, vertical).Normalized;

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

        void HandleShooting()
        {
            bool isShooting = Input.GetMouseButton(MouseButton.Left);
            shootComponent.HandleShooting(isShooting);
        }

        protected override void Death()
        {
            Debug.Log("Player Has Died!");
        }
    }
}
