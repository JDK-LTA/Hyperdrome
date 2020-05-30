using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveInfo
{
    [SerializeField] private int totalDifficulty;
    [SerializeField] private List<EnemyBase> enemiesThisWave;
    [SerializeField] private float cdBetweenEnemiesSpawn = 0.75f;
    [SerializeField] private List<EnemyBase> goldenEnemiesThisWave;
    [SerializeField] private int goldenDifficulty;
    [SerializeField] private float cdBetweenGoldenSpawn = 5;
    [SerializeField] private List<Vector3> positionsToSpawn;

    public int TotalDifficulty { get => totalDifficulty; set => totalDifficulty = value; }
    public List<EnemyBase> EnemiesThisWave { get => enemiesThisWave; set => enemiesThisWave = value; }
    public List<Vector3> PositionsToSpawn { get => positionsToSpawn; set => positionsToSpawn = value; }
    public List<EnemyBase> GoldenEnemiesThisWave { get => goldenEnemiesThisWave; set => goldenEnemiesThisWave = value; }
    public int GoldenDifficulty { get => goldenDifficulty; set => goldenDifficulty = value; }
    public float CdBetweenEnemiesSpawn { get => cdBetweenEnemiesSpawn; set => cdBetweenEnemiesSpawn = value; }
    public float CdBetweenGoldenSpawn { get => cdBetweenGoldenSpawn; set => cdBetweenGoldenSpawn = value; }
}
