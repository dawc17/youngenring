using Unity.Netcode;
using UnityEngine;

namespace DKC
{
    public class TitleScreenManager : MonoBehaviour
    {
        public void StartNetworkAsHost()
        {
            NetworkManager.Singleton.StartHost();
        }

        public void StartNewGame()
        {
            WorldSaveGameManager.Instance.CreateNewGame();
            StartCoroutine(WorldSaveGameManager.Instance.LoadWorldScene());
        }
    }
}
