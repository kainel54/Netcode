using Code.Core;
using Unity.Netcode;
using UnityEngine;

namespace Code.Players
{
    public class RespawningHandler : NetworkBehaviour
    {
        public override void OnNetworkSpawn()
        {
            //서버만 리스폰을 관리한다.
            if (!IsServer) return;


            //만약 이 오브젝트보다 플레이어가 먼저 스폰되었다면 그것들을 가져와서 셋팅해야 한다.
            PlayerController[] players = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);
            foreach (var player in players)
            {
                HandlePlayerSpawned(player);
            }

            PlayerController.OnPlayerSpawned += HandlePlayerSpawned;
            PlayerController.OnPlayerDeSpawned += HandlePlayerDespawned;

        }

        private void HandlePlayerSpawned(PlayerController player)
        {
            //얘는 플레이어 사망시 터칠거라서 구독해제 안해도 된다.(애초에 람다는 해제 못함)
            player.HealthCompo.OnDeadEvent += () =>
            {
                if (!IsServer) return;

                ulong clientId = player.OwnerClientId;
                int idx = player.characterIdx.Value;

                GameManager.Instance.SpawnCharacter(clientId, idx, 1f);
                Destroy(player.gameObject);
            };
        }

        private void HandlePlayerDespawned(PlayerController controller)
        {
        }

    }
}