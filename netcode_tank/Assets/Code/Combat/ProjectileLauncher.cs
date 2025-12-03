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
        [SerializeField] private GameObject weaponHolder;
        private Collider2D _playerCollider;
        [Header("Setting Values")]
        [SerializeField] private WeaponSO defaultWeaponData;
        private WeaponSO weaponData;

        private Transform firePosition;

        private Weapon _currentWeapon;

        private bool _shouldFire;
        private bool _attackPressedOnce;
        private float _prevFireTime;

        public UnityEvent OnProejectileFired;

        private void Awake()
        {
            _playerCollider = GetComponent<Collider2D>();
        }

        private void OnEnable()
        {
            WeaponChange(defaultWeaponData);
        }

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
            if (weaponData.FireType == FireType.Single)
            {
                if (isPressed)
                    _attackPressedOnce = true;  // 한 번만 발사
            }
            else
            {
                _shouldFire = isPressed; // Auto면 눌린 상태 유지
            }
        }

        public void WeaponChange(WeaponSO weaponSO)
        {
            if (_currentWeapon != null)
                Destroy(_currentWeapon.gameObject);
            weaponData = weaponSO;
            _currentWeapon = Instantiate(weaponData.WeaponPrefab, weaponHolder.transform);
            firePosition = _currentWeapon.firePosition;
        }

        private void Update()
        {
            if (!IsOwner) return;

            bool canFire = weaponData.FireType == FireType.Automatic
            ? _shouldFire
            : _attackPressedOnce;
            if (!canFire) return;

            if (Time.time < _prevFireTime + 60 / weaponData.RPM) return; //아직 쿨다운이면 쏘면 안된다.


            SpawnProjectileServerRpc(firePosition.position, firePosition.up);
            SpawnDummyProjectile(firePosition.position, firePosition.up);

            if (weaponData.FireType == FireType.Single)
                _attackPressedOnce = false;
            _prevFireTime = Time.time;
        }

        [ServerRpc]
        private void SpawnProjectileServerRpc(Vector3 position, Vector3 direction)
        {
            GameObject instance = Instantiate(serverProjectilePrefab, position, Quaternion.identity);
            instance.transform.up = direction; //회전 처리

            Physics2D.IgnoreCollision(_playerCollider, instance.GetComponent<Collider2D>());
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