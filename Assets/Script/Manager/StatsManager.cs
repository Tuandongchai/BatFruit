using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance;

    [Header("Stat")]
    [SerializeField] private int levelCurrent;
    [SerializeField] private int Music;
    [SerializeField] private int sfx;
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
        if (!PlayerPrefs.HasKey("Music"))
            PlayerPrefs.SetInt("Music", 1);
        if (!PlayerPrefs.HasKey("SFX"))
            PlayerPrefs.SetInt("SFX", 1);
        //
        SetLevelCurrent(1);
        //
        levelCurrent = PlayerPrefs.GetInt("LevelCurrent", levelCurrent);

    }
    //level
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
    //audio
    public void ToggleMusic()
    {
        PlayerPrefs.SetInt("Music", PlayerPrefs.GetInt("Music")==0?1:0);
        PlayerPrefs.Save();
    }
    public void ToggleSFX()
    {
        PlayerPrefs.SetInt("SFX", PlayerPrefs.GetInt("SFX") == 0 ? 1 : 0);
        PlayerPrefs.Save();
    }
}
