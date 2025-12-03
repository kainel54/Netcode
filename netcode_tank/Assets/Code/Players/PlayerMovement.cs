using Unity.Netcode;
using UnityEngine;

namespace Code.Players
{
    public class PlayerMovement : NetworkBehaviour
    {
        [Header("Reference modules")]
        [field: SerializeField] public PlayerInputSO PlayerInput { get; private set; }

        [SerializeField] private Transform visual;
        [Header("Setting values")]
        [SerializeField] private float moveSpeed = 8f;

        private Rigidbody2D _rigidbody;
        private Vector2 _inputMovement;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public override void OnNetworkSpawn()
        {
            if (!IsOwner) return;

            PlayerInput.OnMoveKeyPress += HandleMoveKeyPress;
        }

        public override void OnNetworkDespawn()
        {
            if (!IsOwner) return;
            PlayerInput.OnMoveKeyPress -= HandleMoveKeyPress;
        }

        private void HandleMoveKeyPress(Vector2 movementInput) => _inputMovement = movementInput;



        private void FixedUpdate()
        {
            if (!IsOwner) return;
            _rigidbody.linearVelocity = _inputMovement * moveSpeed;
        }
    }
}