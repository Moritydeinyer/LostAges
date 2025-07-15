using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class audioManager : MonoBehaviour
{
    [Header("Music SRC")]
    [SerializeField] AudioSource backgroundMusic;
    [SerializeField] AudioSource creditMusic;

    [SerializeField] private AudioSource backTBC;
    [SerializeField] private AudioSource backChronosEndfight;

    [Header("SFX SRC")]
    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private AudioMixerGroup sfxMixerGroup;
    [SerializeField] private AudioSource sfxPlayerHit;
    [SerializeField] private AudioSource sfxMoskito;

    [SerializeField] private AudioSource sfxPlayerWalk;
    public bool playWalkSound = false;

    [SerializeField] private AudioSource sfxTitleReveal;

    [SerializeField] private AudioSource sfxMonsterDeath;

    [Header("Game Data")]
    [SerializeField] private escMenuController escMC;
    private bool init;

    public void Start()
    {
        backgroundMusic.loop = true;
        backgroundMusic.volume = 0.112f;
        backgroundMusic.Play();
    }

    public void StartMoskitoSFX()
    {
        if (escMC.playerData != null && escMC.playerData.moskito == "bought")
        {
            sfxMoskito.Stop();
        }
        else
        {
            sfxMoskito.outputAudioMixerGroup = sfxMixerGroup;
            sfxMoskito.playOnAwake = false;
            sfxMoskito.loop = true;
            sfxMoskito.Play();
        }
    }

    private void Update()
    {
        try
        {
            if (escMC.gameData.story_id.Split(";").Contains("8021") && backgroundMusic.isPlaying)
            {
                init = true;
                if (init)
                {
                    init = false;
                    StartCoroutine(FadeTrack(creditMusic));
                }
            } 
            else if (escMC.storyManager.checkStoryID("K2_52"))
            {
                if (backTBC != null && !backTBC.isPlaying)
                {
                    backTBC.outputAudioMixerGroup = sfxMixerGroup;
                    backTBC.playOnAwake = false;
                    backTBC.loop = true;
                    backTBC.Play();
                }
            }
            else if (escMC.storyManager.checkStoryIDgraterThan("8016"))
            {
                if (backChronosEndfight != null && !backChronosEndfight.isPlaying)
                {
                    backChronosEndfight.outputAudioMixerGroup = sfxMixerGroup;
                    backChronosEndfight.playOnAwake = false;
                    backChronosEndfight.loop = true;
                    backChronosEndfight.Play();
                }
            }
            else if (backChronosEndfight != null && backChronosEndfight.isPlaying)
            {
                backChronosEndfight.Stop();
            }
            else if (backTBC != null && backTBC.isPlaying)
            {
                backTBC.Stop();
            }
            if (playWalkSound && !sfxPlayerWalk.isPlaying)
            {
                sfxPlayerWalk.outputAudioMixerGroup = sfxMixerGroup;
                sfxPlayerWalk.playOnAwake = false;
                sfxPlayerWalk.loop = true;
                sfxPlayerWalk.Play();
            }
            else if (!playWalkSound && sfxPlayerWalk.isPlaying)
            {
                sfxPlayerWalk.Stop();
            }
        }
        catch { }
    }

    private IEnumerator FadeTrack(AudioSource newTrack)
    {
        float timeToFade = 4f;
        float timeElapsed = 0;

        if (backgroundMusic.isPlaying)
        {
            creditMusic.Play();
            while (timeElapsed < timeToFade)
            {
                creditMusic.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
                backgroundMusic.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            backgroundMusic.Stop();
        }

    }

    public void PlayPlayerHitSFX()
    {
        sfxPlayerHit.outputAudioMixerGroup = sfxMixerGroup;
        sfxPlayerHit.playOnAwake = false;
        sfxPlayerHit.loop = false;
        sfxPlayerHit.PlayOneShot(sfxPlayerHit.clip);
    }

    public void PlayMonsterDeathSFX()
    {
        sfxMonsterDeath.outputAudioMixerGroup = sfxMixerGroup;
        sfxMonsterDeath.playOnAwake = false;
        sfxMonsterDeath.loop = false;
        sfxMonsterDeath.PlayOneShot(sfxMonsterDeath.clip);
    }

    public void PlayTitleRevealSFX()
    {
        sfxTitleReveal.outputAudioMixerGroup = sfxMixerGroup;
        sfxTitleReveal.playOnAwake = false;
        sfxTitleReveal.loop = false;
        sfxTitleReveal.PlayOneShot(sfxTitleReveal.clip);
    }
    
    
}
