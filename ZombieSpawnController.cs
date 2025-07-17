using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class ZombieSpawnController : MonoBehaviour
{
    public int initialZombiesPerWave = 5;
    public int currentZombiesPerWave;

    public float spawnDelay = 0.5f;

    public int currentWave = 0;
    public float waveCoolDown = 10.0f;

    public bool inCooldown;
    public float cooldownCounter = 0;

    public List<Enemy> currentZombiesAlive;

    public GameObject zombiePrefab;

    public TextMeshProUGUI WaveOverUI;
    public TextMeshProUGUI coolDownCounterUI;
    public TextMeshProUGUI currentWaveUI;

    private void Start()
    {
        currentZombiesPerWave = initialZombiesPerWave;
        StartNextWave();
    }

    private void StartNextWave()
    {
        ResumeZombieAudio(); // ✅ Unmute zombie sounds

        currentZombiesAlive.Clear();
        currentWave++;
        currentWaveUI.text = "Wave: " + currentWave.ToString();

        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < currentZombiesPerWave; i++)
        {
            Vector3 spawnOffset = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            Vector3 spawnPosition = transform.position + spawnOffset;

            var zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);

            Enemy enemyScript = zombie.GetComponent<Enemy>();
            currentZombiesAlive.Add(enemyScript);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void Update()
    {
        List<Enemy> zombiesToRemove = new List<Enemy>();

        foreach (Enemy zombie in currentZombiesAlive)
        {
            if (zombie.isDead)
            {
                zombiesToRemove.Add(zombie);
            }
        }

        foreach (Enemy zombie in zombiesToRemove)
        {
            currentZombiesAlive.Remove(zombie);
        }

        if (currentZombiesAlive.Count == 0 && !inCooldown)
        {
            StartCoroutine(WaveCoolDown());
        }

        if (inCooldown)
        {
            cooldownCounter -= Time.deltaTime;
        }
        else
        {
            cooldownCounter = waveCoolDown;
        }

        coolDownCounterUI.text = cooldownCounter.ToString("F0");
    }

    private IEnumerator WaveCoolDown()
    {
        inCooldown = true;
        WaveOverUI.gameObject.SetActive(true);

        StopZombieAudio(); // ✅ Mute all zombies

        yield return new WaitForSeconds(waveCoolDown);

        inCooldown = false;
        WaveOverUI.gameObject.SetActive(false);

        currentZombiesPerWave *= 1;

        StartNextWave();
    }

    // ✅ Mute zombie audio
    private void StopZombieAudio()
    {
        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject zombie in zombies)
        {
            AudioSource audio = zombie.GetComponent<AudioSource>();
            if (audio != null)
            {
                //audio.Stop();
                //Or:
                audio.mute = true;
            }
        }
    }

    // ✅ Unmute zombie audio
    private void ResumeZombieAudio()
    {
        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject zombie in zombies)
        {
            AudioSource audio = zombie.GetComponent<AudioSource>();
            if (audio != null)
            {
                audio.mute = false; // Or just let new zombies play sounds
            }
        }
    }
}