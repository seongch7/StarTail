using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    AudioSource bgm_player;
    AudioSource sfx_player;
    public Slider master_slider;
    public Slider bgm_slider;
    public Slider sfx_slider;
    void Awake()
    {
        bgm_player = GameObject.Find("BGM Player").GetComponent<AudioSource>();
        sfx_player = GameObject.Find("SFX Player").GetComponent<AudioSource>();

        master_slider = master_slider.GetComponent<Slider>();
        bgm_slider = bgm_slider.GetComponent<Slider>();
        sfx_slider = sfx_slider.GetComponent<Slider>();

        //bgm_slider.onValueChanged.AddListener(ChangeMasterSound);
        //sfx_slider.onValueChanged.AddListener(ChangeMasterSound);
        bgm_slider.onValueChanged.AddListener(ChangeBgmSound);
        sfx_slider.onValueChanged.AddListener(ChangeSfxSound);
    }

    public void ChangeMasterSound(float value)
    {
        bgm_player.volume = value;
        sfx_player.volume = value;
    }
    void ChangeBgmSound(float value)
    {
        bgm_player.volume = value;
    }

    void ChangeSfxSound(float value)
    {
        sfx_player.volume = value;
    }

    public void OnSfx()
    {
        sfx_player.Play();
    }
    
}
