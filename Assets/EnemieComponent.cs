using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EnemieComponent : MonoBehaviour
{
    public Vector2 velocity { get; set; }
    public float speed { get; set; }
    public EEnemieName enemieName { set; get; }
    public bool wasContact { set; get; }
    public int health { set; get; }
    public bool canContact { set; get; } = true;

    public event Action<EnemieComponent, int> OnCollision;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canContact)
        {
            var bulletComponent = collision.GetComponent<BulletComponent>();
            if (bulletComponent)
            {
                OnCollision.Invoke(this, bulletComponent.damage);
            }
            else
            {
                OnCollision.Invoke(this, 1);
            }
        }
    }

}
