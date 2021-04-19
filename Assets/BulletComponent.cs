using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BulletComponent : GameSystem
{
    public Vector2 velocity { get; set; }
    public float speed { get; set; }
    public float delayAfetShooting { get; set; }
    public EBulletName bulletName { set; get; }
    public int damage { set; get; }

    public bool canContact { set; get; } = false;
    public event Action<BulletComponent> OnCollision;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canContact)
        {
            OnCollision.Invoke(this);
        }
    }

}
