using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemieComponent : MonoBehaviour
{
    public Vector2 velocity { get; set; }
    public float speed { get; set; }

    public EEnemieName enemieName { set; get; }
}
