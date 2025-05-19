using System;
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
        
        public void PlayRollSFX()
        {
            audioSource.PlayOneShot(WorldSFXManager.instance.rollSFX);
        }
    }
}