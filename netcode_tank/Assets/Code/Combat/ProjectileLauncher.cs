using System;
using Code.Players;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Combat
{
    public class ProjectileLauncher : NetworkBehaviour
    {
        [Header("Reference Variable")]
        [field: SerializeField] public PlayerInputSO PlayerInput { get; private set; }
        [SerializeField] private GameObject serverProjectilePrefab;
        [SerializeField] private GameObject clientProjectilePrefab;

        [Header("Setting Values")]
        [SerializeField] private WeaponSO weaponData;
        private Transform firePosition;

        private Weapon _currentWeapon;

        private bool _shouldFire;
        private float _prevFireTime;

        public UnityEvent OnProejectileFired;

        public override void OnNetworkSpawn()
        {
            if (!IsOwner) return;
            PlayerInput.OnAttackKeyPress += HandleAttackKeyPress;
        }

        public override void OnNetworkDespawn()
        {
            if (!IsOwner) return;
            PlayerInput.OnAttackKeyPress -= HandleAttackKeyPress;
        }

        private void HandleAttackKeyPress(bool isPressed)
        {
            _shouldFire = isPressed;
        }

        private void WeaponChange()
        {
            firePosition = weaponData.WeaponPrefab.firePosition;
        }

        private void Update()
        {
            if (!IsOwner) return;
            if (!_shouldFire) return;

            if (Time.time < _prevFireTime + 60/weaponData.RPM) return; //아직 쿨다운이면 쏘면 안된다.

            SpawnProjectileServerRpc(firePosition.position, firePosition.up);
            SpawnDummyProjectile(firePosition.position, firePosition.up);
            _prevFireTime = Time.time;
        }

        [ServerRpc]
        private void SpawnProjectileServerRpc(Vector3 position, Vector3 direction)
        {
            GameObject instance = Instantiate(serverProjectilePrefab, position, Quaternion.identity);
            instance.transform.up = direction; //회전 처리

            if (instance.TryGetComponent(out Rigidbody2D rigid))
            {
                rigid.linearVelocity = direction * weaponData.projectileSpeed;
            }

            SpawnProjectileClientRpc(position, direction);
        }

        [ClientRpc]
        private void SpawnProjectileClientRpc(Vector3 position, Vector3 direction)
        {
            if (IsOwner) return;
            SpawnDummyProjectile(position, direction);
        }

        private void SpawnDummyProjectile(Vector3 position, Vector3 direction)
        {
            GameObject instance = Instantiate(clientProjectilePrefab, position, Quaternion.identity);
            instance.transform.up = direction; //회전 처리

            OnProejectileFired?.Invoke();

            if (instance.TryGetComponent(out Rigidbody2D rigid))
            {
                rigid.linearVelocity = direction * weaponData.projectileSpeed;
            }
        }
    }
}