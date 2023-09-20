
using FlaxEngine;

namespace Game
{
    public class HealthComponent : Script
    {
        [Serialize, ShowInEditor] int health    = 10;
        [Serialize, ShowInEditor] int maxHealth = 10;
        [ShowInEditor] float healthPercent = 1;
        [ShowInEditor] bool isAlive = true;

        public int Health => health;
        public int MaxHealth => maxHealth;
        public float HealthPercent => healthPercent;
        public bool IsAlive => isAlive;

        [HideInEditor] public readonly FlaxEvent<int, float>   OnHealthChanged         = new FlaxEvent<int, float>();
        [HideInEditor] public readonly FlaxEvent<int>          OnHealthAmountChanged   = new FlaxEvent<int>();
        [HideInEditor] public readonly FlaxEvent<float>        OnHealthPercentChanged  = new FlaxEvent<float>();

        [HideInEditor] public readonly FlaxEvent               OnDeath                 = new FlaxEvent();

        public void SetHealth(int health)
        {
            this.health = health;
            maxHealth = health;

            isAlive = true;
        }

        public void ApplyDamage(int amount)
        {
            if (isAlive == false)
                return;

            health = Mathf.Clamp(health - amount, 0, maxHealth);
            HealthChanged();
        }

        public void ApplyHeal(int amount)
        {
            if (isAlive == false)
                return;

            health = Mathf.Clamp(health + amount, 0, maxHealth);
            HealthChanged();
        }

        public void Kill()
        {
            health = 0;
            healthPercent = 0;

            HealthChanged();
        }

        public void Revive()
        {
            health = maxHealth;
            healthPercent = 1;
            isAlive = true;

            HealthChanged();
        }

        void HealthChanged()
        {
            healthPercent = health / maxHealth;

            OnHealthChanged?.Invoke(health, healthPercent);
            OnHealthAmountChanged?.Invoke(health);
            OnHealthPercentChanged?.Invoke(healthPercent);

            if (health <= 0)
                Death();
        }

        void Death()
        {
            isAlive = false;
            OnDeath?.Invoke();
        }
    }
}
