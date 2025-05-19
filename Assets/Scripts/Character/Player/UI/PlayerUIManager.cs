using Unity.Netcode;
using UnityEngine;

namespace Character.Player.UI
{
    public class PlayerUIManager : MonoBehaviour
    {
        public static PlayerUIManager instance;
        [Header("Network Join")]
        [SerializeField] bool startGameAsClient;

        private void Awake()
        {
            // Check if an instance of this class already exists
            if (instance is null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject); 
        }

        private void Update()
        {
            if (startGameAsClient)
            {
                startGameAsClient = false;
                // we must first shut down because we have a host
                NetworkManager.Singleton.Shutdown();
                // start network as client
                NetworkManager.Singleton.StartClient();
            } 
        }
    }
}
