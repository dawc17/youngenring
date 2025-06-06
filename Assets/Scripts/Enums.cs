using UnityEngine;

namespace DKC
{
    public class Enums : MonoBehaviour
    {

    }

    public enum CharacterSlot
    {
        CharacterSlot_01,
        CharacterSlot_02,
        CharacterSlot_03,
        CharacterSlot_04,
        CharacterSlot_05,
        NO_SLOT
    }

    public enum CharacterGroup
    {
        Team01,
        Team02,
    }

    public enum WeaponModelSlot
    {
        RightHand,
        LeftHand,
        // right hips
        // left hips
        // back
    }

    public enum AttackType
    {
        LightAttack01,
        LightAttack02,
        HeavyAttack01,
        HeavyAttack02,
        ChargedAttack01,
        ChargedAttack02,
    }
}