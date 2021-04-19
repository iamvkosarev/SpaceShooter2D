using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovementSystem : GameSystem
{
    private List<BulletComponent> bullets;
    private void Start()
    {
        bullets = StateController.Instance.GetSystem<BulletsSpawnSystem>().bullets;
    }
    void Update()
    {
        foreach (var item in bullets)
        {
            if (item.gameObject.active)
            {
                item.transform.position += Time.deltaTime * new Vector3(item.velocity.x, item.velocity.y);
            }
        }
    }
}
