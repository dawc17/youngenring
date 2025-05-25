using UnityEngine;

namespace DKC
{
    [CreateAssetMenu(menuName = "AI/States/Idle")]
    public class IdleState : AIState
    {
        public override AIState Tick(AICharacterManager aiCharacter)
        {
            if (aiCharacter.characterCombatManager.currentTarget != null)
            {
                Debug.Log("Found a target, commencing fentanyl extraction procedure.");

                return this;
            }
            else
            {
                aiCharacter.aiCharacterCombatManager.FindATargetViaLineOfSight(aiCharacter);
                Debug.Log("Looking for a target...");
                return this;
            }
        }
    }
}
