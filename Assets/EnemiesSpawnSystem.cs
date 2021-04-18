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
    public EEnemieName enemieName;
    public GameObject prefab;
    public int maxNumOnScreen;
}
public class EnemiesSpawnSystem : GameSystem
{
    public EnemieLoadingData[] enemieSettings;
    public bool createEnemieParent;
    public List<EnemieComponent> enemies;
    private Dictionary<EEnemieName, Queue<GameObject>> enemiesDictionary;
    private Transform enemieParent;



    private void Start()
    {
        if (createEnemieParent)
        {
            var enemieParentGO = new GameObject("Enemies");
            enemieParent = enemieParentGO.transform;
        }
        var numOfEnemieSettings = enemieSettings.Length;
        enemies = new List<EnemieComponent>();
        enemiesDictionary = new Dictionary<EEnemieName, Queue<GameObject>>();
        for (int i = 0; i < numOfEnemieSettings; i++)
        {
            var enemiesQueues = new Queue<GameObject>();
            for (int j = 0; j < enemieSettings[i].maxNumOnScreen; j++)
            {
                GameObject newEnemieGO = Instantiate(enemieSettings[i].prefab) as GameObject;
                EnemieComponent enemie = newEnemieGO.GetComponent<EnemieComponent>();
                enemies.Add(enemie);
                enemiesQueues.Enqueue(newEnemieGO);
                enemie.enemieName = enemieSettings[i].enemieName;
                newEnemieGO.SetActive(false);
                if (createEnemieParent)
                {
                    newEnemieGO.transform.parent = enemieParent;
                }
            }
            enemiesDictionary.Add(enemieSettings[i].enemieName, enemiesQueues);
        }
    }

    public GameObject GetEnemie(EEnemieName enemieName)
    {
        if (enemiesDictionary.ContainsKey(enemieName))
        {
            if (enemiesDictionary[enemieName].Count == 0)
            {
                return null;
            }
            var eneimeToReturn = enemiesDictionary[enemieName].Dequeue();
            eneimeToReturn.SetActive(true);
            return eneimeToReturn;
        }
        Debug.LogError("There is no enemie with this name");
        return null;
    }

    public void SetEneimeBack(EEnemieName enemieName, GameObject enemie)
    {
        enemie.SetActive(false);
        if (enemiesDictionary.ContainsKey(enemieName))
        {
            enemiesDictionary[enemieName].Enqueue(enemie);
        }
        else
        {
            Debug.LogError("There is no enemie with this name");

        }
    }
}
