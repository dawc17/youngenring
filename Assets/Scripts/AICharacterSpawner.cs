using UnityEngine;
using Unity.Netcode;

namespace DKC
{
    public class AICharacterSpawner : MonoBehaviour
    {
        [Header("Character")]
        [SerializeField] GameObject characterGameObject;
        [SerializeField] GameObject instantiatedGameObject;

        private void Awake()
        {

        }

        private void Start()
        {
            WorldAIManager.instance.SpawnCharacter(this);
            gameObject.SetActive(false); // Disable the spawner object after spawning
        }

        public void AttemptToSpawnCharacter()
        {
            if (characterGameObject != null)
            {
                instantiatedGameObject = Instantiate(characterGameObject);
                instantiatedGameObject.transform.position = transform.position;
                instantiatedGameObject.transform.rotation = transform.rotation;
                instantiatedGameObject.GetComponent<NetworkObject>().Spawn();
                WorldAIManager.instance.AddCharacterToSpawnedCharactersList(instantiatedGameObject.GetComponent<AICharacterManager>());
            }
        }
    }
}
