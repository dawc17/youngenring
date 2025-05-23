using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

namespace DKC
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        CharacterManager character;

        int vertical;
        int horizontal;

        [Header("Damage Animations")]
        public string lastDamageAnimationPlayed;

        [SerializeField] string hit_ForwardMedium01 = "hit_ForwardMedium01";
        [SerializeField] string hit_ForwardMedium02 = "hit_ForwardMedium02";
        [SerializeField] string hit_BackwardMedium01 = "hit_BackwardMedium01";
        [SerializeField] string hit_BackwardMedium02 = "hit_BackwardMedium02";
        [SerializeField] string hit_LeftMedium01 = "hit_LeftMedium01";
        [SerializeField] string hit_LeftMedium02 = "hit_LeftMedium02";
        [SerializeField] string hit_RightMedium01 = "hit_RightMedium01";
        [SerializeField] string hit_RightMedium02 = "hit_RightMedium02";
        public List<string> forwardMediumDamageAnimations = new List<string>();
        public List<string> backwardMediumDamageAnimations = new List<string>();
        public List<string> leftMediumDamageAnimations = new List<string>();
        public List<string> rightMediumDamageAnimations = new List<string>();

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();

            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        protected virtual void Start()
        {
            // Initialize damage animations lists
            forwardMediumDamageAnimations.Add(hit_ForwardMedium01);
            forwardMediumDamageAnimations.Add(hit_ForwardMedium02);

            backwardMediumDamageAnimations.Add(hit_BackwardMedium01);
            backwardMediumDamageAnimations.Add(hit_BackwardMedium02);

            leftMediumDamageAnimations.Add(hit_LeftMedium01);
            leftMediumDamageAnimations.Add(hit_LeftMedium02);

            rightMediumDamageAnimations.Add(hit_RightMedium01);
            rightMediumDamageAnimations.Add(hit_RightMedium02);
        }

        public string GetRandomAnimationFromList(List<string> animationList)
        {
            List<string> finalList = new List<string>();

            foreach (var item in animationList)
            {
                finalList.Add(item);
            }
            finalList.Remove(lastDamageAnimationPlayed);

            // check the list for null entries or empty strings
            for (int i = finalList.Count - 1; i > -1; i--)
            {
                if (finalList[i] == null || finalList[i] == "")
                {
                    finalList.RemoveAt(i);
                }
            }

            int randomValue = Random.Range(0, finalList.Count);

            return finalList[randomValue];

        }

        public void UpdateAnimatorMovementParameters(float horizontalValue, float verticalValue, bool isSprinting)
        {
            float horizontalAmount = horizontalValue;
            float verticalAmount = verticalValue;

            if (isSprinting)
            {
                verticalAmount = 2;
            }

            character.animator.SetFloat(horizontal, horizontalAmount, 0.1f, Time.deltaTime);
            character.animator.SetFloat(vertical, verticalAmount, 0.1f, Time.deltaTime);

        }

        public virtual void PlayTargetActionAnimation(
            string targetAnimation,
            bool isPerformingAction,
            bool applyRootMotion = true,
            bool canRotate = false,
            bool canMove = false)
        {
            Debug.Log($"Playing Target Action Animation: {targetAnimation}");
            character.animator.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(targetAnimation, 0.2f);

            // used to stop character from attempting new actions
            // if you get damaged and begin performing a damage animation
            // turn true if stunned
            // basically allows and stops actions
            character.isPerformingAction = isPerformingAction;
            character.canMove = canMove;
            character.canRotate = canRotate;

            character.characterNetworkManager.NotifyServerOfActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
        }
        public virtual void PlayTargetAttackActionAnimation(AttackType attackType,
            string targetAnimation,
            bool isPerformingAction,
            bool applyRootMotion = true,
            bool canRotate = false,
            bool canMove = false)
        {
            if (character.isDead.Value)
                return;
            // keep track of last attack performed (combos)
            // keep track of current attck type
            // update animation set to current weapon animations
            // decide if our attack can be parried
            // tell the netwok manager if we are in an attacking flag (counter damage)
            character.characterCombatManager.currentAttackType = attackType;
            character.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(targetAnimation, 0.2f);

            character.isPerformingAction = isPerformingAction;
            character.canMove = canMove;
            character.canRotate = canRotate;

            character.characterNetworkManager.NotifyServerOfAttackActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
        }
    }
}