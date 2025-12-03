using Code.Network;
using UnityEngine;

namespace Code.Network
{
    public class HostSingleton : MonoBehaviour
    {
        private static HostSingleton instance;

        public HostGameManager GameManager { get; private set; }
        public static HostSingleton Instance
        {
            get
            {
                if (instance != null) return instance;

                instance = FindAnyObjectByType<HostSingleton>();

                if (instance == null)
                {
                    Debug.LogError("No Host Singletone");
                    return null;
                }

                return instance;
            }
        }
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void CreateHost()
        {
            GameManager = new HostGameManager();
        }

        private void OnDestroy()
        {
            GameManager?.Dispose();
        }
    }

}