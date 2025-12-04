using System.Globalization;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Network
{
    public class CharacterSelectUI : NetworkBehaviour
    {
        public delegate void ReadyStatusChanged();
        public delegate void Disconnected(CharacterSelectUI ui);
        public event ReadyStatusChanged OnReadyChanged;
        public event Disconnected OnDisconnected;


        [SerializeField] private Image _characterImage;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private Button[] _colorButtons;
        [SerializeField] private Button _readyBtn;
        [SerializeField] private Image _statusImage;

        [SerializeField] private Sprite _readyImage;
        [SerializeField] private Sprite _notReadyImage;
        [SerializeField] private Sprite[] _characterImages;

        public bool isReady = false;
        public int selectedSpriteIdx;

        private TextMeshProUGUI _readyBtnText;
        private Image _readyBtnAttachedImage;
        private NetworkVariable<FixedString32Bytes> playerName;

        private void Awake()
        {
            _readyBtnText = _readyBtn.GetComponentInChildren<TextMeshProUGUI>();
            _readyBtnAttachedImage = _readyBtn.GetComponent<Image>();
            playerName = new NetworkVariable<FixedString32Bytes>();

            playerName.OnValueChanged += HandlePlayerNameChanged;
        }

        private void HandlePlayerNameChanged(FixedString32Bytes previousValue, FixedString32Bytes newValue)
        {
            _nameText.text = newValue.ToString();
        }

        //서버만 실행
        public void SetCharacterName(string name)
        {
            playerName.Value = name;
        }

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                OnDisconnected?.Invoke(this);
            }

            _nameText.text = playerName.Value.ToString();

            if (!IsOwner) return;

            _readyBtn.onClick.AddListener(HandleReadyBtnClick);

            isReady = false;
            SetReadyStatusVisual();

            for (int i = 0; i < _colorButtons.Length; i++)
            {
                int currentIndex = i;

                _colorButtons[i].onClick.AddListener(() =>
                {
                    SetCharacter(currentIndex);
                });
            }
        }

        public override void OnNetworkDespawn()
        {
            if (!IsOwner) return;

            playerName.OnValueChanged -= HandlePlayerNameChanged;
            _readyBtn.onClick.RemoveListener(HandleReadyBtnClick);

            foreach (Button button in _colorButtons)
            {
                button.onClick.RemoveAllListeners();
            }
        }

        private void HandleReadyBtnClick()
        {
            isReady = !isReady;

            SetReadyStatusVisual();
            SetReadyClaimToServerRpc(isReady);
        }

        private void SetCharacter(int idx)
        {
            SetCharacterVisualize(idx);
            SetColorClaimToServerRpc(idx);
        }

        private void SetCharacterVisualize(int idx)
        {
            _characterImage.sprite = _characterImages[idx];
        }

        private void SetReadyStatusVisual()
        {
            if (isReady)
            {
                _statusImage.sprite = _readyImage;
                _readyBtnText.text = "Ready!";
            }
            else
            {
                _statusImage.sprite = _notReadyImage;
                _readyBtnText.text = "Ready?";
            }
        }


        #region RPC Region
        [ServerRpc]
        public void SetColorClaimToServerRpc(int idx)
        {
            selectedSpriteIdx = idx; //서버의 컬러 변경
            SetSpriteClientRpc(idx);
            OnReadyChanged?.Invoke();
        }
        [ClientRpc]
        public void SetSpriteClientRpc(int idx)
        {
            if (IsOwner) return;
            SetCharacterVisualize(idx);
        }

        [ServerRpc]
        public void SetReadyClaimToServerRpc(bool value)
        {
            isReady = value;
            SetReadyClientRpc(isReady);
            OnReadyChanged?.Invoke();
        }

        [ClientRpc]
        public void SetReadyClientRpc(bool value)
        {
            if (IsOwner) return;
            isReady = value;
            SetReadyStatusVisual();
        }

        #endregion
    }
}