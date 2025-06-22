using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] Transform boardPos;
    [SerializeField] GameObject[] maps = new GameObject[] { };



    private void Start()
    {
        int currentLevel = StatsManager.Instance.GetLevelCurrent();
        GameObject map = Instantiate(maps[currentLevel-1], Vector2.zero, Quaternion.identity, boardPos);
        map.transform.localPosition = Vector3.zero;
    }
}
