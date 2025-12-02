using UnityEngine;
using UnityEngine.UI;

public class MenuContainer : MonoBehaviour
{
    [SerializeField] private Button _hostBtn, _clientBtn, _exitBtn;
    [SerializeField] private ConnectPanel _connectPanel;


    private void Awake()
    {
        _hostBtn.onClick.AddListener(
            () => _connectPanel.OpenPanel(ConnectPanel.PanelRole.Host));
        _clientBtn.onClick.AddListener(
            () => _connectPanel.OpenPanel(ConnectPanel.PanelRole.Client));
        _exitBtn.onClick.AddListener(
            () => Application.Quit());
    }
}
