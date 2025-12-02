using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class NetworkButtonUI : MonoBehaviour
    {
        [SerializeField] private Button hostButton;
        [SerializeField] private Button clientButton;

        private void Awake()
        {
            hostButton.onClick.AddListener(HandleHostBtn);
            clientButton.onClick.AddListener(HandleClientBtn);
        }
        private void HandleHostBtn()
        {
            NetworkManager.Singleton.StartHost();
        }

        private void HandleClientBtn()
        {
            NetworkManager.Singleton.StartClient();
        }

        
    }
}