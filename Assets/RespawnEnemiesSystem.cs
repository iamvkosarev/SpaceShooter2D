using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class EnemieRespawnData
{
    public EEnemieName enemieName;
    public float minSpeed;
    public float maxSpeed;
    public float timeBeforeStartSpawning;
    public float spawnMinDelay;
    public float spawnMaxDelay;
}

public class RespawnEnemiesSystem : GameSystem
{
    public EnemieRespawnData[] enemieRespawnDatas;
    public Vector2 edgeOfRespawn;
    private bool[] wasEnemieStartedSpawning;
    private float[] timesSinceWaitingEnemie;
    private float[] currentSpawnDelays;
    private int numOfEnemiesToRespawn;

    private void Start()
    {
        numOfEnemiesToRespawn = enemieRespawnDatas.Length;
        wasEnemieStartedSpawning = new bool[numOfEnemiesToRespawn];
        timesSinceWaitingEnemie = new float[numOfEnemiesToRespawn];
        currentSpawnDelays = new float[numOfEnemiesToRespawn];
        for (int i = 0; i < numOfEnemiesToRespawn; i++)
        {
            SetDelay(i);
            StartCoroutine(WaintingOfStartSpawn(enemieRespawnDatas[i].timeBeforeStartSpawning, i));
        }
    }

    private void SetDelay(int i)
    {
        currentSpawnDelays[i] = UnityEngine.Random.Range(enemieRespawnDatas[i].spawnMinDelay,
                                                            enemieRespawnDatas[i].spawnMaxDelay);
    }

    IEnumerator WaintingOfStartSpawn(float timeForWait, int i)
    {
        wasEnemieStartedSpawning[i] = false;
        yield return new WaitForSeconds(timeForWait);
        timesSinceWaitingEnemie[i] = currentSpawnDelays[i];
        wasEnemieStartedSpawning[i] = true;
    }

    private void Update()
    {
        for (int i = 0; i < numOfEnemiesToRespawn; i++)
        {
            if (!wasEnemieStartedSpawning[i]) { continue; }

            if(timesSinceWaitingEnemie[i] < currentSpawnDelays[i])
            {
                timesSinceWaitingEnemie[i] += Time.deltaTime;
            }
            else
            {
                GameObject newEnemieGO = StateController.Instance.GetSystem<EnemiesSpawnerSystem>().GetEnemie(enemieRespawnDatas[i].enemieName);
                if(newEnemieGO != null)
                {
                    timesSinceWaitingEnemie[i] = 0f;
                    SetDelay(i);
                    var enemie = newEnemieGO.GetComponent<EnemieComponent>();
                    SetEnemiesProperties(enemie);
                }
            }
        }
    }

    private void SetEnemiesProperties(EnemieComponent enemieComponent)
    {
        bool spawnOnHorizontal = UnityEngine.Random.Range(0f, 1f) < 0.5f;
        bool spawnOnNegativeAxis = UnityEngine.Random.Range(0f, 1f) < 0.5f;
        SetPostion(enemieComponent, spawnOnHorizontal, spawnOnNegativeAxis);
        SetVelocity(enemieComponent, spawnOnHorizontal, spawnOnNegativeAxis);
    }

    private void SetVelocity(EnemieComponent enemieComponent, bool spawnOnHorizontal, bool spawnOnNegativeAxis)
    {
        //throw new NotImplementedException();
    }

    private void SetPostion(EnemieComponent enemieComponent, bool spawnOnHorizontal, bool spawnOnNegativeAxis)
    {
        float x, y;
        if (spawnOnHorizontal)
        {
            x = UnityEngine.Random.Range(-edgeOfRespawn.x / 2f, edgeOfRespawn.x / 2f);
            y = edgeOfRespawn.y / 2f;
            if (spawnOnNegativeAxis)
            {
                y *= -1;
            }
        }
        else
        {
            x = edgeOfRespawn.x / 2f;
            y = UnityEngine.Random.Range(-edgeOfRespawn.y / 2f, edgeOfRespawn.y / 2f);
            if (spawnOnNegativeAxis)
            {
                x *= -1;
            }
        }
        enemieComponent.transform.position = new Vector2(x, y);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, edgeOfRespawn);
    }
}
