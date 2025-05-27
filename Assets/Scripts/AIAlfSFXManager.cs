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

        [Header("Intro Quip")]
        public AudioClip introQuip;

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

        public virtual void PlayIntroQuip()
        {
            if (introQuip != null)
            {
                audioSource.PlayOneShot(introQuip);
                audioSource.pitch = 1; // Reset pitch to normal
            }
        }
    }
}
