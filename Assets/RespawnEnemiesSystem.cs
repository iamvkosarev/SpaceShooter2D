using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class EnemieRespawnData
{
    public EEnemieName enemieName;
    public int health = 1;
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
    private List<EnemieComponent> enemies;
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
        enemies = StateController.Instance.GetSystem<EnemiesSpawnSystem>().enemies;
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
        Spawning();
        KillInSpawnZone();
    }

    private void KillInSpawnZone()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (!enemies[i].gameObject.active) { continue; }

            var enemiesPos = enemies[i].transform.position;
            if (enemiesPos.x > edgeOfRespawn.x / 2f  ||
                enemiesPos.x < -edgeOfRespawn.x / 2f ||
                enemiesPos.y > edgeOfRespawn.y / 2f ||
                enemiesPos.y < -edgeOfRespawn.y / 2f    )
            {
                StateController.Instance.GetSystem<EnemiesSpawnSystem>().SetEneimeBack(enemies[i].enemieName, enemies[i].gameObject);
            }

        }
    }

    private void Spawning()
    {
        for (int i = 0; i < numOfEnemiesToRespawn; i++)
        {
            if (!wasEnemieStartedSpawning[i]) { continue; }

            if (timesSinceWaitingEnemie[i] < currentSpawnDelays[i])
            {
                timesSinceWaitingEnemie[i] += Time.deltaTime;
            }
            else
            {
                GameObject newEnemieGO = StateController.Instance.GetSystem<EnemiesSpawnSystem>().GetEnemie(enemieRespawnDatas[i].enemieName);
                if (newEnemieGO != null)
                {
                    timesSinceWaitingEnemie[i] = 0f;
                    SetDelay(i);
                    var enemie = newEnemieGO.GetComponent<EnemieComponent>();
                    enemie.health = enemieRespawnDatas[i].health;
                    enemie.speed = UnityEngine.Random.Range(enemieRespawnDatas[i].minSpeed, enemieRespawnDatas[i].maxSpeed);
                    SetEnemiesProperties(enemie);
                }
            }
        }
    }

    private void SetEnemiesProperties(EnemieComponent enemieComponent)
    {
        bool spawnOnHorizontal = UnityEngine.Random.Range(0f, 1f) < 0.5f;
        bool spawnOnNegativeXAxis = UnityEngine.Random.Range(0f, 1f) < 0.5f;
        bool spawnOnNegativeYAxis = UnityEngine.Random.Range(0f, 1f) < 0.5f;
        SetPostion(enemieComponent, spawnOnHorizontal, spawnOnNegativeXAxis, spawnOnNegativeYAxis);
        SetVelocity(enemieComponent, spawnOnHorizontal, spawnOnNegativeXAxis, spawnOnNegativeYAxis);
    }

    private void SetVelocity(EnemieComponent enemieComponent, bool spawnOnHorizontal, bool spawnOnNegativeXAxis, bool spawnOnNegativeYAxis)
    {

        Vector3 diagonalPos = new Vector3(-enemieComponent.transform.position.x, -enemieComponent.transform.position.y);
        var diagonal = diagonalPos - enemieComponent.transform.position;
        var perpendicularToDiagonal = Vector2.Perpendicular(diagonal).normalized* UnityEngine.Random.Range(1f, 4f);
        var velocity = (diagonal + new Vector3(perpendicularToDiagonal.x, perpendicularToDiagonal.y)* (UnityEngine.Random.Range(0f, 1f) < 0.5f ? -1 : 1)).normalized * enemieComponent.speed;
        enemieComponent.velocity = velocity;
        float rot_z = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        enemieComponent.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
    }

    private void SetPostion(EnemieComponent enemieComponent, bool spawnOnHorizontal, bool spawnOnNegativeXAxis, bool spawnOnNegativeYAxis)
    {
        float x, y;
        if (spawnOnHorizontal)
        {
            x = UnityEngine.Random.Range(0f, edgeOfRespawn.x / 2f);
            y = edgeOfRespawn.y / 2f;
            if (spawnOnNegativeYAxis)
            {
                y *= -1;
            }
            if (spawnOnNegativeXAxis)
            {
                x *= -1;
            }
        }
        else
        {
            x = edgeOfRespawn.x / 2f;
            y = UnityEngine.Random.Range(0f, edgeOfRespawn.y / 2f);
            if (spawnOnNegativeXAxis)
            {
                x *= -1;
            }
            if (spawnOnNegativeXAxis)
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
