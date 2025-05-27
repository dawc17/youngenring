using UnityEngine;


namespace DKC
{
    public class CharacterSFXManager : MonoBehaviour
    {
        public AudioSource audioSource;

        [Header("Damage Grunts")]
        [SerializeField] protected AudioClip[] damageGrunts;

        [Header("Attack Grunts")]
        [SerializeField] protected AudioClip[] attackGrunts;

        [Header("Roll SFX")]
        [SerializeField] protected AudioClip[] rollSounds;

        [Header("Footstep Sounds")]
        [SerializeField] protected AudioClip[] footstepSounds;

        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlaySFX(AudioClip sfx, float volume = 1f, bool randomizePitch = true, float pitchRandom = 0.1f)
        {
            audioSource.PlayOneShot(sfx, volume);
            // reset pitch 
            audioSource.pitch = 1;

            if (randomizePitch)
            {
                audioSource.pitch += Random.Range(-pitchRandom, pitchRandom);
            }
        }

        public virtual void PlayDamageGrunt()
        {
            PlaySFX(WorldSFXManager.instance.ChooseRandomSFXFromArray(damageGrunts));
        }

        public virtual void PlayAttackGrunt()
        {
            if (attackGrunts.Length > 0)
                PlaySFX(WorldSFXManager.instance.ChooseRandomSFXFromArray(attackGrunts));
        }

        public virtual void PlayRollSFX()
        {
            if (rollSounds.Length > 0)
            {
                Debug.Log("Playing roll sound");
                PlaySFX(WorldSFXManager.instance.ChooseRandomSFXFromArray(rollSounds));
            }
        }

        public virtual void PlayFootstepSFX()
        {
            if (footstepSounds.Length > 0)
                PlaySFX(WorldSFXManager.instance.ChooseRandomSFXFromArray(footstepSounds));
        }
    }
}