using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CollisionSystem : MonoBehaviour
{
    public float ignoreContactTime;
    private List<EnemieComponent> enemies;
    private void Start()
    {
        enemies = StateController.Instance.GetSystem<EnemiesSpawnSystem>().enemies;

    }
    private void Update()
    {
        foreach (var item in enemies)
        {
            if (item.gameObject.active && item.wasContact)
            {
                DoContact(item);
            }
        }
    }

    private void DoContact(EnemieComponent enemie)
    {
        enemie.health--;
        if (enemie.health <= 0)
        {
            Death(enemie);
        }
        else
        {
            StartCoroutine(WaitTimeBeforeNextContact(enemie));
        }
        enemie.wasContact = false;
    }

    private void Death(EnemieComponent enemie)
    {
        StateController.Instance.GetSystem<EnemiesSpawnSystem>().SetEneimeBack(enemie.enemieName, enemie.gameObject);

    }

    IEnumerator WaitTimeBeforeNextContact(EnemieComponent enemie)
    {
        enemie.canContact = false;
        yield return new WaitForSeconds(ignoreContactTime);
        enemie.canContact = true;
    }
}
