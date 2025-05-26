using System.Collections;
using UnityEngine;

namespace DKC
{
    public class AIBossCharacterManager : AICharacterManager
    {
        public int bossID = 0; // Unique identifier for the boss
        [SerializeField] bool hasBeenDefeated = false;
        // when spawned check out save file
        // if save file does not contain the boss with this id, add it
        // if it is present, check if it is defeated
        // if it is defeated, do not spawn the boss
        // if the boss has not been defeated, spawn the boss

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

                    if (hasBeenDefeated)
                    {
                        // If the boss has been defeated, do not spawn it
                        aiCharacterNetworkManager.isActive.Value = false;
                    }
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
    }
}
