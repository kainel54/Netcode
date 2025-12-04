using Code.Network;
using Code.Players;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Code.Core
{
    public class GameManager : NetworkBehaviour
    {
        public static GameManager Instance;

        [SerializeField] private CharacterSelectUI _selectUIPrefab;
        [SerializeField] private RectTransform _selectPanelTrm;
        [SerializeField] private RectTransform _parentsTrm;
        [SerializeField] private PlayerController _playerPrefab;

        private Dictionary<ulong, PlayerController> _playerDictionary;

        private CharacterSelectPanel _selectPanel;


        private void Awake()
        {
            Instance = this;
            _selectPanel = _selectPanelTrm.GetComponent<CharacterSelectPanel>();
            _playerDictionary = new Dictionary<ulong, PlayerController>();
        }
        public override void OnNetworkSpawn()
        {
            if (IsClient)
            {
                PlayerController.OnPlayerSpawned += HandlePlayerSpawn;
                PlayerController.OnPlayerDeSpawned += HandlePlayerDeSpawn;
            }
        }

        public override void OnNetworkDespawn()
        {
            if (IsClient)
            {
                PlayerController.OnPlayerSpawned -= HandlePlayerSpawn;
                PlayerController.OnPlayerDeSpawned -= HandlePlayerDeSpawn;
            }
        }

        private void HandlePlayerSpawn(PlayerController controller)
        {
            ulong clientId = controller.OwnerClientId;

            if (!_playerDictionary.ContainsKey(clientId))
            {
                _playerDictionary.Add(clientId, controller);
            }
        }

        private void HandlePlayerDeSpawn(PlayerController controller)
        {
            if (_playerDictionary.ContainsKey(controller.OwnerClientId))
            {
                _playerDictionary.Remove(controller.OwnerClientId);
            }
        }


        public void CreateUIPanel(ulong clientID, string username)
        {
            CharacterSelectUI ui = Instantiate(_selectUIPrefab);
            ui.NetworkObject.SpawnAsPlayerObject(clientID);
            ui.transform.SetParent(_parentsTrm);
            ui.transform.localScale = Vector3.one;

            _selectPanel.AddUI(ui);

            ui.SetCharacterName(username);
        }

        public void StartGame(List<CharacterSelectUI> list)
        {
            foreach (CharacterSelectUI ui in list)
            {
                ulong clientID = ui.OwnerClientId;
                int idx = ui.selectedSpriteIdx;

                SpawnCharacter(clientID, idx);
            }
        }

        public async void SpawnCharacter(ulong clientID, int idx, float delay = 0)
        {
            if (delay > 0)
            {
                await Task.Delay(Mathf.CeilToInt(delay * 1000));
            }

            Vector3 position = CharacterSpawnPoint.GetRandomSpawnPos();

            PlayerController controller = Instantiate(_playerPrefab, position, Quaternion.identity);
            controller.NetworkObject.SpawnAsPlayerObject(clientID);
            controller.SetCharacterData(idx);
        }


    }
}