using UnityEngine;

namespace DKC
{
    public class AICharacterCombatManager : CharacterCombatManager
    {
        [Header("Viewable Angle")]
        public float viewableAngle;
        public Vector3 directionOfTarget;

        [Header("Detection")]
        [SerializeField] private float detectionRadius = 15f;
        [SerializeField] private float minimumDetectionAngle = -35f;
        [SerializeField] private float maximumDetectionAngle = 35f;

        public void FindATargetViaLineOfSight(AICharacterManager aiCharacter)
        {
            if (currentTarget != null)
            {
                return;
            }

            Collider[] colliders = Physics.OverlapSphere(aiCharacter.transform.position, detectionRadius, WorldUtilityManager.instance.GetCharacterLayers());

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();

                if (targetCharacter == null)
                    continue;

                if (targetCharacter == aiCharacter)
                    continue;

                if (targetCharacter.isDead.Value)
                    continue;

                // can i attack this character, if so, make them my target
                if (WorldUtilityManager.instance.CanIDamageThisTarget(character.characterGroup, targetCharacter.characterGroup))
                {
                    // if found, they have to be within our viewable angle
                    Vector3 directionOfTarget = targetCharacter.transform.position - aiCharacter.transform.position;
                    float angleOfTarget = Vector3.Angle(directionOfTarget, aiCharacter.transform.forward);

                    if (angleOfTarget > minimumDetectionAngle && angleOfTarget < maximumDetectionAngle)
                    {
                        if (Physics.Linecast(
                            aiCharacter.characterCombatManager.lockOnTransform.position,
                            targetCharacter.characterCombatManager.lockOnTransform.position,
                            WorldUtilityManager.instance.GetEnvironmentLayers()))
                        {
                            Debug.DrawLine(aiCharacter.characterCombatManager.lockOnTransform.position, targetCharacter.characterCombatManager.lockOnTransform.position);
                            Debug.Log("BLOOOCKED");
                        }
                        else
                        {
                            directionOfTarget = targetCharacter.transform.position - aiCharacter.transform.position;
                            viewableAngle = WorldUtilityManager.instance.GetAngleOfTarget(transform, directionOfTarget);
                            aiCharacter.characterCombatManager.SetTarget(targetCharacter);
                            Debug.Log($"Target acquired: {targetCharacter.name}");
                            PivotTowardsTarget(aiCharacter);
                        }
                    }
                }
            }
        }

        public void PivotTowardsTarget(AICharacterManager aiCharacter)
        {
            if (aiCharacter.isPerformingAction)
            {
                return; // Do not pivot if performing an action
            }

            if (viewableAngle >= 61 && viewableAngle <= 110)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Zombie_Turn_Right90", true);
            }
            else if (viewableAngle <= -61 && viewableAngle >= -110)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Zombie_Turn_Left90", true);
            }

            if (viewableAngle >= 146 && viewableAngle <= 180)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Zombie_Turn_180", true);
            }
            else if (viewableAngle <= -146 && viewableAngle >= -180)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Zombie_Turn_180", true);
            }
        }
    }
}
