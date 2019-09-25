using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] BGMs;
    public List<AudioSource> SFXs;
    public AudioClip[] clips;
    public static AudioManager instance = null;
    public float lowPitch = 0.80f;
    public float highPitch = 1.20f;
    public float slowmopitch = 0.3f;
    public int bgmNo = 1;
    public float FadeTime = 5.0f;
    private bool slowed;
    private AudioSource BGM1;
    private AudioSource BGM2;
    private AudioSource BGM3;
    private bool fading;
    // private GameObject Camera;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }




    void Start()
    {
        BGM1 = GetComponent<AudioSource>();
        BGM1.clip = BGMs[0];
        BGM1.volume = 1.0f;
        BGM1.loop = false;
        BGM1.Play();
        BGM2 = gameObject.AddComponent<AudioSource>();
        BGM2.clip = BGMs[2];
        BGM2.volume = 0.0f;
        BGM2.loop = true;
        BGM3 = gameObject.AddComponent<AudioSource>();
        BGM3.clip = BGMs[3];
        BGM3.volume = 0.0f;
        BGM3.loop = true;
        SFXs = new List<AudioSource>();
        slowed = false;
        fading = false;

       // Camera = GameObject.Find("Main Camera");
    }


    public void PlayClip(int SoundNo)
    {

        AudioSource Sound = gameObject.AddComponent<AudioSource>();
        float pitch = Random.Range(lowPitch, highPitch);
        Sound.clip = clips[SoundNo];
        Sound.loop = false;
        Sound.volume = 1;
        if (slowed == true)
        { pitch *= slowmopitch; }
        Sound.pitch = pitch;

        Sound.Play();
        SFXs.Add(Sound);

    }


    // Update is called once per frame
    void Update()
    {
        if (BGM1.isPlaying == false)
        {
            BGM1.clip = BGMs[1];
            BGM1.loop = true;
            BGM1.Stop();
            BGM2.Stop();
            BGM3.Stop();
            BGM1.Play();
            BGM2.Play();
            BGM3.Play();

        }
        if (BGM1.volume == 1.0f || BGM2.volume == 1.0f || BGM3.volume == 1.0f)
        {
            fading = false;
        }

        if (Input.GetButtonDown("Jump") && BGM1.loop == true && fading == false)
        {
            
                fading = true;
                ++bgmNo;
                if (bgmNo > 3)
                {
                    bgmNo = 1;
                }


         }

        if (fading)
        {
            if (bgmNo == 2)
            {
                BGM1.volume -= Time.unscaledDeltaTime / FadeTime;
                BGM2.volume += Time.unscaledDeltaTime / FadeTime;
            }
            if (bgmNo == 3)
            {
                BGM2.volume -= Time.unscaledDeltaTime / FadeTime;
                BGM3.volume += Time.unscaledDeltaTime / FadeTime;
            }
            if (bgmNo == 1)
            {
                BGM3.volume -= Time.unscaledDeltaTime / FadeTime;
                BGM1.volume += Time.unscaledDeltaTime / FadeTime;
            }
        }










        if (Input.GetButtonDown("Slowmo") && !slowed)
        {


            foreach (AudioSource SFX in SFXs)
            {
                SFX.pitch *= slowmopitch;
            }
            slowed = true;

        }



        if (Input.GetButtonUp("Slowmo") && slowed)
        {
            foreach (AudioSource SFX in SFXs)
            {
                SFX.pitch /= slowmopitch;
            }
            slowed = false;
        }




        if (SFXs.Count > 0)
        {
            List<AudioSource> ToDelete = new List<AudioSource>();
            foreach (AudioSource SFX in SFXs)
            {
                if(!SFX.isPlaying)
                {
                    ToDelete.Add(SFX);
                }
            }
            foreach (AudioSource SFX in ToDelete)
            {
                    SFXs.Remove(SFX);
                    Destroy(SFX);
            }

        }
    }


    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        Debug.Log("Fade Out");
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume* Time.unscaledDeltaTime / FadeTime;
        }
        yield return null;
    }

    public static IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
    {
        Debug.Log("Fade in");
        float startVolume = audioSource.volume;
        audioSource.volume = 0.05f;
        while (audioSource.volume < 1)
        {
            audioSource.volume += startVolume * Time.unscaledDeltaTime / FadeTime;
        }
        yield return null;
    }


}
