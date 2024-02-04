using System.Collections.Generic;
using UnityEngine;

namespace assets.code
{
    public class PlayerSound : MonoBehaviour
    {
        [SerializeField] private AudioSource? source;

        [SerializeField] private AudioClip? bottleSound;
        [SerializeField] private AudioClip? dropSound;
        [SerializeField] private AudioClip? pickSound;
        [SerializeField] private AudioClip? jumpSound;
        [SerializeField] private AudioClip? damageSound;
        [SerializeField] private AudioClip? gameOver;
        [SerializeField] private List<AudioClip> stepSounds = new();
        [SerializeField] private float stepDelay = 0.5f;


        private readonly DelayAction _stepDelay = new();
        private int _stepIndex;

        public void PlayJump()
        {
            if (source != null && jumpSound != null) source.PlayOneShot(jumpSound);
        }

        public void PlayPick()
        {
            if (source != null && pickSound != null) source.PlayOneShot(pickSound);
        }

        public void PlayDrop()
        {
            if (source != null && dropSound != null) source.PlayOneShot(dropSound);
        }

        public void PlayBottle()
        {
            if (source != null && bottleSound != null) source.PlayOneShot(bottleSound);
        }

        public void PlayDamage()
        {
            if (source != null && damageSound != null) source.PlayOneShot(damageSound);
        }

        public void PlayDeath()
        {
            if (source != null && damageSound != null) source.PlayOneShot(gameOver);
        }


        public void PlayStep(float delta)
        {
            _stepDelay.Advance(delta);

            if (_stepDelay.HasJustPassed(stepDelay))
            {
                _stepDelay.Reset();
                if (source != null)
                    if (stepSounds.Count > 0)
                    {
                        source.PlayOneShot(stepSounds[_stepIndex]);
                        _stepIndex = (_stepIndex + 1) % stepSounds.Count;
                    }
            }
        }
    }
}