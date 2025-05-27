using UnityEngine;

namespace DKC
{
    public class CharacterFootstepSFXMaker : MonoBehaviour
    {
        CharacterManager character;

        AudioSource audioSource;
        GameObject steppedOnObject;

        private bool hasTouchedGround = false;
        private bool hasPlayedFootstepSFX = false;
        [SerializeField] float distanceToGround = 0.05f;

        private void Awake()
        {
            character = GetComponentInParent<CharacterManager>();
            audioSource = GetComponent<AudioSource>();
        }

        private void FixedUpdate()
        {
            CheckForFootsteps();
        }

        private void CheckForFootsteps()
        {
            if (character == null)
            {
                return;
            }

            if (!character.characterNetworkManager.isMoving.Value)
            {
                return;
            }

            RaycastHit hit;

            if (Physics.Raycast(transform.position, character.transform.TransformDirection(Vector3.down), out hit, distanceToGround, WorldUtilityManager.instance.GetEnvironmentLayers()))
            {
                hasTouchedGround = true;

                if (!hasPlayedFootstepSFX)
                    steppedOnObject = hit.transform.gameObject;
            }
            else
            {
                hasTouchedGround = false;
                hasPlayedFootstepSFX = false;
                steppedOnObject = null;
            }

            if (hasTouchedGround && !hasPlayedFootstepSFX)
            {
                hasPlayedFootstepSFX = true;
                PlayFootstepSFX();
            }
        }

        private void PlayFootstepSFX()
        {
            character.characterSFXManager.PlayFootstepSFX();
        }
    }
}
