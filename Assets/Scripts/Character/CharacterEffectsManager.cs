using UnityEngine;

namespace DKC
{
    public class CharacterEffectsManager : MonoBehaviour
    {
        // process instant effects (healing, damage etc.)

        // process continuous effects (poison, rot etc.)

        // process static effects (armor buffs, trinkets etc.)

        CharacterManager character;

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
    }
}