using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EnemieComponent : DamageDealer
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
            var damageDealer = collision.GetComponent<DamageDealer>();

            OnCollision.Invoke(this, damageDealer.damage);
        }
    }

}
