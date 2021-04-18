using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementSystem : GameSystem
{
    public Vector2 movingZone;
    public float maxSpeed = 10f;
    public float inputAcceleration;
    private Vector3 velocity;
    public float velocityDrag = 1f;
    private float speed;
    private PlayerComponent player;

    void Start()
    {
        player = StateController.Instance.GetSystem<PlayerSpawnSystem>().player;
    }

    void Update()
    {
        Debug.Log(Input.acceleration);
        LookAtMouse();
        ChangeSpeed();
    }

    private void FixedUpdate()
    {
        LookAtMouse();
        ApplySpeed();
    }

    private void ApplySpeed()
    {
        velocity = velocity * (1 - Time.deltaTime * velocityDrag);
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        var newPos = new Vector3(Mathf.Clamp(player.transform.position.x + velocity.x * Time.deltaTime, -movingZone.x / 2f, movingZone.x / 2f),
            Mathf.Clamp(player.transform.position.y + velocity.y * Time.deltaTime, -movingZone.y / 2f, movingZone.y / 2f));
        player.transform.position = newPos;
    }

    private void ChangeSpeed()
    {
        Vector3 acceleration = new Vector3(Input.GetAxis("Horizontal") * inputAcceleration, Input.GetAxis("Vertical") * inputAcceleration);
        velocity += acceleration * Time.deltaTime;
    }

    private void LookAtMouse()
    {

        Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(player.transform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        player.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, movingZone);
    }
}
