

using FlaxEngine;
using System.Collections.Generic;

namespace Game
{
    public class ShootComponent : Script
    {
        [Header("Shooting")]
        public Prefab projectilePrefab;
        public Actor spawnPoint;
        public float fireRate = 3;              // projectiles per second
        public List<Projectile> projectiles     = new List<Projectile>();

        float fireDelay, timeInCooldown;
        [ShowInEditor] ShootingState shootingState;

        [HideInEditor] public readonly CustomEvent<Projectile> OnProjectileSpawned = new CustomEvent<Projectile>();

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
            Projectile projectile = PrefabManager.SpawnPrefab(projectilePrefab, spawnPoint.Position, spawnPoint.Orientation).GetScript<Projectile>();
            projectiles.Add(projectile);

            OnProjectileSpawned?.Invoke(projectile);
            projectile.OnProjectileDestroyed.AddListener(ProjectileDestroyed);

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

        void ProjectileDestroyed(Projectile projectile)
        {
            if (projectiles.Contains(projectile))
                projectiles.Remove(projectile);
        }
    }
}
