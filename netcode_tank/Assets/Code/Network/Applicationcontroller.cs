using System.Threading.Tasks;
using UnityEngine;

namespace Code.Network
{
    public class ApplicationController : MonoBehaviour
    {
        [SerializeField] private ClientSingleton _clientPrefab;
        [SerializeField] private HostSingleton _hostPrefab;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            
            LaunchInMode(SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null);
        }

        private async Task LaunchInMode(bool isDedicatedServer)
        {
            if (isDedicatedServer)
            {
                
            }
            else
            {
                HostSingleton hostSingleton = Instantiate(_hostPrefab); 
                hostSingleton.CreateHost();

                ClientSingleton clientSingleton = Instantiate(_clientPrefab);
                bool authenticated = await clientSingleton.CreateClient();

                if (authenticated)
                {
                    
                    Debug.Log("Load");
                    ClientSingleton.Instance.GameManager.GotoMenu();
                }
                else
                {
                    Debug.LogError("UGS Service login failed");
                }

            }
        }
    }
}
