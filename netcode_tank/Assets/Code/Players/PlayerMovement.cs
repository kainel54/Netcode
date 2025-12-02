using Unity.Netcode;
using UnityEngine;

namespace Code.Players
{
    public class PlayerMovement : NetworkBehaviour
    {
        [Header("Reference modules")]
        [field: SerializeField] public PlayerInputSO PlayerInput { get; private set; }

        [Header("Setting values")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float turningRate = 30f; //회전속도

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

        private void Update()
        {
            if(!IsOwner) return;



           
        }

        private void FixedUpdate()
        {
            if (!IsOwner) return;
            _rigidbody.linearVelocity = transform.up *(_inputMovement.y * moveSpeed);
        }
    }
}