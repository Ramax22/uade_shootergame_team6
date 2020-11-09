using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Current Wave Info")]
    [SerializeField] private GameObject enemy;
    [SerializeField] public int totalEnemies;

    [Header("Variables")]
    [SerializeField] private SpawnState state = SpawnState.WAITING;
    [SerializeField] private int currentWave;
    [SerializeField] private int nextWave;
    [SerializeField] public float searchTimer;
    [SerializeField] private float originalTimer;
    [SerializeField] private bool foundEnemy;
    [SerializeField] private bool newWave;

    public enum SpawnState { SPAWNING, WAITING }
    [SerializeField] public Wave[] waves;

    public class Wave
    {
        [SerializeField] public GameObject enemyToSpawn;
        [SerializeField] public int minEnemies;
        [SerializeField] public int maxEnemies;
        [SerializeField] public float spawnRate;
    }

    public void Start()
    {
        originalTimer = searchTimer;
    }

    public void Update()
    {
        if (state == SpawnState.WAITING)
        {
            if (!EnemyIsAlive() && !newWave)
            {
                WaveCompleted();
                newWave = true;
            }

            else
            {
                return;
            }
        }

        if (state != SpawnState.SPAWNING)
        {
            StartCoroutine(SpawnWave(waves[currentWave]));
        }
    }

    IEnumerator SpawnWave(Wave waveNumber) // Spawn new wave.
    {
        state = SpawnState.SPAWNING;
        newWave = true;
        totalEnemies = Random.Range(waveNumber.minEnemies, waveNumber.maxEnemies);

        for (int i = 0; i < totalEnemies; i++)
        {
            SpawnEnemy(enemy);
            yield return new WaitForSeconds(waveNumber.spawnRate);
        }

        //Debug.Log("Finished Spawning");
        state = SpawnState.WAITING;
        newWave = false;
    }

    public void SpawnEnemy(GameObject spawnEnemy)
    {
        // Call Method to Instantiate;
    }

    public bool EnemyIsAlive()
    {
        searchTimer -= Time.deltaTime;
        if (searchTimer <= 0f)
        {
            //Debug.Log("Search");
            if (GameObject.FindGameObjectWithTag("Virus") == null)
            {
                //Debug.Log("Nothing is Alive");
                foundEnemy = false;
                return false;
            }

            if (GameObject.FindGameObjectWithTag("Virus") != null)
            {
                //Debug.Log("Enemy Alive");
                foundEnemy = true;
            }
            searchTimer = originalTimer;
        }
        return true;
    }

    // TENGO QUE FIJARME EN ESTO !!!
    public void WaveCompleted()
    {
        currentWave = nextWave;
        if (nextWave + 1 > waves.Length - 1)
        {
            nextWave = 0;
        }
        nextWave += 1;
    }
}
