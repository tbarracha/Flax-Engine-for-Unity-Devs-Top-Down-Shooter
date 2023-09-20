
using FlaxEngine;

namespace Game
{
    public class Player : Agent
    {
        [Header("Mouse Pick")]   
        public Actor pickActor;
        public Collider pickCollider;

        [Header("Shooting")]
        public Prefab projectilePrefab;
        public Actor spawnPoint;
        public float fireRate = 3;          // projectiles per second

        float horizontal, vertical;
        Vector3 direction;

        float fireDelay, timeInCooldown;
        ShootingState shootingState;

        public override void OnStart()
        {
            base.OnStart();

            fireDelay = 1 / fireRate;
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

        void HandleShooting()
        {
            switch (shootingState)
            {
                case ShootingState.Idle:
                    ShootingState_Idle();
                    break;

                case ShootingState.Shooting:
                    ShootingState_Shooting();
                    break;

                case ShootingState.Cooldown:
                    ShootingState_Cooldown();
                    break;
            }
        }

        // Input
        void ShootingState_Idle()
        {
            if (Input.GetMouseButton(MouseButton.Left))
                ChangeShootingState(ShootingState.Shooting);
        }

        // Spawn Projectile
        void ShootingState_Shooting()
        {
            Actor projectile = PrefabManager.SpawnPrefab(projectilePrefab, spawnPoint.Position, spawnPoint.Orientation);
            ChangeShootingState(ShootingState.Cooldown);
        }

        void ShootingState_Cooldown()
        {
            timeInCooldown += Time.DeltaTime;
            if (timeInCooldown > fireDelay)
                ChangeShootingState(ShootingState.Idle);
        }

        void ChangeShootingState(ShootingState targetState)
        {
            if (shootingState == targetState)
                return;

            fireDelay = 1 / fireRate;
            shootingState = targetState;

            if (shootingState == ShootingState.Cooldown)
                timeInCooldown = 0;
        }
    }
}
