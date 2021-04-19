using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EEnemieName
{
    Spaceship_1,
    Meteor_1,
    Meteor_2,

}

[System.Serializable]
public class EnemieLoadingData
{
    public EEnemieName enemiesName;
    public GameObject prefab;
    public int health;
    public int maxNumOnScreen;
    public int damage;
}
public class EnemiesSpawnSystem : GameSystem
{
    public EnemieLoadingData[] enemieData;
    public bool createParent;
    public List<EnemieComponent> enemies;
    private Dictionary<EEnemieName, Queue<GameObject>> enemiesPool;
    private Transform enemieParent;



    private void Start()
    {
        if (createParent)
        {
            var enemieParentGO = new GameObject("Enemies");
            enemieParent = enemieParentGO.transform;
        }
        var numOfEnemieSettings = enemieData.Length;
        enemies = new List<EnemieComponent>();
        enemiesPool = new Dictionary<EEnemieName, Queue<GameObject>>();
        for (int i = 0; i < numOfEnemieSettings; i++)
        {
            var enemiesQueues = new Queue<GameObject>();
            for (int j = 0; j < enemieData[i].maxNumOnScreen; j++)
            {
                GameObject newEnemieGO = Instantiate(enemieData[i].prefab) as GameObject;
                EnemieComponent enemie = newEnemieGO.GetComponent<EnemieComponent>();
                enemies.Add(enemie);
                enemiesQueues.Enqueue(newEnemieGO);
                enemie.enemieName = enemieData[i].enemiesName;
                enemie.health = enemieData[i].health;
                enemie.damage = enemieData[i].damage;
                newEnemieGO.SetActive(false);
                if (createParent)
                {
                    newEnemieGO.transform.parent = enemieParent;
                }
            }
            enemiesPool.Add(enemieData[i].enemiesName, enemiesQueues);
        }
    }

    public GameObject GetEnemie(EEnemieName enemieName)
    {
        if (enemiesPool.ContainsKey(enemieName))
        {
            if (enemiesPool[enemieName].Count == 0)
            {
                return null;
            }
            var eneimeToReturn = enemiesPool[enemieName].Dequeue();
            eneimeToReturn.SetActive(true);
            return eneimeToReturn;
        }
        Debug.LogError("There is no enemie with this name");
        return null;
    }

    public void SetEneimeBack(EEnemieName enemieName, GameObject enemie)
    {
        enemie.SetActive(false);
        if (enemiesPool.ContainsKey(enemieName))
        {
            enemiesPool[enemieName].Enqueue(enemie);
        }
        else
        {
            Debug.LogError("There is no enemie with this name");

        }
    }
}
