using Unity.Netcode;
using UnityEngine;

namespace Code.Players
{
    public class PlayerAiming : NetworkBehaviour
    {
        [field:SerializeField] public PlayerInputSO playerInput { get; private set; }
        [SerializeField] private Transform turretTransform;

        private void LateUpdate()
        {
            if (!IsOwner) return;
            Vector3 mousePosition = playerInput.AimPosition;
            Vector2 direction = (mousePosition - turretTransform.position).normalized;

            turretTransform.up = direction;
        }
    }
}
