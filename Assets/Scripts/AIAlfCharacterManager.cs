using UnityEngine;

namespace DKC
{
    public class AIAlfCharacterManager : AIBossCharacterManager
    {
        public AIAlfSFXManager aiAlfSFXManager;
        AiBossCharacterNetworkManager aiBossCharacterNetworkManager;

        protected override void Awake()
        {
            base.Awake();
            aiAlfSFXManager = GetComponent<AIAlfSFXManager>();

            aiBossCharacterNetworkManager = GetComponent<AiBossCharacterNetworkManager>();
            aiBossCharacterNetworkManager.maxHealth.Value = 5300;
            aiBossCharacterNetworkManager.currentHealth.Value = aiBossCharacterNetworkManager.maxHealth.Value;
        }
    }
}
