using UnityEngine;

namespace DKC
{
    public class AIAlfSFXManager : CharacterSFXManager
    {
        [Header("Club Wooshes")]
        public AudioClip[] clubWooshClips;

        [Header("Club Hits")]
        public AudioClip[] clubHitClips;

        [Header("Foot Impacts")]
        public AudioClip[] footImpactClips;

        public virtual void PlayClubHitSFX()
        {
            if (clubHitClips.Length > 0)
                PlaySFX(WorldSFXManager.instance.ChooseRandomSFXFromArray(clubHitClips));
        }

        public virtual void PlayStompSFX()
        {
            if (footImpactClips.Length > 0)
                PlaySFX(WorldSFXManager.instance.ChooseRandomSFXFromArray(footImpactClips));
        }
    }
}
