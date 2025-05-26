using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace DKC
{
    public class WorldSFXManager : MonoBehaviour
    {
        public static WorldSFXManager instance;

        [Header("boss track")]
        [SerializeField] AudioSource bossLoopPlayer;

        [Header("Damage SFX")]
        public AudioClip[] physicalDamageSFX;

        [Header("Action SFX")]
        public AudioClip rollSFX;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void PlayBossLoop(AudioClip clip)
        {
            if (bossLoopPlayer != null && clip != null)
            {
                bossLoopPlayer.clip = clip;
                bossLoopPlayer.loop = true;
                bossLoopPlayer.volume = 0.6f; // Set volume to a reasonable level
                bossLoopPlayer.pitch = 1.0f; // Set pitch to normal
                bossLoopPlayer.Play();
            }
        }

        public void StopBossMusic()
        {
            StartCoroutine(FadeOut());
        }

        private IEnumerator FadeOut()
        {
            while (bossLoopPlayer.volume > 0)
            {
                bossLoopPlayer.volume -= Time.deltaTime; // Adjust the fade-out speed as needed
                yield return null;
            }

            bossLoopPlayer.Stop();
        }

        public AudioClip ChooseRandomSFXFromArray(AudioClip[] array)
        {
            int index = Random.Range(0, array.Length);
            return array[index];
        }
    }
}