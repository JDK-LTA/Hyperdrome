using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveInfo
{
    [SerializeField] private int totalDifficulty;
    [SerializeField] private List<GameObject> enemiesThatCanSpawn;
    [Range(1, 100)]
    [SerializeField] private int percentageOfSpawnsPerSubwave;
    public int TotalDifficulty { get => totalDifficulty; set => totalDifficulty = value; }
    public List<GameObject> EnemiesThatCanSpawn { get => enemiesThatCanSpawn; set => enemiesThatCanSpawn = value; }
    public int PercentageOfSpawnsPerSubwave { get => percentageOfSpawnsPerSubwave; set => percentageOfSpawnsPerSubwave = value; }
}
