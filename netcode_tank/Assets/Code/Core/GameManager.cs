using Code.Network;
using Code.Players;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Code.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [SerializeField] private CharacterSelectUI _selectUIPrefab;
        [SerializeField] private RectTransform _selectPanelTrm;
        [SerializeField] private RectTransform _parentsTrm;
        [SerializeField] private PlayerController _playerPrefab;

        private CharacterSelectPanel _selectPanel;


        private void Awake()
        {
            Instance = this;
            _selectPanel = _selectPanelTrm.GetComponent<CharacterSelectPanel>();
        }

        public void CreateUIPanel(ulong clientID, string username)
        {
            CharacterSelectUI ui = Instantiate(_selectUIPrefab);
            ui.NetworkObject.SpawnAsPlayerObject(clientID);
            ui.transform.SetParent(_parentsTrm);
            ui.transform.localScale = Vector3.one;

            _selectPanel.AddUI(ui);

            ui.SetTankName(username);
        }

        public void StartGame(List<CharacterSelectUI> list)
        {
            foreach (CharacterSelectUI ui in list)
            {
                ulong clientID = ui.OwnerClientId;
                int idx = ui.selectedSpriteIdx;

                SpawnTank(clientID, idx);
            }
        }

        public async void SpawnTank(ulong clientID, int idx, float delay = 0)
        {
            if (delay > 0)
            {
                await Task.Delay(Mathf.CeilToInt(delay * 1000));
            }

            Vector3 position = CharacterSpawnPoint.GetRandomSpawnPos();

            PlayerController controller = Instantiate(_playerPrefab, position, Quaternion.identity);
            controller.NetworkObject.SpawnAsPlayerObject(clientID);
            controller.SetTankData(idx);
        }


    }
}