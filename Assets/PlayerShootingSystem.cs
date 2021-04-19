using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootingSystem : GameSystem
{
    public EBulletName currentBullet;
    public float timeWhileDontTouchAnything;
    private PlayerComponent player;
    private bool canShoot = true;
    private void Start()
    {
        player = StateController.Instance.GetSystem<PlayerSpawnSystem>().player;
    }
    void Update()
    {
        if (canShoot && Input.GetMouseButton(0))
        {
            var bulletGO = StateController.Instance.GetSystem<BulletsSpawnSystem>().GetBullet(currentBullet);
            if (bulletGO)
            {
                Shoot(bulletGO);
            }
        }
    }

    private void Shoot(GameObject bulletGO)
    {
        var bullet = bulletGO.GetComponent<BulletComponent>();
        StartCoroutine(SusppendShooting(bullet.delayAfetShooting));
        bullet.transform.position = player.transform.position;
        bullet.velocity = player.transform.right * bullet.speed;
        float rot_z = Mathf.Atan2(bullet.velocity.y, bullet.velocity.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        StartCoroutine(SusppendCollision(timeWhileDontTouchAnything, bullet));
    }

    IEnumerator SusppendCollision(float time, BulletComponent bullet)
    {
        bullet.canContact = false;
        yield return new WaitForSeconds(time);
        bullet.canContact = true;
    }
    IEnumerator SusppendShooting(float time)
    {
        canShoot = false;
        yield return new WaitForSeconds(time);
        canShoot = true;
    }
}
