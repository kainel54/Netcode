using System;
using UnityEngine;

namespace Code.Combat
{
    public class DealDamageOnContact : MonoBehaviour
    {
        [SerializeField] private int damage = 10;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.attachedRigidbody is null) return;

            if(other.attachedRigidbody.TryGetComponent(out HealthModule healthModule))
            {
                healthModule.TakeDamage(damage);
            }
        }
    }
}