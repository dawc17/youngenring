using UnityEngine;

namespace DKC
{
    public class AIState : ScriptableObject
    {
        public virtual AIState Tick(AICharacterManager aiCharacter)
        {
            Debug.Log("we are running this state");
            return this;
        }
    }
}
