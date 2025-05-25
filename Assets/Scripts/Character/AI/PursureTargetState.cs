using UnityEngine;

namespace DKC
{
    public class PursureTargetState : AIState
    {
        public override AIState Tick(AICharacterManager aiCharacter)
        {
            return base.Tick(aiCharacter);

            // check if we are performing an action (if so do nothing until action is complete)

            // chcek if our target is null, it it is go back to idle state

            // make sure our navmesh agent is active

            // if we are within combat range switch to combat state

            // if target is not reachable return home

            // puruse the target
        }
    }
}
