using UnityEngine;


namespace DKC
{
    public class CharacterSFXManager : MonoBehaviour
    {
        private AudioSource audioSource;

        [Header("Damage Grunts")]
        [SerializeField] protected AudioClip[] damageGrunts;

        [Header("Attack Grunts")]
        [SerializeField] protected AudioClip[] attackGrunts;

        [Header("Roll SFX")]
        [SerializeField] protected AudioClip[] rollSounds;

        protected void Awake()
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
            PlaySFX(WorldSFXManager.instance.ChooseRandomSFXFromArray(attackGrunts));
        }

        public virtual void PlayRollSFX()
        {
            Debug.Log("Playing roll sound");
            PlaySFX(WorldSFXManager.instance.ChooseRandomSFXFromArray(rollSounds));
        }
    }
}