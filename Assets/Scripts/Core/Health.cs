using System;
using UnityEngine;

namespace Game.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 100f;
        public Action<float, float> OnHealthSet;
        public Action<float> OnDamaged;
        public Action OnDeath;
        public Action OnHealthRestored;

        private float currentHealth = 0.0f;

        public float CurrentHealth { get => currentHealth; }
        public float MaxHealth { get => maxHealth; set=> maxHealth=value; }

        private void SetHealth(float health, float maxHealth)
        {
            currentHealth = Mathf.Clamp(health, 0.0f, this.maxHealth);
        }

        private void TakeDamage(float amount)
        {
            OnHealthSet?.Invoke(currentHealth - amount, maxHealth);

            if(currentHealth == 0.0f)
            {
                OnDeath?.Invoke();
            }
        }

        private void RestoreHealth()
        {
            OnHealthSet?.Invoke(maxHealth, maxHealth);
        }
    
        private void Start() 
        {
            OnHealthSet += SetHealth;
            OnDamaged += TakeDamage;
            OnHealthRestored += RestoreHealth;
            OnHealthSet?.Invoke(maxHealth, maxHealth);
        }

        private void OnDestroy() 
        {
            OnHealthSet -= SetHealth;
            OnDamaged -= TakeDamage;
            OnHealthRestored -= RestoreHealth;
        }
    }
}

