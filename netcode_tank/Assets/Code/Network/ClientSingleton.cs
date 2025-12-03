using System.Threading.Tasks;
using UnityEngine;

namespace Code.Network
{
    public class ClientSingleton : MonoBehaviour
    {
        private static ClientSingleton instance;

        public ClientGameManager GameManager { get; set; }
        public static ClientSingleton Instance
        {
            get
            {
                if (instance != null) return instance;

                instance = FindAnyObjectByType<ClientSingleton>();

                if (instance == null)
                {
                    Debug.LogError("No Client Singletone");
                    return null;
                }

                return instance;
            }
        }
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public async Task<bool> CreateClient()
        {
            GameManager = new ClientGameManager();

            return await GameManager.InitAsync();
        }

        private void OnDestroy()
        {
            GameManager?.Dispose();
        }
    }
}