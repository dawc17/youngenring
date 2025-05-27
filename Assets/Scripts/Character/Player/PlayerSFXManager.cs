using UnityEngine;

namespace DKC
{
    public class PlayerSFXManager : CharacterSFXManager
    {
        [Header("Death Sounds")]
        [SerializeField] private AudioClip deathSound;

        protected override void Awake()
        {
            base.Awake();

            audioSource = GetComponent<AudioSource>();
            audioSource.volume = 2f;
        }

        public void PlayDeathSFX()
        {
            if (deathSound != null)
            {
                PlaySFX(deathSound, 3f, false, 0f);
            }
        }
    }
}