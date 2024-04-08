using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using PixelCrushers.QuestMachine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] string volumeParameter = "MasterVolume";
    [SerializeField] AudioSource music;
    [SerializeField] AudioSource sounds;
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider soundsSlider;
    [SerializeField] AudioClip[] musicPlaylist;

    float musicVolume;
    float soundsVolume;
    bool gameStarted;

    const string PREFS_MUSIC_VOLUME = "MusicVolume";
    const string PREFS_SOUNDS_VOLUME = "EffectsVolume";

    private void Start()
    {
        StartCutSceneManager.startGameEvent.AddListener(PlayMusicOnStart);

        if (PlayerPrefs.HasKey(PREFS_MUSIC_VOLUME))
        {
            musicSlider.value = PlayerPrefs.GetFloat(PREFS_MUSIC_VOLUME);
            music.volume = musicSlider.value / musicSlider.maxValue; ;
        }
        if (PlayerPrefs.HasKey(PREFS_SOUNDS_VOLUME))
        {
            soundsSlider.value = PlayerPrefs.GetFloat(PREFS_SOUNDS_VOLUME);
            mixer.SetFloat(volumeParameter, soundsVolume);
        }
    }

    private void Update()
    {
        if (!music.isPlaying && gameStarted)
        {
            PlayMusic();
        }
    }
    public void PlayMusicOnStart()
    {
        gameStarted = true;
        PlayMusic();
    }

    public void PlaySound(AudioClip audio)
    {
        sounds.PlayOneShot(audio);
    }

    public void SetMusicVolume()
    {
        if(musicSlider != null)
        {
            musicVolume = musicSlider.value;
            PlayerPrefs.SetFloat(PREFS_MUSIC_VOLUME, musicVolume);
            music.volume = musicSlider.value / musicSlider.maxValue;
        }
        else
        {
            Debug.Log("Нет ссылки на слайдер");
        }

    }

    public void SetSoundsVolume()
    {
        if (soundsSlider != null)
        {
            soundsVolume = soundsSlider.value;
            PlayerPrefs.SetFloat(PREFS_SOUNDS_VOLUME, soundsVolume);
            mixer.SetFloat(volumeParameter, soundsVolume);
        }
        else
        {
            Debug.Log("Нет ссылки на слайдер");
        }

    }

    void PlayMusic()
    {
        music.clip = musicPlaylist[Random.Range(0, musicPlaylist.Length)];
        music.Play();
    }
}
