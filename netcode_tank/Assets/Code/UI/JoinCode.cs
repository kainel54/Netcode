using Code.Network;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class JoinCode : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI joinCodeTxt;

    public override void OnNetworkSpawn()
    {
        joinCodeTxt.text = $"JoinCode :{HostSingleton.Instance.GameManager.GetJoinCode()} "; 
    }
}
