using UnityEngine;

namespace DKC
{
    [CreateAssetMenu(menuName = "AI/States/Boss Sleep State")]
    public class BossSleepState : AIState
    {
        public override AIState Tick(AICharacterManager aiCharacter)
        {
            return base.Tick(aiCharacter);
        }
    }
}
