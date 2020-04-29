using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WaveManager : Singleton<WaveManager>
{
    private int _currentWave = 0;
    private List<WaveInfo> _waves;

    private List<GameObject> _enemiesThisWave;
    private int _waveDifficulty;
    private int _currentDifficulty;
    private int _percPerSubwave;

    private List<EnemyBase> _createdEnemies;

    private void Start()
    {
        _waves = Resources.Load<WaveList>("WaveList.prefab").waves;

        UpdateWave();
    }

    private void UpdateWave()
    {
        _enemiesThisWave = _waves[_currentWave].EnemiesThatCanSpawn;
        _waveDifficulty = _waves[_currentWave].TotalDifficulty;
        _currentDifficulty = _waves[_currentWave].TotalDifficulty;
        _percPerSubwave = _waves[_currentWave].PercentageOfSpawnsPerSubwave;
    }

    private void SpawnEnemies()
    {
        int nOfEnemyDiffSpawned = 0;
        List<GameObject> enemiesToSpawn = new List<GameObject>();
        List<Vector3> spawnPoints = new List<Vector3>();

        while (nOfEnemyDiffSpawned < (_percPerSubwave * _waveDifficulty) / 100 && _currentDifficulty > 0)
        {
            int i = Random.Range(0, _enemiesThisWave.Count);
            EnemyBase ebComp = _enemiesThisWave[i].GetComponent<EnemyBase>();

            if (ebComp.Difficulty < _currentDifficulty)
            {
                //METER ENEMIGO EN UNA LISTA, GENERAR N POSICIONES DE SPAWN DENTRO DE UN CÍRCULO DE POSIBILIDADES Y METERLAS EN UNA LISTA, SPAWNEARLOS EN DICHAS POSICIONES
                enemiesToSpawn.Add(_enemiesThisWave[i]);
                spawnPoints.Add(GetSpawnPosition());

                _currentDifficulty -= ebComp.Difficulty;
            }
        }

        List<GameObject> inactiveEnemies = GetInactiveEnemies();
        for (int i = 0; i < enemiesToSpawn.Count; i++)
        {
            if (inactiveEnemies.Count > 0)
            {
                //CONVERTIR ENEMIGO INACTIVO EN ACTIVO DEL TIPO QUE QUIERO
                ConvertEnemy(inactiveEnemies[inactiveEnemies.Count - 1], enemiesToSpawn[i]);
            }
            else
            {
                GameObject go = Instantiate(enemiesToSpawn[i], spawnPoints[i], new Quaternion(0, 0, 0, 0));
            }
        }
    }
    private Vector3 GetSpawnPosition()
    {

        return Vector3.zero;
    }
    private List<GameObject> GetInactiveEnemies()
    {
        List<GameObject> inactive = new List<GameObject>();
        for (int i = 0; i < _createdEnemies.Count; i++)
        {
            GameObject checker = _createdEnemies[i].gameObject;
            if (!checker.activeInHierarchy)
            {
                inactive.Add(checker);
                break;
            }
        }

        return inactive;
    }
    private void ConvertEnemy(GameObject inactive, GameObject goal)
    {
        Component[] comps = inactive.GetComponents<Component>();

        for (int i = 0; i < comps.Length; i++)
        {
            if (!(comps[i] is Transform))
            {
                Destroy(comps[i]);
            }
        }

        comps = goal.GetComponents<Component>();
        for (int i = 0; i < comps.Length; i++)
        {
            inactive.AddComponent(comps[i].GetType());
        }
    }




    class GFG : IComparer<int>
    {
        public int Compare(int x, int y)
        {
            if (x == 0 || y == 0)
            {
                return 0;
            }

            // CompareTo() method 
            return x.CompareTo(y);

        }
    }
}
