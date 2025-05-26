using System.Collections.Generic;
using UnityEngine;

namespace DKC
{
    public class CombatState : AIState
    {
        // 1. select an attack for the attack state depending on angle, distance and weight of attack
        // 2. process combat logic while waiting to attack
        // 3. if target moves out of combat range, switch to pursue target
        // 4. if target is not present switch to idle

        [Header("Attacks")]
        public List<AICharacterAttackAction> aiCharacterAttacks; // list of all attacks this character can do
        protected List<AICharacterAttackAction> potentialAttacks; // all attacks possible in current situation
        private AICharacterAttackAction selectedAttack; // the attack that will be performed
        private AICharacterAttackAction previousAttack; // the last attack that was performed, used for combos

        [Header("Combo")]
        [SerializeField] protected bool canPerformCombo = false;
        [SerializeField] protected int chanceToPerformCombo = 25; // the chance (in percent) of the character to perform a combo on the next attack
        [SerializeField] protected bool hasRolledForComboChance = false;

        [Header("Engagement Distance")]
        [SerializeField] protected float maximumEngagementDistance = 5; // the distance we have to be away from the target before we enter the pursue target state

        public override AIState Tick(AICharacterManager aiCharacter)
        {
            return base.Tick(aiCharacter);
        }

        protected virtual void GetNewAttack(AICharacterManager aiCharacter)
        {
            potentialAttacks = new List<AICharacterAttackAction>();

            foreach (var potentialAttack in potentialAttacks)
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

            hasRolledForComboChance = false;
        }
    }

}
