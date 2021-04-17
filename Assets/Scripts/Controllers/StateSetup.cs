using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateSetup : MonoBehaviour
{
    [SerializeField] private EGameState state;

    public EGameState GetState()
    {
        return state;
    }

    private void Start()
    {
        Debug.Log($"{state} started");
    }
}
