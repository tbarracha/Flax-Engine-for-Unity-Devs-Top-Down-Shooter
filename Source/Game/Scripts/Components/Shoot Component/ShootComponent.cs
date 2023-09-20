

using FlaxEngine;

namespace Game
{
    public class ShootComponent : Script
    {
        [Header("Shooting")]
        public Prefab projectilePrefab;
        public Actor spawnPoint;
        public float fireRate = 3;          // projectiles per second

        float fireDelay, timeInCooldown;
        [ShowInEditor] ShootingState shootingState;

        public void HandleShooting(bool isShooting)
        {
            switch (shootingState)
            {
                case ShootingState.Idle:
                    ShootingState_Idle(isShooting);
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
        void ShootingState_Idle(bool isShooting)
        {
            if (isShooting)
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
