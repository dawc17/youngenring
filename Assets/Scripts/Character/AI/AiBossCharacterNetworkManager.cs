using UnityEngine;

namespace DKC
{
    public class AiBossCharacterNetworkManager : AICharacterNetworkManager
    {
        AIBossCharacterManager aiBossCharacter;

        protected override void Awake()
        {
            base.Awake();

            aiBossCharacter = GetComponent<AIAlfCharacterManager>();
        }
    }
}
