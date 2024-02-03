using System.Collections.Generic;
using UnityEngine;

namespace assets.code
{
    public class Music : MonoBehaviour
    {
        [SerializeField] AudioSource musicSource;
        [SerializeField] AudioSource interactionSource;

        [SerializeField] private AudioClip? mainSoundtrack;
        [SerializeField] private AudioClip? beginSoundtrack;
        [SerializeField] private AudioClip? fastSoundtrack1;
        [SerializeField] private AudioClip? fastSoundtrack2;
        [SerializeField] private AudioClip? gameOver;
        [SerializeField] private AudioClip? winGame;
        [SerializeField] private AudioClip? finishBrewing;

        void Start()
        {
            if(musicSource != null && mainSoundtrack != null)
            {
                musicSource.PlayOneShot(mainSoundtrack);
            }
        }

        public void PlayMainSoundtrack()
        {
            if(musicSource != null && mainSoundtrack != null)
            {
                musicSource.PlayOneShot(mainSoundtrack);
            }
        }

        public void PlayFastSoundtrack1()
        {
            if(musicSource != null && fastSoundtrack1 != null)
            {
                musicSource.PlayOneShot(fastSoundtrack1);
            }
        }

        public void PlayFastSoundtrack2()
        {
            if(musicSource != null && fastSoundtrack2 != null)
            {
                musicSource.PlayOneShot(fastSoundtrack2);
            }
        }

        public void PlayGameOver()
        {
            if(interactionSource != null && gameOver != null)
            {
                interactionSource.PlayOneShot(gameOver);
            }
        }

        public void PlayWinGame()
        {
            if(interactionSource != null && winGame != null)
            {
                interactionSource.PlayOneShot(winGame);
            }
        }

        public void PlayFinishBrewing()
        {
            if(musicSource != null && finishBrewing != null)
            {
                musicSource.PlayOneShot(finishBrewing);
            }
        }
    }
}