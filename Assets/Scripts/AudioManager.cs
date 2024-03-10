using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : SingletonClass<AudioManager>
{

    [SerializeField]
    private AudioClip explosion, shoot, emptyGun, buy, earnMoney, shotLandMiss, shotLandHit, walk, gunSwitch, levelFinished;
    [HideInInspector]
    public AudioSource explosionSource, shootSource, emptyGunSource, buySource, earnMoneySource, shotLandMissSource, shotLandHitSource, walkSource, gunSwitchSource, levelFInishedSource;
    private AudioSource  musicSource;

    public bool Walking;

    static float VolumeStatic = -1f;

    public float Volume
    {
        get
        {
            return AudioListener.volume;
        }
        set
        {
            VolumeStatic = value;
            AudioListener.volume = value;
        }
    }


    private void Update()
    {
        if (Walking)
        {
            if (!walkSource.isPlaying)
            {
                walkSource.clip = walk;
                walkSource.Play();
            }
        }
        else
        {
            walkSource.Stop();
        }
    }


    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlayMusic(AudioClip music = null)
    {
        if (music != null)
        {
            musicSource.clip = music;
        }
        musicSource.Play();
    }

    private void Start()
    {
        explosionSource = gameObject.AddComponent<AudioSource>();
        explosionSource.clip = explosion;

        shootSource = gameObject.AddComponent<AudioSource>();
        shootSource.clip = shoot;

        walkSource = gameObject.AddComponent<AudioSource>();
        walkSource.clip = walk;

        buySource = gameObject.AddComponent<AudioSource>();
        buySource.clip = buy;

        earnMoneySource = gameObject.AddComponent<AudioSource>();
        earnMoneySource.clip = earnMoney;

        shotLandMissSource = gameObject.AddComponent<AudioSource>();
        shotLandMissSource.clip = shotLandMiss;

        shotLandHitSource = gameObject.AddComponent<AudioSource>();
        shotLandHitSource.clip = shotLandHit;

        gunSwitchSource = gameObject.AddComponent<AudioSource>();
        gunSwitchSource.clip = gunSwitch;

        emptyGunSource = gameObject.AddComponent<AudioSource>();
        emptyGunSource.clip = emptyGun;

        musicSource = gameObject.AddComponent<AudioSource>();

        levelFInishedSource = gameObject.AddComponent<AudioSource>();
        levelFInishedSource.clip = levelFinished;

        musicSource.loop = true;
        musicSource.volume = 0.9f;

        explosionSource.volume = 0.4f;
        gunSwitchSource.volume = 0.3f;

        shootSource.volume = 0.1f;



        walkSource.loop = true;

        if (VolumeStatic != -1f)
        {
            Volume = VolumeStatic;
        }
        else
        {
            Volume = 0.3f;
        }
    }

}