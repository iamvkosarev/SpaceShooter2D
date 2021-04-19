using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollisionSystem : GameSystem
{
    public Vector2 deathZone;
    private List<BulletComponent> bullets;
    private void Start()
    {
        bullets = StateController.Instance.GetSystem<BulletsSpawnSystem>().bullets;
        for (int i = 0; i < bullets.Count; i++)
        {
            bullets[i].OnCollision += DoContact;
        }
    }

    private void Update()
    {
        for (int i = 0; i < bullets.Count; i++)
        {
            if (!bullets[i].gameObject.active) { continue; }

            var bulletPos = bullets[i].transform.position;
            if (bulletPos.x > deathZone.x / 2f ||
                bulletPos.x < -deathZone.x / 2f ||
                bulletPos.y > deathZone.y / 2f ||
                bulletPos.y < -deathZone.y / 2f)
            {
                DoContact(bullets[i]);
            }
        }
    }

    private void DoContact(BulletComponent bullet)
    {
        StateController.Instance.GetSystem<BulletsSpawnSystem>().SetEneimeBack(bullet.bulletName, bullet.gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, deathZone);
    }
}
