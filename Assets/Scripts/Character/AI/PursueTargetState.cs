using UnityEngine;
using UnityEngine.AI;

namespace DKC
{
    [CreateAssetMenu(menuName = "AI/States/Pursue Target")]
    public class PursueTargetState : AIState
    {
        public override AIState Tick(AICharacterManager aiCharacter)
        {
            // check if we are performing an action (if so do nothing until action is complete)
            if (aiCharacter.isPerformingAction)
            {
                return this; // stay in the current state
            }

            // chcek if our target is null, it it is go back to idle state
            if (aiCharacter.characterCombatManager.currentTarget == null)
            {
                return SwitchState(aiCharacter, aiCharacter.idleState);
            }

            // make sure our navmesh agent is active
            if (!aiCharacter.navMeshAgent.enabled)
            {
                aiCharacter.navMeshAgent.enabled = true;
            }

            // if we are within combat range switch to combat state

            // if target is not reachable and far away return home

            // puruse the target
            NavMeshPath path = new NavMeshPath();
            aiCharacter.navMeshAgent.CalculatePath(aiCharacter.characterCombatManager.currentTarget.transform.position, path);
            aiCharacter.navMeshAgent.SetPath(path);

            return this;
        }
    }
}
