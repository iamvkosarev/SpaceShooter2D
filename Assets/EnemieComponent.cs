using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemieComponent : MonoBehaviour
{
    public Vector2 velocity { get; set; }
    public float speed { get; set; }
    public EEnemieName enemieName { set; get; }
    public bool wasContact { set; get; }
    public int health { set; get; }
    public bool canContact { set; get; } = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Contact!!!");
        if (canContact)
        {
            wasContact = true;
        }
    }

}
