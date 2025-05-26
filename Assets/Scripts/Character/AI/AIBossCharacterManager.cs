using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DKC
{
    public class AIBossCharacterManager : AICharacterManager
    {
        public int bossID = 0; // Unique identifier for the boss
        [SerializeField] bool hasBeenDefeated = false;
        [SerializeField] bool hasBeenAwakened = false;
        [SerializeField] private List<FogWallInteractable> fogWalls;
        // when spawned check out save file
        // if save file does not contain the boss with this id, add it
        // if it is present, check if it is defeated
        // if it is defeated, do not spawn the boss
        // if the boss has not been defeated, spawn the boss

        [Header("Debug")]
        [SerializeField] bool wakeBossUp = false;

        protected override void Update()
        {
            base.Update();

            if (wakeBossUp)
            {
                wakeBossUp = false;
                WakeBoss();
            }
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (IsServer)
            {
                if (!WorldSaveGameManager.Instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
                {
                    WorldSaveGameManager.Instance.currentCharacterData.bossesAwakened.Add(bossID, false);
                    WorldSaveGameManager.Instance.currentCharacterData.bossesDefeated.Add(bossID, false);
                }
                else
                {
                    hasBeenDefeated = WorldSaveGameManager.Instance.currentCharacterData.bossesDefeated[bossID];
                    hasBeenAwakened = WorldSaveGameManager.Instance.currentCharacterData.bossesAwakened[bossID];
                }

                StartCoroutine(GetFogWallsFromWorldObjectManager());

                // If the boss has been awakened, set the fog walls to active
                if (hasBeenAwakened)
                {
                    for (int i = 0; i < fogWalls.Count; i++)
                    {
                        fogWalls[i].isActive.Value = true;
                    }
                }

                // if the boss has been defeated, set the fog walls to inactive
                if (hasBeenDefeated)
                {
                    for (int i = 0; i < fogWalls.Count; i++)
                    {
                        fogWalls[i].isActive.Value = false;
                    }

                    // If the boss has been defeated, do not spawn it
                    aiCharacterNetworkManager.isActive.Value = false;
                }
            }
        }

        private IEnumerator GetFogWallsFromWorldObjectManager()
        {
            while (WorldObjectManager.instance.fogWalls.Count == 0)
            {
                yield return new WaitForEndOfFrame(); // Wait until the fog walls are populated
            }

            // locate fog walls
            fogWalls = new List<FogWallInteractable>();

            foreach (var fogWall in WorldObjectManager.instance.fogWalls)
            {
                if (fogWall.fogWallID == bossID)
                {
                    if (fogWall.fogWallID == bossID)
                        fogWalls.Add(fogWall);
                }
            }
        }

        public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
            if (IsOwner)
            {
                characterNetworkManager.currentHealth.Value = 0;
                isDead.Value = true;

                // reset any flags here that need to be reset

                // if we are not grounded, play an aerial death animation

                if (!manuallySelectDeathAnimation)
                {
                    characterAnimatorManager.PlayTargetActionAnimation("Dead_01", true);
                }

                hasBeenDefeated = true;

                if (!WorldSaveGameManager.Instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
                {
                    WorldSaveGameManager.Instance.currentCharacterData.bossesAwakened.Add(bossID, true);
                    WorldSaveGameManager.Instance.currentCharacterData.bossesDefeated.Add(bossID, true);
                }
                else
                {
                    WorldSaveGameManager.Instance.currentCharacterData.bossesAwakened.Remove(bossID);
                    WorldSaveGameManager.Instance.currentCharacterData.bossesDefeated.Remove(bossID);
                    WorldSaveGameManager.Instance.currentCharacterData.bossesAwakened.Add(bossID, true);
                    WorldSaveGameManager.Instance.currentCharacterData.bossesDefeated.Add(bossID, true);
                }

                WorldSaveGameManager.Instance.SaveGame();
            }

            // play death sfx

            yield return new WaitForSeconds(5);

            // award players with runes

            // disable character
        }

        public void WakeBoss()
        {
            hasBeenAwakened = true;

            if (!WorldSaveGameManager.Instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
            {
                WorldSaveGameManager.Instance.currentCharacterData.bossesAwakened.Add(bossID, true);
            }
            else
            {
                WorldSaveGameManager.Instance.currentCharacterData.bossesAwakened.Remove(bossID);
                WorldSaveGameManager.Instance.currentCharacterData.bossesAwakened.Add(bossID, true);
            }

            for (int i = 0; i < fogWalls.Count; i++)
            {
                fogWalls[i].isActive.Value = true;
            }
        }
    }
}
