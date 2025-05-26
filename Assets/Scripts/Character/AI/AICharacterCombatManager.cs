using UnityEngine;

namespace DKC
{
    public class AICharacterCombatManager : CharacterCombatManager
    {
        protected AICharacterManager aiCharacter;

        [Header("Action Recovery")]
        public float actionRecoveryTimer = 0f;

        [Header("Target Information")]
        public float distanceFromTarget;
        public float viewableAngle;
        public Vector3 directionOfTarget;
        public bool enablePivot = true;

        [Header("Detection")]
        [SerializeField] private float detectionRadius = 15f;
        public float minimumDetectionAngle = -35f;
        public float maximumDetectionAngle = 35f;

        [Header("Attack Rotation Speed")]
        public float attackRotationSpeed = 25f;

        protected override void Awake()
        {
            base.Awake();

            aiCharacter = GetComponent<AICharacterManager>();
            lockOnTransform = GetComponentInChildren<LockOnTransform>().transform;
        }

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

                            if (enablePivot)
                            {
                                PivotTowardsTarget(aiCharacter);
                            }

                        }
                    }
                }
            }
        }

        public virtual void PivotTowardsTarget(AICharacterManager aiCharacter)
        {
            if (aiCharacter.isPerformingAction)
            {
                return; // Do not pivot if performing an action
            }

            if (viewableAngle >= 20 && viewableAngle <= 60)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Right_45", true);
            }
            else if (viewableAngle <= -20 && viewableAngle >= -60)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Left_45", true);
            }
            else if (viewableAngle >= 61 && viewableAngle <= 110)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Right_90", true);
            }
            else if (viewableAngle <= -61 && viewableAngle >= -110)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Left_90", true);
            }
            if (viewableAngle >= 110 && viewableAngle <= 145)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Left_135", true);
            }
            else if (viewableAngle <= -110 && viewableAngle >= -145)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Right_135", true);
            }
            if (viewableAngle >= 146 && viewableAngle <= 180)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Right_180", true);
            }
            else if (viewableAngle <= -146 && viewableAngle >= -180)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Left_180", true);
            }


        }

        public void RotateTowardsAgent(AICharacterManager aiCharacter)
        {
            if (aiCharacter.aiCharacterNetworkManager.isMoving.Value)
            {
                aiCharacter.transform.rotation = aiCharacter.navMeshAgent.transform.rotation;
            }
        }

        public void RotateTowardsTargetWhileAttacking(AICharacterManager aiCharacter)
        {
            if (currentTarget == null)
            {
                return; // No target to rotate towards
            }

            if (!aiCharacter.characterLocomotionManager.canRotate)
            {
                return; // Cannot rotate if the character is not allowed to rotate
            }

            if (!aiCharacter.isPerformingAction)
            {
                return;
            }

            Vector3 targetDirection = currentTarget.transform.position - aiCharacter.transform.position;
            targetDirection.y = 0;
            targetDirection.Normalize();

            if (targetDirection == Vector3.zero)
            {
                targetDirection = aiCharacter.transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            aiCharacter.transform.rotation = Quaternion.Slerp(aiCharacter.transform.rotation, targetRotation, attackRotationSpeed * Time.deltaTime);
        }

        public void HandleActionRecovery(AICharacterManager aiCharacter)
        {
            if (actionRecoveryTimer > 0)
            {
                if (!aiCharacter.isPerformingAction)
                {
                    actionRecoveryTimer -= Time.deltaTime;
                }
            }
        }
    }
}
