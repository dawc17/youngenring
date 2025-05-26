using UnityEngine;

namespace DKC
{
    public class AICharacterAttackAction : ScriptableObject
    {
        [Header("Attack")]
        [SerializeField] private string attackAnimation;

        [Header("Combo Action")]
        public AICharacterAttackAction comboAction;

        [Header("Action Values")]
        [SerializeField] AttackType attackType;
        public int attackWeight = 50; // Weight of the attack, used to determine how likely it is to be chosen
        public float actionRecoveryTime = 1.0f; // Time it takes before action can be performed again
        public float minimumAttackAngle = -35.0f;
        public float maximumAttackAngle = 35.0f;
        public float minimumAttackDistance = 0.0f;
        public float maximumAttackDistance = 2.0f;

        public void AttemptToPerformAction(AICharacterManager aiCharacter)
        {
            aiCharacter.characterAnimatorManager.PlayTargetAttackActionAnimation(attackType, attackAnimation, true);
        }
    }
}
