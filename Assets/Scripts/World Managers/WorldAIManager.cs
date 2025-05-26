using UnityEngine;
using Unity.Netcode;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace DKC
{
    public class WorldAIManager : MonoBehaviour
    {
        public static WorldAIManager instance;

        [Header("Debug")]
        [SerializeField] bool despawnCharacters = false;
        [SerializeField] bool respawnCharacters = false;

        [Header("Characters")]
        [SerializeField] private List<AICharacterSpawner> aiCharacterSpawners;
        [SerializeField] List<GameObject> spawnedCharacters;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SpawnCharacter(AICharacterSpawner spawner)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                aiCharacterSpawners.Add(spawner);
                spawner.AttemptToSpawnCharacter();
            }

        }

        private void DespawnAllCharacters()
        {
            foreach (var character in spawnedCharacters)
            {
                character.GetComponent<NetworkObject>().Despawn();
            }
        }

        private void DisableAllCharacters()
        {
            // todo: disable character gameobjects, sync disabled state across network
            // disable gameobjects for clients upon connecting, if disables status is true
            // can be used to disable characters that are far from players to save memory
            // characters can be split into areas
        }
    }
}