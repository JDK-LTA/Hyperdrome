using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveInfo
{
    [SerializeField] private int totalDifficulty;
    [SerializeField] private List<GameObject> enemiesThatCanSpawn;
    [SerializeField] private List<EnemyBase> enemiesThisWave;
    [SerializeField] private List<Vector3> positionsToSpawn;
    [Range(1, 100)]
    [SerializeField] private int percentageOfSpawnsPerSubwave;
    public int TotalDifficulty { get => totalDifficulty; set => totalDifficulty = value; }
    public List<GameObject> EnemiesThatCanSpawn { get => enemiesThatCanSpawn; set => enemiesThatCanSpawn = value; }
    public List<EnemyBase> EnemiesThisWave { get => enemiesThisWave; set => enemiesThisWave = value; }
    public int PercentageOfSpawnsPerSubwave { get => percentageOfSpawnsPerSubwave; set => percentageOfSpawnsPerSubwave = value; }
    public List<Vector3> PositionsToSpawn { get => positionsToSpawn; set => positionsToSpawn = value; }
}
