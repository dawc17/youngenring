using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DKC
{
    public class WorldSaveGameManager : MonoBehaviour
    {
        public static WorldSaveGameManager Instance;
        [SerializeField] int worldSceneIndex = 1;

        private void Awake()
        {
            // Check if an instance of this class already exists
            if (Instance is null)
            {
                Instance = this;
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

        public IEnumerator LoadNewGame()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);
            yield return null;
        }

        public int GetWorldSceneIndex()
        {
            return worldSceneIndex;
        }
    }
}
