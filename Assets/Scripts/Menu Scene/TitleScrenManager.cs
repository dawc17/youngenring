using Unity.Netcode;
using UnityEngine;

namespace DKC
{
    public class TitleScrenManager : MonoBehaviour
    {
        public void StartNetworkAsHost()
        {
            NetworkManager.Singleton.StartHost();
        }

        public void StartNewGame()
        {
            StartCoroutine(WorldSaveGameManager.Instance.LoadNewGame());
        }
    }
}
