using UnityEngine;

namespace DKC
{
    public class WorldUtilityManager : MonoBehaviour
    {
        public static WorldUtilityManager instance;

        [SerializeField] LayerMask characterLayers;
        [SerializeField] LayerMask environmentLayers;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public LayerMask GetCharacterLayers()
        {
            return characterLayers;
        }

        public LayerMask GetEnvironmentLayers()
        {
            return environmentLayers;
        }

        public bool CanIDamageThisTarget(CharacterGroup attackingCharacter, CharacterGroup targetCharacter)
        {
            if (attackingCharacter == CharacterGroup.Team01)
            {
                switch (targetCharacter)
                {
                    case CharacterGroup.Team01:
                        return false; // Cannot damage own team
                    case CharacterGroup.Team02:
                        return true; // Can damage enemy team
                    default:
                        break;
                }
            }
            else if (attackingCharacter == CharacterGroup.Team02)
            {
                switch (targetCharacter)
                {
                    case CharacterGroup.Team01:
                        return true;
                    case CharacterGroup.Team02:
                        return false;
                    default:
                        break;
                }
            }

            return false; // Default case, cannot damage unknown targets
        }

        public float GetAngleOfTarget(Transform characterTransform, Vector3 directionOfTarget)
        {
            directionOfTarget.y = 0; // Ignore vertical angle
            float viewableAngle = Vector3.Angle(characterTransform.forward, directionOfTarget);
            Vector3 cross = Vector3.Cross(characterTransform.forward, directionOfTarget);

            if (cross.y < 0)
            {
                viewableAngle = -viewableAngle;
            }

            return viewableAngle; // Return the angle in degrees
        }
    }
}