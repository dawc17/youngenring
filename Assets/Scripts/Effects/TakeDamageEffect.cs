using DKC;
using UnityEngine;

namespace DKC
{
    public class TakeDamageEffect : InstantCharacterEffect
    {
        [Header("Character Causing Damage")] 
        public CharacterManager characterCausingDamage;

        [Header("Damage")] 
        public float physicalDamage = 0;
        public float
        public override void ProcessEffect(CharacterManager character)
        {
            base.ProcessEffect(character);
        }
    }
}