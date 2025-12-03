using Unity.Netcode;
using UnityEngine;

namespace Code.Players
{
    public class PlayerController : NetworkBehaviour
    {
        public NetworkVariable<int> characterIdx;

        public PlayerVisual VisualCompo { get; private set; }
        public PlayerMovement MovementCompo { get; private set; }

        private void Awake()
        {
            characterIdx = new NetworkVariable<int>();
            VisualCompo = GetComponent<PlayerVisual>();
            MovementCompo = GetComponent<PlayerMovement>();
        }

        //차후 데이터로(이건 서버만 실행)
        public void SetTankData(int idx)
        {
            characterIdx.Value = idx;
        }

        public override void OnNetworkSpawn()
        {
            //차후 이부분이 데이터 체인지로 변경되어야 해.
            characterIdx.OnValueChanged += HandleColorChanged;
        }

        private void HandleColorChanged(int previousIdx, int newIdx)
        {
            VisualCompo.SetSprite(newIdx);
        }

    }
}