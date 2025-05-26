using UnityEngine;

namespace DKC
{
    public class AIState : ScriptableObject
    {
        public virtual AIState Tick(AICharacterManager aiCharacter)
        {
            return this;
        }

        protected virtual AIState SwitchState(AICharacterManager aiCharacter, AIState newState)
        {
            ResetStateFlags(aiCharacter);
            return newState;
        }

        protected virtual void ResetStateFlags(AICharacterManager aICharacter)
        {
            // reset any state flags so when you return to this state it is clean
        }
    }
}
