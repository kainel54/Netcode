using Code.Combat;
using Code.Network;
using Code.Network.Shared;
using System;
using TMPro;
using Unity.Cinemachine;
using Unity.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

namespace Code.Players
{
    public class PlayerController : NetworkBehaviour
    {
        public NetworkVariable<int> characterIdx;

        public PlayerVisual VisualCompo { get; private set; }
        public PlayerMovement MovementCompo { get; private set; }
        public HealthModule HealthCompo { get; private set; }


        [Header("References")]
        [SerializeField] private CinemachineCamera _followCam;
        [SerializeField] private TextMeshPro _nameText;

        [Header("Setting values")]
        [SerializeField] private int _ownerCamPriority;

        public NetworkVariable<FixedString32Bytes> playerName;

        public static event Action<PlayerController> OnPlayerSpawned;
        public static event Action<PlayerController> OnPlayerDeSpawned;

        private void Awake()
        {
            characterIdx = new NetworkVariable<int>();
            playerName = new NetworkVariable<FixedString32Bytes>();
            VisualCompo = GetComponent<PlayerVisual>();
            MovementCompo = GetComponent<PlayerMovement>();
            HealthCompo = GetComponent<HealthModule>();
        }

        public void SetCharacterData(int idx)
        {
            characterIdx.Value = idx;
        }

        public override void OnNetworkSpawn()
        {
            characterIdx.OnValueChanged += HandleSpriteChanged;
            playerName.OnValueChanged += HandlePlayerNameChange;

            if (IsOwner)
            {
                _followCam.Priority = _ownerCamPriority;
            }

            if (IsServer)
            {
                UserData data = HostSingleton.Instance.GameManager.NetworkServer.GetUserDataByClientId(OwnerClientId);
                playerName.Value = data.username;
                OnPlayerSpawned?.Invoke(this);
            }
            HandlePlayerNameChange(string.Empty, playerName.Value); //처음한번

            OnPlayerSpawned?.Invoke(this);
        }


        public override void OnNetworkDespawn()
        {
            characterIdx.OnValueChanged -= HandleSpriteChanged;
            playerName.OnValueChanged -= HandlePlayerNameChange;

            if (IsServer)
            {
                OnPlayerDeSpawned?.Invoke(this);
            }

            OnPlayerDeSpawned?.Invoke(this);
        }
        private void HandlePlayerNameChange(FixedString32Bytes previousValue, FixedString32Bytes newValue)
        {
            _nameText.text = newValue.ToString();
        }


        private void HandleSpriteChanged(int previousIdx, int newIdx)
        {
            VisualCompo.SetSprite(newIdx);
        }

    }
}