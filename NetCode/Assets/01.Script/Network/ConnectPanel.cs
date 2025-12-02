using System.Text.RegularExpressions;
using TMPro;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UnityEngine.SceneManagement;

public class ConnectPanel : MonoBehaviour
{
    public enum PanelRole
    {
        Host,
        Client
    }

    [SerializeField] private TMP_InputField _tmpIpInput, _tmpPortInput;
    [SerializeField] private Button _tmpConnectBtn, _tmpCloseBtn;

    private CanvasGroup _canvasGroup;
    private PanelRole _role;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _tmpCloseBtn.onClick.AddListener(() => SetActive(false));
        _tmpConnectBtn.onClick.AddListener(HandleConnectBtn);
    }


    public void OpenPanel(PanelRole role)
    {
        _role = role;
        SetActive(true);
    }

    private void HandleConnectBtn()
    {
        if (SetUpNetworkPassport())
        {
            if (_role == PanelRole.Host)
            {
                NetworkManager.Singleton.StartHost();
                NetworkManager.Singleton.SceneManager.LoadScene(
                    SceneNames.Select, LoadSceneMode.Single);
            }
            else if (_role == PanelRole.Client)
            {
                NetworkManager.Singleton.StartClient();
            }
        }
    }

    private void SetActive(bool value)
    {
        _canvasGroup.interactable = value;
        _canvasGroup.blocksRaycasts = value;
        float alpha = value ? 1f : 0f;
        _canvasGroup.DOFade(alpha, 0.4f);
    }


    private bool SetUpNetworkPassport()
    {
        var ip = _tmpIpInput.text;
        var port = _tmpPortInput.text;

        var portRegex = new Regex(@"^[0-9]{3,5}$");
        var ipRegex = new Regex(@"^[0-9\.]+$");

        var portMatch = portRegex.Match(port);
        var ipMatch = ipRegex.Match(ip);

        if (!portMatch.Success || !ipMatch.Success)
        {
            Debug.LogError("Wrong ip or port");
            return false;
        }

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(
            ip,
            (ushort)int.Parse(port)
        );
        return true;
    }

}
