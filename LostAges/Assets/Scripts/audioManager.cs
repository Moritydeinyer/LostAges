using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class audioManager : MonoBehaviour
{
    [SerializeField] AudioSource backgroundMusic;
    [SerializeField] AudioSource creditMusic;
    [SerializeField] private escMenuController escMC;
    private bool init;

    public void Start()
    {
        backgroundMusic.loop = true;
        backgroundMusic.Play();
    }

    private void Update()
    {
        try 
        {
            if (escMC.gameData.story_id.Split(";").Contains("1000") && backgroundMusic.isPlaying)
            {
            init = true;
            if (init) {
                    init = false;
                    StartCoroutine(FadeTrack(creditMusic));
            }
            }
        } catch {}
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
    
}
