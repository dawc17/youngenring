using UnityEngine;

namespace DKC
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        CharacterManager character;

        float vertical;
        float horizontal;
        
        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }
        
        public void UpdateAnimatorMovementParameters(float horizontalValue, float verticalValue)
        {
            character.animator.SetFloat("Horizontal", horizontalValue, 0.1f, Time.deltaTime);
            character.animator.SetFloat("Vertical", verticalValue, 0.1f, Time.deltaTime);
        }

        public virtual void PlayTargetActionAnimation(
            string targetAnimation, 
            bool isPerformingAction, 
            bool applyRootMotion = true, 
            bool canRotate = false, 
            bool canMove = false)
        {
            character.animator.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(targetAnimation, 0.2f);
            
            // used to stop character from attempting new actions
            // if you get damaged and begin performing a damage animation
            // turn true if stunned
            // basically allows and stops actions
            character.isPerformingAction = isPerformingAction;
            character.canMove = canMove;
            character.canRotate = canRotate;
        }
    }
}