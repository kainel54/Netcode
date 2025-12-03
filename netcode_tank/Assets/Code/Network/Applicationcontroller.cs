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
            //차후 데디케이트 서버를 만들기 위한 구조
            LaunchInMode(SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null);
        }

        private async Task LaunchInMode(bool isDedicatedServer)
        {
            if (isDedicatedServer)
            {
                //do something in later
            }
            else
            {
                HostSingleton hostSingleton = Instantiate(_hostPrefab); //순서바꾸면 안돼
                hostSingleton.CreateHost();

                ClientSingleton clientSingleton = Instantiate(_clientPrefab);
                bool authenticated = await clientSingleton.CreateClient();

                if (authenticated)
                {
                    //차후 이곳에 에셋 로딩부분이 들어가야 한다.
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