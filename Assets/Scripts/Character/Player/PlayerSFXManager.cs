using UnityEngine;

namespace DKC
{
    public class PlayerSFXManager : CharacterSFXManager
    {
        protected override void Awake()
        {
            base.Awake();

            audioSource = GetComponent<AudioSource>();
            audioSource.volume = 2f;
        }
    }
}