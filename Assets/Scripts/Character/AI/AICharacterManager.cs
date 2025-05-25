using UnityEngine;
using UnityEngine.AI;

namespace DKC
{
    public class AICharacterManager : CharacterManager
    {
        [HideInInspector] public AICharacterCombatManager aiCharacterCombatManager;
        [HideInInspector] public AICharacterNetworkManager aiCharacterNetworkManager;
        [HideInInspector] public AICharacterLocomotionManager aiCharacterLocomotionManager;

        [Header("Navmesh Agent")]
        public NavMeshAgent navMeshAgent;

        [Header("Current AI State")]
        [SerializeField] AIState currentState;

        [Header("AI States")]
        public IdleState idleState;
        public PursueTargetState pursueTargetState;
        // combat state
        // attack state

        protected override void Awake()
        {
            base.Awake();

            aiCharacterCombatManager = GetComponent<AICharacterCombatManager>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            aiCharacterNetworkManager = GetComponent<AICharacterNetworkManager>();
            aiCharacterLocomotionManager = GetComponent<AICharacterLocomotionManager>();

            idleState = Instantiate(idleState);
            pursueTargetState = Instantiate(pursueTargetState);

            currentState = idleState; // Start with the idle state
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            ProcessStateMachine();
        }

        private void ProcessStateMachine()
        {
            AIState nextState = currentState?.Tick(this);

            if (nextState != null)
            {
                currentState = nextState;
            }

            navMeshAgent.transform.localPosition = Vector3.zero; // Reset local position to avoid unwanted movement
            navMeshAgent.transform.localRotation = Quaternion.identity; // Reset local rotation to avoid unwanted rotation

            if (aiCharacterCombatManager.currentTarget != null)
            {
                aiCharacterCombatManager.directionOfTarget = aiCharacterCombatManager.currentTarget.transform.position - transform.position;
                aiCharacterCombatManager.viewableAngle = WorldUtilityManager.instance.GetAngleOfTarget(transform, aiCharacterCombatManager.directionOfTarget);
            }

            if (navMeshAgent.enabled)
            {
                Vector3 agentDestination = navMeshAgent.destination;
                float remainingDistance = Vector3.Distance(agentDestination, transform.position);

                if (remainingDistance > navMeshAgent.stoppingDistance)
                {
                    aiCharacterNetworkManager.isMoving.Value = true;
                }
                else
                {
                    aiCharacterNetworkManager.isMoving.Value = false;
                }
            }
            else
            {
                aiCharacterNetworkManager.isMoving.Value = false;
            }
        }
    }
}