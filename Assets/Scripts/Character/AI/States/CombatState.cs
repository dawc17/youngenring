using System.Collections.Generic;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using UnityEngine.AI;

namespace DKC
{
    [CreateAssetMenu(menuName = "AI/States/Combat State")]
    public class CombatState : AIState
    {
        // 1. select an attack for the attack state depending on angle, distance and weight of attack
        // 2. process combat logic while waiting to attack
        // 3. if target moves out of combat range, switch to pursue target
        // 4. if target is not present switch to idle

        [Header("Attacks")]
        public List<AICharacterAttackAction> aiCharacterAttacks; // list of all attacks this character can do
        [SerializeField] protected List<AICharacterAttackAction> potentialAttacks; // all attacks possible in current situation
        [SerializeField] private AICharacterAttackAction selectedAttack; // the attack that will be performed
        [SerializeField] private AICharacterAttackAction previousAttack; // the last attack that was performed, used for combos
        protected bool hasAttack = false; // if we have selected an attack to perform

        [Header("Combo")]
        [SerializeField] protected bool canPerformCombo = false;
        [SerializeField] protected int chanceToPerformCombo = 25; // the chance (in percent) of the character to perform a combo on the next attack
        [SerializeField] protected bool hasRolledForComboChance = false;

        [Header("Pivot")]
        [SerializeField] protected bool enablePivot; // if the character can pivot towards the target

        [Header("Engagement Distance")]
        [SerializeField] public float maximumEngagementDistance = 5; // the distance we have to be away from the target before we enter the pursue target state

        public override AIState Tick(AICharacterManager aiCharacter)
        {
            if (aiCharacter.isPerformingAction)
                return this; // if we are performing an action, stay in combat state

            if (!aiCharacter.navMeshAgent.enabled)
                aiCharacter.navMeshAgent.enabled = true; // enable navmesh agent if it is disabled

            if (!aiCharacter.aiCharacterNetworkManager.isMoving.Value)
            {
                if (aiCharacter.aiCharacterCombatManager.viewableAngle < -30 || aiCharacter.aiCharacterCombatManager.viewableAngle > 30)
                    aiCharacter.aiCharacterCombatManager.PivotTowardsTarget(aiCharacter);
            }

            aiCharacter.aiCharacterCombatManager.RotateTowardsAgent(aiCharacter);

            if (aiCharacter.aiCharacterCombatManager.currentTarget == null)
            {
                return SwitchState(aiCharacter, aiCharacter.idleState);
            }

            if (!hasAttack)
            {
                GetNewAttack(aiCharacter);
            }
            else
            {
                aiCharacter.attackState.currentAttack = selectedAttack;

                // roll for combo

                return SwitchState(aiCharacter, aiCharacter.attackState);
            }

            // if out of engagement distance, switch to pursue target state
            if (aiCharacter.aiCharacterCombatManager.distanceFromTarget > maximumEngagementDistance)
            {
                return SwitchState(aiCharacter, aiCharacter.pursueTargetState);
            }

            NavMeshPath path = new NavMeshPath();
            aiCharacter.navMeshAgent.CalculatePath(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position, path);
            aiCharacter.navMeshAgent.SetPath(path);

            return this; // stay in combat state
        }

        protected virtual void GetNewAttack(AICharacterManager aiCharacter)
        {
            potentialAttacks = new List<AICharacterAttackAction>();

            foreach (var potentialAttack in aiCharacterAttacks)
            {
                // too close
                if (potentialAttack.minimumAttackDistance > aiCharacter.aiCharacterCombatManager.distanceFromTarget)
                    continue;
                // too far
                if (potentialAttack.maximumAttackDistance < aiCharacter.aiCharacterCombatManager.distanceFromTarget)
                    continue;

                // too wide angle
                if (potentialAttack.minimumAttackAngle > aiCharacter.aiCharacterCombatManager.viewableAngle)
                    continue;
                // too narrow angle
                if (potentialAttack.maximumAttackAngle < aiCharacter.aiCharacterCombatManager.viewableAngle)
                    continue;

                potentialAttacks.Add(potentialAttack);
            }

            if (potentialAttacks.Count <= 0)
                return;

            var totalWeight = 0;

            foreach (var attack in potentialAttacks)
            {
                totalWeight += attack.attackWeight;
            }

            var randomWeight = Random.Range(1, totalWeight + 1);
            var processedWeight = 0;

            foreach (var attack in potentialAttacks)
            {
                processedWeight += attack.attackWeight;

                if (randomWeight <= processedWeight)
                {
                    selectedAttack = attack;
                    previousAttack = selectedAttack; // store the previous attack for combo logic
                    hasAttack = true;
                    return;
                }
            }
            // 1. sort through all possible attacks
            // 2. remove attacks that cannot be used in this situation
            // 3. place remaining attacks into a list
            // 4. pick one randomly based on weight
        }

        protected virtual bool RollForOutcomeChance(int outcomeChance)
        {
            bool outcomeWillBePerformed = false;

            int randomPercentage = Random.Range(0, 100);

            if (randomPercentage < outcomeChance)
                outcomeWillBePerformed = true;

            return outcomeWillBePerformed;
        }

        protected override void ResetStateFlags(AICharacterManager aICharacter)
        {
            base.ResetStateFlags(aICharacter);

            hasAttack = false;
            hasRolledForComboChance = false;
        }
    }

}
