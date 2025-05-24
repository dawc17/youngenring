using UnityEngine;


namespace DKC
{
    public class CharacterSFXManager : MonoBehaviour
    {
        private AudioSource audioSource;

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

        public void PlayRollSFX()
        {
            audioSource.PlayOneShot(WorldSFXManager.instance.rollSFX);
        }
    }
}