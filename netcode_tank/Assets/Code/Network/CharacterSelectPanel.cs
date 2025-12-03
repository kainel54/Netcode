using Code.Core;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Network
{
    public class CharacterSelectPanel : NetworkBehaviour
    {
        [SerializeField] private RectTransform _panelTrm;
        [SerializeField] private Button _startButton;
        [SerializeField] private List<CharacterSelectUI> _selectUIList;

        private void Awake()
        {
            _selectUIList = new List<CharacterSelectUI>();
        }

        private void HandleGameStart()
        {
            GameManager.Instance.StartGame(_selectUIList);
            GameStartClientRpc();//창닫으라는 명령
        }


        public override void OnNetworkSpawn()
        {
            if (IsHost)
            {
                _startButton.onClick.AddListener(HandleGameStart);
                HandleReadyChanged();
            }
            else
            {
                _startButton.gameObject.SetActive(false);
            }
        }


        public void AddUI(CharacterSelectUI ui)
        {
            _selectUIList.Add(ui);
            ui.OnDisconnected += HandleDisconnected;
            ui.OnReadyChanged += HandleReadyChanged;
        }

        public void RemoveUI(CharacterSelectUI ui)
        {
            _selectUIList.Remove(ui);
        }

        private void HandleReadyChanged()
        {
            _startButton.interactable = AllReady();
        }

        private void HandleDisconnected(CharacterSelectUI ui)
        {
            ui.OnDisconnected -= HandleDisconnected;
            RemoveUI(ui);
        }


        public bool AllReady()
        {
            //하나라도 false인게 없으면
            return _selectUIList.Count > 0 && _selectUIList.Any(x => x.isReady == false) == false;
        }

        [ClientRpc]
        private void GameStartClientRpc()
        {
            _panelTrm.gameObject.SetActive(false);
        }

    }
}