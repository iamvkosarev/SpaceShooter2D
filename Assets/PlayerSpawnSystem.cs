using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnSystem : GameSystem
{
    public GameObject playerPrefab;
    public PlayerComponent player { set; get; }

    private void Start()
    {
        var playerGO = Instantiate(playerPrefab);
        player = playerGO.GetComponent<PlayerComponent>();
    }
}
