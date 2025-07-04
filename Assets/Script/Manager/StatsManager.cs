using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance;

    [Header("Stat")]
    [SerializeField] private int levelCurrent;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        if (!PlayerPrefs.HasKey("LevelCurrent"))
            PlayerPrefs.SetInt("LevelCurrent", 1);
        //
        SetLevelCurrent(1);
        //
        levelCurrent = PlayerPrefs.GetInt("LevelCurrent", levelCurrent);

    }
    public void SetLevelCurrent(int i)
    {
        levelCurrent = i;
        PlayerPrefs.SetInt("LevelCurrent", levelCurrent);
        PlayerPrefs.Save();
    } 
    public int GetLevelCurrent() => levelCurrent;

    public void GetIncreaseLevel()
    {
        levelCurrent++;
        PlayerPrefs.SetInt("LevelCurrent", levelCurrent);
        PlayerPrefs.Save();
    }
}
