using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Wave
{
    public AudioClip clip;
    public Color tint;
    [Space]
    public float waveSpawnOffset;
    public int waveSpawnSize;
    [Header("Enemy Spawning")]
    public int basicShrooms;
    public int tankShrooms;
    public int splitShrooms;
    public int boomShrooms;
    public int rangedShrooms;
    public int bossShrooms;
}

public class LevelManager : SingletonClass<LevelManager>
{
    public List<Wave> waves;
    Wave wave;
    public int currentWave = -1;

    bool waveActive = false;
    float currentOffset = 0;

    public List<ShroomTypeObject> shroomsToSpawn;

    public GameObject button;

    public AudioClip endlessmusic;

    public GameObject WinScreen;

    Material skybox;

    GameManager gm;

    private void Start()
    {
        gm = GameManager.Instance;
    }

    private void Update()
    {
        IsWaveFinished();
        button.SetActive(!waveActive);
        if (waveActive)
            UpdateWave();
    }

    public void PrepareWave()
    {
        if(waveActive)
            return;
        gm.StartLevel();
        button.SetActive(false);
        currentWave++;
        if (currentWave == waves.Count)
            wave = PrepareEndless();
        else
            wave = waves[currentWave];

        if (wave.clip != null)
            AudioManager.Instance.PlayMusic(wave.clip);
        else
            AudioManager.Instance.PlayMusic();

        if (wave.tint.a != 0)
            RenderSettings.skybox.SetColor("_Tint", wave.tint);

        shroomsToSpawn = new List<ShroomTypeObject>();
        for (int i = 0; i < wave.basicShrooms; i++)
            shroomsToSpawn.Add(gm.shroomTypes[UnityEngine.Random.Range(0, 2)]);
        for (int i = 0; i < wave.tankShrooms; i++)
            shroomsToSpawn.Add(gm.shroomTypes[2]);
        for (int i = 0; i < wave.boomShrooms; i++)
            shroomsToSpawn.Add(gm.shroomTypes[3]);
        for (int i = 0; i < wave.splitShrooms; i++)
            shroomsToSpawn.Add(gm.shroomTypes[4]);
        for (int i = 0; i < wave.rangedShrooms; i++)
            shroomsToSpawn.Add(gm.shroomTypes[UnityEngine.Random.Range(5,7)]);
        for (int i = 0; i < wave.bossShrooms; i++)
            shroomsToSpawn.Add(gm.BOSS);

        Shuffle(shroomsToSpawn);

        waveActive = true;
    }

    private void UpdateWave()
    {
        currentOffset += Time.deltaTime;
        if (shroomsToSpawn.Count == 0 || currentOffset < wave.waveSpawnOffset )
            return;

        currentOffset = 0;
        int spawns = Math.Min(shroomsToSpawn.Count, wave.waveSpawnSize);
        for (int i = 0; i < spawns; i++)
        {
            gm.AddToSpawnQueue(shroomsToSpawn[0]);
            shroomsToSpawn.RemoveAt(0);
        }


    }

    public bool IsWaveFinished()
    {
        var prev = waveActive;
        waveActive = shroomsToSpawn.Count != 0 || GameManager.Instance.shroomsAlive != 0;
        if(prev && !waveActive)
        {
            if(currentWave == waves.Count - 1)
            {
                WinScreen.SetActive(true);
            }
            AudioManager.Instance.levelFInishedSource.Play();
            AudioManager.Instance.StopMusic();
        }
        return !waveActive;
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int r = UnityEngine.Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[r];
            list[r] = temp;
        }
        return;
    }

    private Wave PrepareEndless()
    {
        int shrooms = 10 + currentWave * 2;
        Wave wave = new Wave();
        wave.tint = new Color(0.5f, 0.5f, 0.5f,1);
        wave.waveSpawnOffset = 1;
        wave.waveSpawnSize = currentWave - 8;

        for (int i = 0; i < shrooms; i++)
        {
            float type = UnityEngine.Random.Range(0, 10);
            if(type < 4)
                wave.basicShrooms++; //4
            else if (type < 5)
                wave.tankShrooms++; //1
            else if (type < 7)
                wave.boomShrooms++; //2
            else if (type < 8)
                wave.splitShrooms++; //1
            else
                wave.rangedShrooms++; //2
        }
        
        wave.clip = endlessmusic;

        return wave;
    }
}
