using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemieCollisionSystem : GameSystem
{
    private List<EnemieComponent> enemies;
    private void Start()
    {
        enemies = StateController.Instance.GetSystem<EnemiesSpawnSystem>().enemies;
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].OnCollision += DoContact;
        }
    }
    

    private void DoContact(EnemieComponent enemie, int damage)
    {
        enemie.health -= damage;
        if (enemie.health <= 0)
        {
            Death(enemie);
        }
        enemie.wasContact = false;
    }

    private void Death(EnemieComponent enemie)
    {
        StateController.Instance.GetSystem<EnemiesSpawnSystem>().SetEneimeBack(enemie.enemieName, enemie.gameObject);

    }

}
