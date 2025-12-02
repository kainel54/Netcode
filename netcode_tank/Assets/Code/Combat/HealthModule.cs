using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Combat
{
    public class HealthModule : NetworkBehaviour
    {
        public NetworkVariable<int> currentHealth = new NetworkVariable<int>();

        public int maxHealth = 100;

        public event Action OnDeadEvent;
        public UnityEvent<HealthModule> OnHealthChangeEvent;

        public bool IsDead { get; private set; } = false;

        public override void OnNetworkSpawn()
        {
            if (IsClient)
            {
                currentHealth.OnValueChanged += HandleChangeHealth;
                HandleChangeHealth(0, maxHealth); //처음에 값을 갱신한다.
            }

            if (!IsServer) return; //네트워크 변수 값은 서버만 건드린다.
            currentHealth.Value = maxHealth;
        }

        public override void OnNetworkDespawn()
        {
            if (IsClient)
            {
                currentHealth.OnValueChanged -= HandleChangeHealth;
            }
        }

        public float GetNormalizedHealth() => currentHealth.Value / (float)maxHealth;

        private void HandleChangeHealth(int previousValue, int newValue)
            => OnHealthChangeEvent?.Invoke(this);

        public void TakeDamage(int damageAmount)
            => ModifyHealth(-damageAmount);

        public void RestoredHealth(int healAmount)
            => ModifyHealth(healAmount);

        private void ModifyHealth(int amount)
        {
            if (IsDead) return;
            currentHealth.Value = Mathf.Clamp(currentHealth.Value + amount, 0, maxHealth);
            if (currentHealth.Value <= 0)
            {
                OnDeadEvent?.Invoke();
                IsDead = true;
            }
        }
    }
}