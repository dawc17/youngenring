using UnityEngine;

namespace DKC
{
    public class AiBossCharacterNetworkManager : AICharacterNetworkManager
    {
        AIBossCharacterManager aiBossCharacter;

        protected override void Awake()
        {
            base.Awake();

            aiBossCharacter = GetComponent<AIBossCharacterManager>();

            aiBossCharacter.aiCharacterNetworkManager.maxHealth.Value = 5000;
            aiBossCharacter.aiCharacterNetworkManager.currentHealth.Value = aiBossCharacter.aiCharacterNetworkManager.maxHealth.Value;
        }
    }
}
