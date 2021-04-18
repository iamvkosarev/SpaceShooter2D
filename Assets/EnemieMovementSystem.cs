using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemieMovementSystem : GameSystem
{
    private List<EnemieComponent> enemies;
    private void Start()
    {
        enemies = StateController.Instance.GetSystem<EnemiesSpawnSystem>().enemies;
    }
    void Update()
    {
        foreach (var item in enemies)
        {
            if (item.gameObject.active)
            {
                item.transform.position += Time.deltaTime * new Vector3(item.velocity.x, item.velocity.y);
            }
        }
    }
}
