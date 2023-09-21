

using FlaxEngine;

namespace Game
{
    public class HealthComponent : Script
    {
        [Serialize, ShowInEditor] int maxHealth     = 10;
        [Serialize, ShowInEditor] int health        = 10;
        [ShowInEditor] float healthPercent          = 1;
        [ShowInEditor] bool isDead                  = false;

        public readonly CustomEvent<int>            OnHealthChanged         = new CustomEvent<int>();
        public readonly CustomEvent<float>          OnHealthPercentChanged  = new CustomEvent<float>();
        public readonly CustomEvent<int, float>     OnHealthValuesChanged   = new CustomEvent<int, float>();

        public readonly CustomEvent                 OnDeath                 = new CustomEvent();

        public void SetHealth(int health)
        {
            this.health = health;
            HealthChanged();
        }

        public void SetMaxHealth(int maxHealth)
        {
            this.maxHealth = maxHealth;
            HealthChanged();
        }

        public void SetHealthValues(int maxHealth, int health)
        {
            this.maxHealth  = maxHealth;
            this.health     = health;
            HealthChanged();
        }

        public void ApplyDamage(int damage)
        {
            if (isDead == true)
                return;

            health = Mathf.Clamp(health - damage, 0, maxHealth);
            HealthChanged();
        }

        public void ApplyHeal(int heal)
        {
            if (isDead == true)
                return;

            health = Mathf.Clamp(health + heal, 0, maxHealth);
            HealthChanged();
        }

        public void Kill()
        {
            if (isDead == true)
                return;

            health = 0;
            HealthChanged();
        }

        public void Revive()
        {
            health = maxHealth;
            HealthChanged();
        }

        void HealthChanged()
        {
            healthPercent = health / maxHealth;

            OnHealthChanged?.Invoke(health);
            OnHealthPercentChanged?.Invoke(healthPercent);
            OnHealthValuesChanged?.Invoke(health, healthPercent);

            if (health <= 0)
                Death();
        }

        void Death()
        {
            if (isDead == true)
                return;

            isDead = true;
            OnDeath?.Invoke();
        }
    }
}
