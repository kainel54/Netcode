using Code.Network;
using Code.Network.Client;
using Code.Network.Shared;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Multiplayer.Widgets;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientGameManager : IDisposable
{
    private JoinAllocation _allocation;
    private Code.Network.Client.NetworkClient _networkClient;
    private string _playerName;
    public string PlayerName => _playerName;
    public void SetPlayerName(string playerName)
    {
        _playerName = playerName;
    }

    public async Task<bool> InitAsync()
    {
        await UnityServices.InitializeAsync();
        _networkClient = new Code.Network.Client.NetworkClient(NetworkManager.Singleton);
        AuthState authState = await UGSAuthWrapper.DoAuth();
        return authState == AuthState.Authenticated;
    }

    public void GotoMenu()
    {
        SceneManager.LoadScene(SceneNames.MenuScene);
    }

    public async Task StartClientWithJoinCode(string joinCode)
    {
        try
        {
            _allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();

            var dtlsEndpoint = _allocation.ServerEndpoints.First(e => e.ConnectionType == "dtls");

            var relayServerData = new RelayServerData(
                dtlsEndpoint.Host,
                (ushort)dtlsEndpoint.Port,
                _allocation.AllocationIdBytes,
                _allocation.ConnectionData,
                _allocation.HostConnectionData, 
                _allocation.Key,
                isSecure: true 
            );

            transport.SetRelayServerData(relayServerData);

            UserData userData = new UserData()
            {
                username = _playerName,
                userAuthId = AuthenticationService.Instance.PlayerId
            };

            string json = JsonUtility.ToJson(userData);
            byte[] payload = Encoding.UTF8.GetBytes(json);

            NetworkManager.Singleton.NetworkConfig.ConnectionData = payload;

            NetworkManager.Singleton.StartClient();
        }
        catch (Exception e)
        {
            Debug.LogError($"Relay Join Failed: {e.Message}");
            return;
        }
    }

    public bool StartClientWithLocal()
    {
        if (!NetworkManager.Singleton.StartClient())
        {
            NetworkManager.Singleton.Shutdown();
            return false;
        }
        return true;
    }

    public void Dispose()
    {
        _networkClient?.Dispose();
    }

    public void Disconnect()
    {
        _networkClient.Disconnect();
    }

}