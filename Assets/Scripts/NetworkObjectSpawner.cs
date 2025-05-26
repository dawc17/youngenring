using UnityEngine;
using Unity.Netcode;

namespace DKC
{
    public class NetworkObjectSpawner : MonoBehaviour
    {
        [Header("Object")]
        [SerializeField] GameObject networkGameObject;
        [SerializeField] GameObject instantiatedGameObject;

        private void Awake()
        {

        }

        private void Start()
        {
            WorldObjectManager.instance.SpawnObject(this);
            gameObject.SetActive(false); // Disable the spawner object after spawning
        }

        public void AttemptToSpawnCharacter()
        {
            if (networkGameObject != null)
            {
                instantiatedGameObject = Instantiate(networkGameObject);
                instantiatedGameObject.transform.position = transform.position;
                instantiatedGameObject.transform.rotation = transform.rotation;
                instantiatedGameObject.GetComponent<NetworkObject>().Spawn();
            }
        }
    }
}
