using UnityEngine;
using UnityEngine.AI;

namespace DKC
{
    public class AICharacterManager : CharacterManager
    {
        public AICharacterCombatManager aiCharacterCombatManager;

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
        }
    }
}