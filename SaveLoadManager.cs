using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance { get; set; }

    string highScoreKey = "BestWaveSavedValue";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ✅ Corrected
        }
    }

    public void SaveHighScore(int score)
    {
        PlayerPrefs.SetInt(highScoreKey, score);
    }

    public int LoadHighScore()
    {
        return PlayerPrefs.GetInt(highScoreKey, 0); // Cleaner fallback
    }
}