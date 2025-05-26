using UnityEngine;
using Unity.Netcode;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

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
        [SerializeField] List<AICharacterManager> spawnedCharacters;

        [Header("Boss Characters")]
        [SerializeField] List<AIBossCharacterManager> spawnedBossCharacters;

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

        public void AddCharacterToSpawnedCharactersList(AICharacterManager character)
        {
            if (spawnedCharacters.Contains(character))
                return;

            spawnedCharacters.Add(character);

            AIBossCharacterManager bossCharacter = character as AIBossCharacterManager;

            if (bossCharacter != null)
            {
                if (spawnedBossCharacters.Contains(bossCharacter))
                    return;

                spawnedBossCharacters.Add(bossCharacter);
            }
        }

        public AIBossCharacterManager GetBossCharacterByID(int bossID)
        {
            return spawnedBossCharacters.FirstOrDefault(boss => boss.bossID == bossID);
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