using UnityEngine;

namespace DKC
{
    public class CharacterEffectsManager : MonoBehaviour
    {
        // process instant effects (healing, damage etc.)

        // process continuous effects (poison, rot etc.)

        // process static effects (armor buffs, trinkets etc.)

        CharacterManager character;

        [Header("VFX")]
        [SerializeField] GameObject bloodSplatterVFX;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public virtual void ProcessInstantEffect(InstantCharacterEffect effect)
        {
            // take in an effect
            // process it

            effect.ProcessEffect(character);
        }

        public void PlayBloodSplatterVFX(Vector3 contactPoint)
        {
            // if we manually placed blood vfs on this model play its version
            if (bloodSplatterVFX != null)
            {
                GameObject bloodSplatter = Instantiate(bloodSplatterVFX, contactPoint, Quaternion.identity);
            }
            // else use the default we have elsewhere
            else
            {
                GameObject bloodSplatter = Instantiate(WorldCharacterEffectsManager.Instance.bloodSplatterVFX, contactPoint, Quaternion.identity);
            }
        }
    }
}