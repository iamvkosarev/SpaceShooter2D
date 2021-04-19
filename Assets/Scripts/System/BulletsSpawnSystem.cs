using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EBulletName
{
    Bullet_1

}

[System.Serializable]
public class BulletLoadingData
{
    public EBulletName bulletName;
    public GameObject prefab;
    public int maxNumOnScreen;
    public float delayAfterShooting;
    public int damage;
    public float speed;
}

public class BulletsSpawnSystem : GameSystem
{
    public BulletLoadingData[] bulletsData;
    public bool createParent;
    public List<BulletComponent> bullets;
    private Dictionary<EBulletName, Queue<GameObject>> bulletPool;
    private Transform bulletsParent;

    private void Start()
    {
        if (createParent)
        {
            var enemieParentGO = new GameObject("Bullets");
            bulletsParent = enemieParentGO.transform;
        }
        var numOfEnemieSettings = bulletsData.Length;
        bullets = new List<BulletComponent>();
        bulletPool = new Dictionary<EBulletName, Queue<GameObject>>();
        for (int i = 0; i < numOfEnemieSettings; i++)
        {
            var bulletsQueues = new Queue<GameObject>();
            for (int j = 0; j < bulletsData[i].maxNumOnScreen; j++)
            {
                GameObject newBulletGO = Instantiate(bulletsData[i].prefab) as GameObject;
                BulletComponent bullet = newBulletGO.GetComponent<BulletComponent>();
                bullets.Add(bullet);
                bulletsQueues.Enqueue(newBulletGO);
                bullet.bulletName = bulletsData[i].bulletName;
                bullet.damage = bulletsData[i].damage;
                bullet.speed = bulletsData[i].speed;
                bullet.delayAfetShooting = bulletsData[i].delayAfterShooting;
                newBulletGO.SetActive(false);
                if (createParent)
                {
                    newBulletGO.transform.parent = bulletsParent;
                }
            }
            bulletPool.Add(bulletsData[i].bulletName, bulletsQueues);
        }
    }
    public GameObject GetBullet(EBulletName bulletName)
    {
        if (bulletPool.ContainsKey(bulletName))
        {
            if (bulletPool[bulletName].Count == 0)
            {
                return null;
            }
            var bulletToReturn = bulletPool[bulletName].Dequeue();
            bulletToReturn.SetActive(true);
            return bulletToReturn;
        }
        Debug.LogError("There is no bullet with this name");
        return null;
    }

    public void SetEneimeBack(EBulletName bulletName, GameObject bullet)
    {
        bullet.SetActive(false);
        if (bulletPool.ContainsKey(bulletName))
        {
            bulletPool[bulletName].Enqueue(bullet);
        }
        else
        {
            Debug.LogError("There is no bullet with this name");

        }
    }
}
