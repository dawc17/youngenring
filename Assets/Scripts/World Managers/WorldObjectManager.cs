using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace DKC
{
    public class WorldObjectManager : MonoBehaviour
    {
        public static WorldObjectManager instance;

        [Header("Objects")]
        [SerializeField] private List<NetworkObjectSpawner> networkObjectSpawners;
        [SerializeField] List<GameObject> spawnedObjects;

        [Header("Fog Walls")]
        public List<FogWallInteractable> fogWalls;

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

        public void SpawnObject(NetworkObjectSpawner spawner)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                networkObjectSpawners.Add(spawner);
                spawner.AttemptToSpawnCharacter();
            }
        }

        public void AddFogWallToList(FogWallInteractable fogWall)
        {
            if (!fogWalls.Contains(fogWall))
            {
                fogWalls.Add(fogWall);
            }
        }

        public void RemoveFogWallFromList(FogWallInteractable fogWall)
        {
            if (fogWalls.Contains(fogWall))
            {
                fogWalls.Remove(fogWall);
            }
        }
    }
}
